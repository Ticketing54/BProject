using UnityEngine;

public class SingleReplicateBox : MonoBehaviour, IObstacle
{
    [SerializeField] ReplicateRange replicateRange;
    [SerializeField][Range(1, 10)] private int count = 1;
    [SerializeField] private DataBundle.BallColor targetColor;

    private void OnValidate()
    {
        if (Application.isPlaying)
            return;

        UpdateData();
    }

    private void UpdateData()
    {
        if (replicateRange == null)
            return;

        replicateRange.UpdateData(targetColor, count);
    }

    private void Setup(SingleDuplicationBoxData _data)
    {
        count = _data.count;

        transform.position = _data.position;
        transform.rotation = _data.rotation;
        transform.localScale = _data.scale;

        UpdateData();
    }

    public void ApplyData(ObstacleData data)
    {
        if (data is SingleDuplicationBoxData == false)
            return;

        Setup(data as SingleDuplicationBoxData);
    }

    public ObstacleData GetObstacleData()
    {
        SingleDuplicationBoxData data = new SingleDuplicationBoxData();
        data.prefabtype = DataBundle.ObstacleType.SINGLE_DUPLICATION_BOX;
        data.position = transform.position;
        data.rotation = transform.rotation;
        data.scale = transform.localScale;
        data.count = count;
        data.targetColor = targetColor;
        return data;
    }
}
