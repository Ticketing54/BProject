using System.Collections.Generic;
using UnityEngine;

public class MapData : ScriptableObject
{
    [System.Serializable]
    public class ObstacleData
    {
        public MapElement.MapMakerElementType   type;
        public Vector3      target_Position;
        public Quaternion   target_Rotation;
        public Vector3      target_Scale;
    }

    public List<ObstacleData> targetData_List = new List<ObstacleData>();
    public int stagesLvel;
}
