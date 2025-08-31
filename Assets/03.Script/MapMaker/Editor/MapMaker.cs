using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class MapMaker
{
    private static float stageLength = 300f;
    private const string SAVE_PATH = "Assets/05.Data/MapData";
    private const string MAKE_SCENE_NAME = "CreateScene";

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


        GUI.Box(boxRect, "박스 영역");

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

    }

}
