using UnityEngine;
using static DataBundle;

[System.Serializable]
public abstract class ObstacleData
{
    public ObstacleType prefabtype;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
}

public interface IObstacle
{
    ObstacleData GetObstacleData();
    void ApplyData(ObstacleData data);
}

[System.Serializable]
public class StaticObstacleData : ObstacleData
{
    
}

[System.Serializable]
public class SingleDuplicationBox : ObstacleData
{
    public int count = 0;
    public DataBundle.BallColor targetColor;
}

[System.Serializable]

public class DubbleDuplicateBox : ObstacleData
{
    public int leftCount = 0;
    public int rightCount = 0;
    public DataBundle.BallColor left_TargetColor;
    public DataBundle.BallColor rightTargetColor;
    public float ratio = 0;

}


