using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class MapMaker
{
    private static float stageLength = 300f;
    private const string SAVE_PATH = "Assets/05.Data/MapData/";
    private const string MAKE_SCENE_NAME = "CreateScene";
    private static MapElement.MapMakerElementType tempElement;

    private static MapMakerContainer mapMakerContainer;
    private static MapMakerContainer MapMakerContainer
    {
        get
        {
            if (mapMakerContainer == null)
                mapMakerContainer = GameObject.FindAnyObjectByType<MapMakerContainer>();

            return mapMakerContainer;
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
        float padding = 10f;

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

        GUILayout.Label("스테이지 길이 설정");
        stageLength = EditorGUILayout.FloatField(stageLength);

        GUILayout.Space(10);

        if (GUILayout.Button("적용"))
        {
            ApplyButton();
        }

        GUILayout.Space(10);

        GUILayout.Label("구성요소 생성", labelCenterStyle);

        tempElement = (MapElement.MapMakerElementType)EditorGUILayout.EnumPopup(tempElement,popupCenterStyle); // 드롭다운표시

        if(GUILayout.Button("생성"))
        {
            if (tempElement == MapElement.MapMakerElementType.NONE)
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
        MapElement[] mapElement = GameObject.FindObjectsByType<MapElement>(FindObjectsSortMode.None);

        foreach (MapElement one in mapElement)
        {
            if (one.ElementType != MapElement.MapMakerElementType.WALL)
                continue;

            Vector3 targetScale = one.transform.localScale;
            Vector3 targetPosition = one.transform.localPosition;

            targetScale.y = stageLength;
            targetPosition.y = stageLength * 0.5f;

            one.transform.localScale = targetScale;
            one.transform.localPosition = targetPosition;
        }

    }


    private static void SaveData()
    {
        MapData newMapData = ScriptableObject.CreateInstance<MapData>();
        MapElement[] elementDatas = GameObject.FindObjectsByType<MapElement>(FindObjectsSortMode.None);

        foreach(MapElement element in elementDatas)
        {
            MapData.ObstacleData obstacleData = new MapData.ObstacleData();
            obstacleData.type = element.ElementType;
            obstacleData.target_Position = element.transform.position;
            obstacleData.target_Rotation = element.transform.rotation;
            obstacleData.target_Scale = element.transform.lossyScale;


            newMapData.targetData_List.Add(obstacleData);
        }

        int index = 1;
        string newPath = $"{SAVE_PATH}Mapdata.asset";

        while (System.IO.File.Exists(newPath))
        {
            newPath = SAVE_PATH + "MapData_" + index + ".asset";
            index++;
        }

        AssetDatabase.CreateAsset(newMapData, newPath);
        AssetDatabase.SaveAssets();

        Debug.Log($"PlayerData saved to {newPath}");
    }

    private static void CreateMapElement()
    {
        if (MapMakerContainer == null)
        {
            Debug.LogError("MapMakerContainer 가 존재하지않아 생성 불가능!");
            return;
        }

        GameObject element = MapMakerContainer.GetElementObject(tempElement);

        if(element == null)
        {
            Debug.LogError("해당 Enum 에 등록된 Element Object 가 없습니다.");
        }

        GameObject newElement = GameObject.Instantiate(element);
        newElement.transform.SetParent(null);
        newElement.transform.position = Vector3.zero;
    }

}
