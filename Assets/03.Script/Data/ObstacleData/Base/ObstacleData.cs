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


