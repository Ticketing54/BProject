using System.Collections.Generic;
using UnityEngine;

public class SlideBox : MonoBehaviour, IObstacle
{
    [SerializeField] ReplicateRange leftRange;
    [SerializeField] ReplicateRange rightRange;
    [SerializeField][Range(0.1f, 1)] private float ratio = 0.5f;
    [SerializeField][Range(1, 10)] private int leftCount = 1;
    [SerializeField][Range(1, 10)] private int rightCount = 1;

    [SerializeField] private DataBundle.BallColor leftTargetColor;
    [SerializeField] private DataBundle.BallColor righTargetColor;

    [Header("Move Data")]
    [SerializeField] private bool isMoveLeft = true;
    [SerializeField] private float moveSpeed = 1f;


    private List<ReplicateRange> moveBoxList = new List<ReplicateRange>();
    private Vector3 moveDirection => isMoveLeft ? Vector3.left : Vector3.right;

    private void Awake()
    {
        Setup();
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
            return;

        UpdateData();
    }


    private void Update()
    {
        if (moveBoxList.Count == 0)
            return;

        if (moveBoxList == null || moveBoxList.Count == 0) return;

        // 1. 0번 박스(리더)만 직접 이동
        moveBoxList[0].transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // 2. 1번부터 마지막까지 앞 박스를 기준으로 위치 고정
        // 로컬 좌표계에서 박스의 '절반 너비'를 활용해 딱 붙입니다.
        for (int i = 1; i < moveBoxList.Count; i++)
        {
            Transform prev = moveBoxList[i - 1].transform;
            Transform curr = moveBoxList[i].transform;

            // 앞 박스의 중심점 + (앞 박스 너비의 절반) + (내 너비의 절반)
            float prevHalfWidth = prev.localScale.x * 0.5f;
            float currHalfWidth = curr.localScale.x * 0.5f;

            Vector3 newPos = curr.localPosition;

            if (isMoveLeft)
            {
                // 왼쪽 이동 시: 내 위치는 앞 박스의 오른쪽(+)에 붙어야 함
                newPos.x = prev.localPosition.x + prevHalfWidth + currHalfWidth;
            }
            else
            {
                // 오른쪽 이동 시: 내 위치는 앞 박스의 왼쪽(-)에 붙어야 함
                newPos.x = prev.localPosition.x - prevHalfWidth - currHalfWidth;
            }

            curr.localPosition = newPos;
        }

        // 3. 경계 체크 및 순환
        if (HasReachedBoundary(moveBoxList[0]))
        {
            RelayBox();
        }
    }

    private bool HasReachedBoundary(ReplicateRange _box)
    {
        float boundary = isMoveLeft ? -0.5f : 0.5f;
        float offset = isMoveLeft ? -_box.transform.localScale.x : _box.transform.localScale.x;
        offset *= 0.5f;
        boundary += offset;

        if (isMoveLeft)
            return boundary >= _box.transform.localPosition.x;

        return boundary <= _box.transform.localPosition.x;
    }

    private void RelayBox()
    {
        if (moveBoxList == null || moveBoxList.Count < 2) return;

        ReplicateRange frontBox = moveBoxList[0];
        moveBoxList.RemoveAt(0);

        moveBoxList.Add(frontBox);

        Vector3 farPos = frontBox.transform.localPosition;

        float teleportOffset = 2.0f;
        farPos.x = isMoveLeft ? teleportOffset : -teleportOffset;

        frontBox.transform.localPosition = farPos;
    }


    private void Setup()
    {
        if (leftRange == null || rightRange == null)
        {
            Debug.LogError("Range등록 필요");
            return;
        }

        ReplicateRange replicateRange_Left = GameObject.Instantiate(leftRange, this.transform);
        ReplicateRange replicateRange_Right = GameObject.Instantiate(rightRange, this.transform);

        if (TryGetComponent<ReplicateController>(out ReplicateController controller))
        {
            controller.AddBoxList(replicateRange_Right);
            controller.AddBoxList(replicateRange_Left);
        }

        ReplicateRange[] originPair = isMoveLeft ?
            new[] { leftRange, rightRange } : new[] { rightRange, leftRange };
        ReplicateRange[] instantiatePair = isMoveLeft ?
            new[] { replicateRange_Left, replicateRange_Right } : new[] { replicateRange_Right, replicateRange_Left };

        moveBoxList.AddRange(originPair);
        moveBoxList.AddRange(instantiatePair);

    }

    private void UpdateData()
    {
        if (leftRange == null)
            return;

        leftRange.UpdateData(leftTargetColor, leftCount);
        rightRange.UpdateData(righTargetColor, rightCount);


        leftRange.transform.localScale = new Vector3(ratio, 1f, 1f);
        rightRange.transform.localScale = new Vector3(1f - ratio, 1f, 1f);

        float leftPosX = -0.5f + (ratio * 0.5f);
        leftRange.transform.localPosition = new Vector3(leftPosX, 0f, 0f);


        float rightPosX = 0.5f - ((1f - ratio) * 0.5f);
        rightRange.transform.localPosition = new Vector3(rightPosX, 0f, 0f);


    }

    private void Setup(SlideBoxData _data)
    {
        transform.position = _data.position;
        transform.rotation = _data.rotation;
        transform.localScale = _data.scale;

        leftTargetColor = _data.left_TargetColor;
        righTargetColor = _data.rightTargetColor;

        leftCount = _data.leftCount;
        rightCount = _data.rightCount;

        isMoveLeft = _data.isMoveLeft;
        moveSpeed = _data.moveSpeed;

        UpdateData();
    }

    public void ApplyData(ObstacleData data)
    {
        if (data is DubbleDuplicateBoxData == false)
            return;

        Setup(data as SlideBoxData);
    }

    public ObstacleData GetObstacleData()
    {
        SlideBoxData data = new SlideBoxData();
        data.prefabtype = DataBundle.ObstacleType.DOUBLE_DUPLICATION_BOX;
        data.position = transform.position;
        data.rotation = transform.rotation;
        data.scale = transform.localScale;
        data.leftCount = leftCount;
        data.rightCount = rightCount;
        data.left_TargetColor = leftTargetColor;
        data.rightTargetColor = righTargetColor;
        data.leftCount = leftCount;
        data.rightCount = rightCount;
        data.ratio = ratio;
        data.isMoveLeft = isMoveLeft;
        data.moveSpeed = moveSpeed;
        return data;
    }
}
