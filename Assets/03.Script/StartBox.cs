using DG.Tweening;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;

public class StartBox : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private Transform boxBody;
    [SerializeField] private Transform leftBoxWing;
    [SerializeField] private Transform rightBoxWing;
    [SerializeField] private Transform frontBoxWing;
    [SerializeField] private Transform backBoxWing;

    [Header("Move")]
    [SerializeField] private float moveSpeed = 0.01f;

    [Header("Start Data")]
    [SerializeField] private int ballCount;
    [SerializeField] private List<Transform> startPosition;


    public Rigidbody rig;

    private Sequence closeBoxAnimation;
    private Sequence dropBoxAnimation;
    private bool isRotate = false;
    private GameManager gameManager => GameManager.Instance;

    #region BoxAnimation

    private void SetupCloseAnimation()
    {
        closeBoxAnimation = DOTween.Sequence().SetAutoKill(false)
            .Append(leftBoxWing.DOLocalRotate(new Vector3(90f, 0f, -90f), 0.5f)).SetUpdate(UpdateType.Fixed)
            .Join(rightBoxWing.DOLocalRotate(new Vector3(90f, 0f, 90f), 0.5f)).SetUpdate(UpdateType.Fixed)
            .Append(frontBoxWing.DOLocalRotate(new Vector3(0, 0, 0), 0.5f)).SetUpdate(UpdateType.Fixed)
            .Join(backBoxWing.DOLocalRotate(new Vector3(180f, 0f, 0f), 0.5f)).SetUpdate(UpdateType.Fixed)
            .Append(boxBody.DOLocalRotate(new Vector3(0, 0, -180), 0.5f)).SetUpdate(UpdateType.Fixed)
            .AppendCallback(() => isRotate = !isRotate);                         // Animation End
    }

    private void SetupDropAnimation()
    {
        dropBoxAnimation = DOTween.Sequence().SetAutoKill(false)
            .Append(leftBoxWing.DOLocalRotate(new Vector3(90f, 0f, 45f), 1)).SetUpdate(UpdateType.Fixed)
            .Join(rightBoxWing.DOLocalRotate(new Vector3(90f, 0f, -45f), 1)).SetUpdate(UpdateType.Fixed)
            .Join(frontBoxWing.DOLocalRotate(new Vector3(130f, 0, 0), 1)).SetUpdate(UpdateType.Fixed)
            .Join(backBoxWing.DOLocalRotate(new Vector3(45f, 0f, 0f), 1)).SetUpdate(UpdateType.Fixed)
            .AppendCallback(RealeaseInput)
            .AppendCallback(() => gameManager.SetCameraTarget(null));                         // Animation End

    }


    private void CloseBoxAnimation() => closeBoxAnimation.PlayForward();
    private void CancleBoxAnimation() => closeBoxAnimation.PlayBackwards();
    private void DropBoxAnimation() => dropBoxAnimation.PlayForward();

    #endregion

    void Start()
    {
        // Input Setup
        RegisterInput();

        // Animation Setup
        SetupCloseAnimation();
        SetupDropAnimation();
    }

    private void RegisterInput()
    {
        gameManager.InputClickDown += OnClickDown;
        gameManager.InputClickUp += OnClickUp;
    }

    private void RealeaseInput()
    {
        gameManager.InputClickDown -= OnClickDown;
        gameManager.InputClickUp -= OnClickUp;
    }
    private void FixedUpdate()
    {
        if (!isRotate)
            return;

        MoveBox(gameManager.Direction);
    }

    private void OnClickDown() => CloseBoxAnimation();

    private void OnClickUp()
    {
        if (isRotate)
            DropBoxAnimation();
        else
            CancleBoxAnimation();
    }

    private void MoveBox(Vector2 _direction)
    {
        if (!isRotate)
            return;

        if (Mathf.Abs(_direction.x) < 0.1f)
            return;

        //transform.position += new Vector3(_direction.x * moveSpeed, 0, 0);
        Vector3 destination = transform.position + new Vector3(_direction.x * moveSpeed, 0, 0);
        rig.MovePosition(destination);

    }

    public void SetStartPosition(float _stateLength)
    {
        transform.position = new Vector3(0, _stateLength - 5f, 0);
    }

    public List<Transform> GetStartBallPosition()
    {
        if(startPosition == null || startPosition.Count == 0)
        {
            Debug.LogError("StartBox: Start positions are not assigned.");
            return null;
        }

        List<Transform> shuffledPositions = new List<Transform>(startPosition);

        for (int i = shuffledPositions.Count - 1; i > 0; i--)
        {   
            int randomIndex = UnityEngine.Random.Range(0, i + 1);

            Transform temp = shuffledPositions[i];
            shuffledPositions[i] = shuffledPositions[randomIndex];
            shuffledPositions[randomIndex] = temp;
        }

        return shuffledPositions;
    }
}
