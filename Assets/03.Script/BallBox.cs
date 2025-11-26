using DG.Tweening;
using UnityEngine;

public class BallBox : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private Transform boxBody;
    [SerializeField] private Transform leftBoxWing;
    [SerializeField] private Transform rightBoxWing;
    [SerializeField] private Transform frontBoxWing;
    [SerializeField] private Transform backBoxWing;

    [Header("Move")]
    [SerializeField] private float moveSpeed = 0.01f;

    private Sequence closeBoxAnimation;
    private Sequence dropBoxAnimation;

    //public bool isRotate = false;

    void Start()
    {
        // Animation Setup
        SetupCloseAnimation();
        SetupDropAnimation();

        // Apply Init Input
        BallInState();
    }

    #region BoxAnimation

    private void SetupCloseAnimation()
    {
        closeBoxAnimation = DOTween.Sequence().SetAutoKill(false)
            .Append(leftBoxWing.DOLocalRotate(new Vector3(90f, 0f, -90f), 1))
            .Join(rightBoxWing.DOLocalRotate(new Vector3(90f, 0f, 90f), 1))
            .Append(frontBoxWing.DOLocalRotate(new Vector3(0, 0, 0), 1))
            .Join(backBoxWing.DOLocalRotate(new Vector3(180f, 0f, 0f), 1))
            .Append(boxBody.DOLocalRotate(new Vector3(0, 0, -180), 1))
            .AppendCallback(MoveBox);                         // Change ClickUpEvent
    }

    private void SetupDropAnimation()
    {
        dropBoxAnimation = DOTween.Sequence().SetAutoKill(false)
            .Append(leftBoxWing.DOLocalRotate(new Vector3(90f, 0f, 45f), 1))
            .Join(rightBoxWing.DOLocalRotate(new Vector3(90f, 0f, -45f), 1))
            .Join(frontBoxWing.DOLocalRotate(new Vector3(130f, 0, 0), 1))
            .Join(backBoxWing.DOLocalRotate(new Vector3(45f, 0f, 0f), 1));
    }



    private void BallInState()
    {
        InputManager.ResetAllEvent();
        InputManager.OnClickDownEvent += CloseBoxAnimation;
        InputManager.OnClickUpEvent += CancleBoxAnimation;
    }

    private void CloseBoxAnimation() => closeBoxAnimation.PlayForward();
    private void CancleBoxAnimation() => closeBoxAnimation.PlayBackwards();
    private void DropBoxAnimation()
    {
        InputManager.SetGeneralClickEvent();
        dropBoxAnimation.PlayForward();
    }

    #endregion

    private void MoveBox()
    {
        InputManager.ResetAllEvent();
        InputManager.OnDragEvent += MoveBox;
        InputManager.OnClickUpEvent += DropBoxAnimation;
    }

    private void MoveBox(Vector2 _direction)
    {
        if (Mathf.Abs(_direction.x) < 0.1f)
            return;

        transform.position += new Vector3(_direction.x * moveSpeed, 0, 0);
    }
}
