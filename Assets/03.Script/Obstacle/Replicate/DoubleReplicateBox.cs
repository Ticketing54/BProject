using UnityEngine;

public class DoubleReplicateBox : MonoBehaviour, IObstacle
{
    [SerializeField] ReplicateRange leftRange;
    [SerializeField] ReplicateRange rightRange;
    [SerializeField][Range(0.1f, 1)] private float ratio = 0.5f;
    [SerializeField][Range(1, 10)] private int leftCount = 1;
    [SerializeField][Range(1, 10)] private int rightCount = 1;

    [SerializeField] private DataBundle.BallColor leftTargetColor;
    [SerializeField] private DataBundle.BallColor righTargetColor;


    private void OnValidate()
    {
        if (Application.isPlaying)
            return;

        UpdateData();
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

    private void Setup(DubbleDuplicateBox _data)
    {
        transform.position = _data.position;
        transform.rotation = _data.rotation;
        transform.localScale = _data.scale;

        leftTargetColor = _data.left_TargetColor;
        righTargetColor = _data.rightTargetColor;

        leftCount = _data.leftCount;
        rightCount = _data.rightCount;

        UpdateData();
    }

    public void ApplyData(ObstacleData data)
    {
        if (data is DubbleDuplicateBox == false)
            return;

        Setup(data as DubbleDuplicateBox);
    }

    public ObstacleData GetObstacleData()
    {
        DubbleDuplicateBox data = new DubbleDuplicateBox();
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
        return data;
    }
}
