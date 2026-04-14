using UnityEngine;

[System.Serializable]
public class SlideBoxData : ObstacleData
{
    public bool isMoveLeft;
    public float moveSpeed;
    public int leftCount = 0;
    public int rightCount = 0;
    public DataBundle.BallColor left_TargetColor;
    public DataBundle.BallColor rightTargetColor;
    public float ratio = 0;
}