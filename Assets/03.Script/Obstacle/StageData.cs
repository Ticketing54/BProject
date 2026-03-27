using System.Collections.Generic;
using UnityEngine;

public class StageData : ScriptableObject
{
    public int stagesLvel;
    public float stageLength;
    public int goalScore;
    public int startBallCount;
    [SerializeReference] public List<ObstacleData> obstacleData = new List<ObstacleData>();
}
