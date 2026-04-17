using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Rigidbody boxRig;
    [SerializeField] private float moveSpeed = 0.05f;

    [Header("Start Position Data")]
    [SerializeField] private List<Transform> startPosition;
    private Sequence closeBoxAnimation;
    private Sequence dropBoxAnimation;
    private Coroutine moveBox_routine;



    #region BoxAnimation

    private void SetupCloseAnimation()
    {
        closeBoxAnimation = DOTween.Sequence().SetAutoKill(false)
            .Append(leftBoxWing.DOLocalRotate(new Vector3(90f, 0f, -90f), 0.5f)).SetUpdate(UpdateType.Fixed)
            .Join(rightBoxWing.DOLocalRotate(new Vector3(90f, 0f, 90f), 0.5f)).SetUpdate(UpdateType.Fixed)
            .Append(frontBoxWing.DOLocalRotate(new Vector3(0, 0, 0), 0.5f)).SetUpdate(UpdateType.Fixed)
            .Join(backBoxWing.DOLocalRotate(new Vector3(180f, 0f, 0f), 0.5f)).SetUpdate(UpdateType.Fixed)
            .Append(boxBody.DOLocalRotate(new Vector3(0, 0, -180), 0.5f)).SetUpdate(UpdateType.Fixed)            
            .AppendCallback(LockBalls)                              // Animation End
            .AppendCallback(StartMoveBox);                         // Animation End
    }

    private void SetupDropAnimation()
    {
        dropBoxAnimation = DOTween.Sequence().SetAutoKill(false)
            .AppendCallback(ReleaseInput)            
            .AppendInterval(0.1f)
            .Append(leftBoxWing.DOLocalRotate(new Vector3(90f, 0f, 45f), 0.5f)).SetUpdate(UpdateType.Fixed)
            .Join(rightBoxWing.DOLocalRotate(new Vector3(90f, 0f, -45f), 0.5f)).SetUpdate(UpdateType.Fixed)
            .Append(frontBoxWing.DOLocalRotate(new Vector3(130f, 0, 0), 0.5f)).SetUpdate(UpdateType.Fixed)
            .Join(backBoxWing.DOLocalRotate(new Vector3(45f, 0f, 0f), 0.5f)).SetUpdate(UpdateType.Fixed)
            .AppendCallback(BallsWakeUp)
            .AppendCallback(AnimationEnd);                         // Animation End

    }


    private void CloseBoxAnimation() => closeBoxAnimation.PlayForward();
    private void CancelBoxAnimation() => closeBoxAnimation.PlayBackwards();
    private void DropBoxAnimation() => dropBoxAnimation.PlayForward();

    #endregion

    void Start()
    {
        // Animation Setup
        SetupCloseAnimation();
        SetupDropAnimation();
    }

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetObject += ResetObject;
            GameManager.Instance.OnStartBoxSet += SetStartBox;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetObject -= ResetObject;
            GameManager.Instance.OnStartBoxSet -= SetStartBox;
        }
    }

    private List<Ball> ballList = new List<Ball>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Ball>(out Ball ball))
        {
            if (ballList.Contains(ball))
                return;

            ballList.Add(ball);

            if (GameManager.Instance.IsAllBallsEntered(ballList.Count))
            {
                RegisterInput();
            }
        }
    }


    private void RegisterInput()
    {
        GameManager.Instance.InputClickDown += OnClickDown;
        GameManager.Instance.InputClickUp += OnClickUp;
    }

    private void ReleaseInput()
    {
        GameManager.Instance.InputClickDown -= OnClickDown;
        GameManager.Instance.InputClickUp -= OnClickUp;
    }

    private void SetStartBox(List<Ball> _ballList, float _stateLength)
    {
        Vector3 startPosition = new Vector3(0, _stateLength - 5f, 0);
        transform.position = startPosition;
        boxRig.position = startPosition;

        List<Transform> startPositionList = GetStartBallPosition();

        if (_ballList.Count >= startPositionList.Count)
        {
            Debug.LogError("지정된 시작 위치보다 공의 개수가 많습니다.");
            return;
        }

        for (int i = 0; i < _ballList.Count; i++)
        {
            _ballList[i].Move(startPositionList[i].position);
        }
    }

    public List<Transform> GetStartBallPosition()
    {
        if (startPosition == null || startPosition.Count == 0)
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


    private void StartMoveBox()
    {
        if (moveBox_routine != null)
            StopCoroutine(moveBox_routine);

        moveBox_routine = StartCoroutine(CoMoveBox());
    }

    private void AnimationEnd()
    {
        GameManager.Instance?.LookatLowestBall();

        if (moveBox_routine != null)
            StopCoroutine(moveBox_routine);
    }

    private void LockBalls()
    {
        foreach(Ball ball in ballList)
        {
            ball.FixedObject(boxRig);
        }
    }

    private void BallsWakeUp()
    {
        foreach(Ball ball in ballList)
        {
            ball.RealsedObject();
        }

        ballList.Clear();
    }

    private IEnumerator CoMoveBox()
    {
        while (true)
        {
            Vector2 direction = GameManager.Instance?.Direction ?? Vector2.zero;

            if (Mathf.Abs(direction.x) < 0.1f)
            {
                yield return null;
                continue;
            }

            Vector3 destination = transform.position + new Vector3(direction.x * moveSpeed, 0, 0);

            Debug.Log(direction + " / " + destination);

            destination.x = Mathf.Clamp(destination.x, -3.3f, 3.3f);
            transform.position = destination;

            yield return null;
        }
    }

    private void ResetObject()
    {
        dropBoxAnimation.Rewind();
        closeBoxAnimation.Rewind();


        RegisterInput();
    }


    private void OnClickDown() => CloseBoxAnimation();

    private void OnClickUp()
    {
        if (moveBox_routine != null)
            DropBoxAnimation();
        else
            CancelBoxAnimation();
    }

}
