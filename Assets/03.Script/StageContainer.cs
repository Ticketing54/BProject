using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public StageData CurrentStageData{ get; private set; }

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

    public void SetStage(bool _isRestart= false)
    {
        if (_isRestart)
            return;

        foreach (string key in keyList)
        {
            int isCleared = PlayerPrefs.GetInt(key, 0); 

            if (isCleared == 0)
            {
                CurrentStageData = stageData_Dictionary[key];
                return;
            }
        }

        // AllClear 
        List<string> tempkeyList = new List<string>(keyList);

        if (CurrentStageData != null && tempkeyList.Count > 1)
        {
            tempkeyList.Remove(CurrentStageData.name);
        }   

        int randomIndex = UnityEngine.Random.Range(0, tempkeyList.Count);
        CurrentStageData = stageData_Dictionary[tempkeyList[randomIndex]];
    }

    public void SetStage(StageData _stageData) => CurrentStageData = _stageData;

}
