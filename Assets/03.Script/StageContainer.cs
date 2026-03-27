using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StageContainer : MonoBehaviour
{
    [Serializable]
    public class StageGroup
    {
        public int level;
        public StageData[] stageDataArray;
    }

    [SerializeField] private StageGroup[] levelData;
    

    public StageData GetStageData(int level)
    {
        StageGroup stageGroup = Array.Find(levelData, group => group.level == level);

        if (stageGroup != null)
        {
            return stageGroup.stageDataArray[UnityEngine.Random.Range(0, stageGroup.stageDataArray.Length)];
        }
        else
        {
            Debug.LogError($"Stage data for level {level} not found.");
            return null;
        }
    }
}
