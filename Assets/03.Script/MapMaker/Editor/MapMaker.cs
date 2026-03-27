using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class MapMaker
{
    private static int stateLevel = 1;
    private static float stageLength = 300f;
    private static int startBallCount = 1;
    private const string SAVE_PATH = "Assets/05.Data/MapData/";
    private const string MAKE_SCENE_NAME = "CreateScene";
    private static DataBundle.ObstacleType tempElement;

    private static MapMakerManager mapMakerManager;
    private static MapMakerManager MapMakerManager
    {
        get
        {
            if (mapMakerManager == null)
                mapMakerManager = GameObject.FindAnyObjectByType<MapMakerManager>();

            return mapMakerManager;
        }
    }

    static MapMaker()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private static bool IsMakeScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        return MAKE_SCENE_NAME == currentScene;
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        if (!IsMakeScene())
            return;

        Handles.BeginGUI();

        float boxWidth = 200f;
        float padding = 20f;

        Rect boxRect = new Rect(
            sceneView.position.width - boxWidth - padding,
            sceneView.position.height - 300 - padding,
            boxWidth,
            300
        );

        // Popup Style
        GUIStyle popupCenterStyle = new GUIStyle(EditorStyles.popup);
        popupCenterStyle.alignment = TextAnchor.MiddleCenter;

        GUIStyle labelCenterStyle = new GUIStyle(EditorStyles.label);
        labelCenterStyle.alignment = TextAnchor.MiddleCenter;

        GUI.Box(boxRect, "맵 생성 도구");

        GUILayout.BeginArea(boxRect);

        GUILayout.Space(20);

        GUILayout.Label("스테이지 레벨");
        stateLevel = EditorGUILayout.IntField(stateLevel);

        GUILayout.Label("스테이지 길이 설정");
        stageLength = EditorGUILayout.FloatField(stageLength);

        GUILayout.Label("시작 공갯수[1~10]");
        startBallCount = EditorGUILayout.IntField(startBallCount);

        GUILayout.Space(10);

        if (GUILayout.Button("적용"))
        {
            ApplyButton();
        }

        GUILayout.Space(10);

        GUILayout.Label("구성요소 생성", labelCenterStyle);

        tempElement = (DataBundle.ObstacleType)EditorGUILayout.EnumPopup(tempElement,popupCenterStyle); // 드롭다운표시

        if(GUILayout.Button("생성"))
        {
            if (tempElement == DataBundle.ObstacleType.NONE)
                return;

            CreateMapElement();
        }

        GUILayout.Space(10);



        GUILayout.Label($"Path : {SAVE_PATH}", EditorStyles.boldLabel);

        if (GUILayout.Button("맵정보 저장"))
        {
            SaveData();
        }

        GUILayout.EndArea();

        Handles.EndGUI();
    }

    private static void ApplyButton()
    {
        if(MapMakerManager == null)
        {
            Debug.LogError("ObjectContainer 가 존재하지않아 적용 불가능!");
            return;
        }

        startBallCount = Mathf.Clamp(startBallCount, 1, 10);

        MapMakerManager.CreateObjects(stageLength,startBallCount);
    }


    private static void SaveData()
    {   
        if (!AssetDatabase.IsValidFolder(SAVE_PATH))
        {
            string[] folders = SAVE_PATH.Split('/');
            string currentPath = folders[0];
            for (int i = 1; i < folders.Length; i++)
            {
                if (!AssetDatabase.IsValidFolder(currentPath + "/" + folders[i]))
                {
                    AssetDatabase.CreateFolder(currentPath, folders[i]);
                }
                currentPath += "/" + folders[i];
            }
        }

        StageData newStage = ScriptableObject.CreateInstance<StageData>();

        newStage.stagesLvel = stateLevel;
        newStage.stageLength = stageLength;
        newStage.startBallCount = startBallCount;

        var founds = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                           .OfType<IObstacle>();

        foreach (var obstacle in founds)
        {
            newStage.obstacleData.Add(obstacle.GetObstacleData());
        }

        string fullPath = $"{SAVE_PATH}/NewState.asset";

        fullPath = AssetDatabase.GenerateUniqueAssetPath(fullPath);

        AssetDatabase.CreateAsset(newStage, fullPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"<color=green><b>[Save Success]</b></color> {fullPath} 맵 정보가 생성되었습니다.");

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = newStage;

    }

    private static void CreateMapElement()
    {
        if (MapMakerManager == null)
        {
            Debug.LogError("objectContainer 가 존재하지않아 생성 불가능!");
            return;
        }

        mapMakerManager.CreateObjstacle(tempElement);
    }

}
