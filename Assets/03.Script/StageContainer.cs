using System;
using System.Collections.Generic;
using Unity.Mathematics;
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

    private Dictionary<string, StageData> stageData_Dictionary = new Dictionary<string, StageData>();
    private List<string> keyList = new List<string>();

    private void Start()
    {
        foreach (var group in levelData)
        {
            foreach (var data in group.stageDataArray)
            {
                keyList.Add(data.name);
                stageData_Dictionary.Add(data.name, data);
            }
        }
    }




    public StageData GetStageData()
    {
        foreach (string key in keyList)
        {
            int isCleared = PlayerPrefs.GetInt(key, 0); // 추후 수정

            if (isCleared == 0)
            {
                return stageData_Dictionary[key];
            }
        }

        // AllClear 

        int randomIndex = UnityEngine.Random.Range(0, keyList.Count);

        return stageData_Dictionary[keyList[randomIndex]];
    }

}
