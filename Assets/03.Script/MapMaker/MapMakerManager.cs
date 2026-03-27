using NUnit.Framework;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UIElements;

public class MapMakerManager : MonoBehaviour
{
    public ObjectContainer objectContainer;
    public StartBox startBox;
    public GameObject ballPrefab;
    public CameraController cameraController;
    public Transform tempCreateParent;
    
    public void CreateObjstacle(DataBundle.ObstacleType _type)
    {
        if(objectContainer == null)
        {
            Debug.LogError("ObjectContainer reference is missing in MapMakerContainer.");
            return;
        }
        GameObject newObstacle =  objectContainer.GetObstacleObject(_type);
        if (newObstacle == null)
        {
            Debug.LogError("해당 Enum 에 등록된 Element Object 가 없습니다.");
        }

        GameObject newElement = GameObject.Instantiate(newObstacle, null);
        newElement.transform.position = Vector3.zero;
    }

    public void CreateObjects(float _lenght,int _count)
    {
        ClearAllObjects();
        SetStageLength(_lenght);
        CreateBall(_count); 
    }

    private void SetStageLength(float _lenght)
    {
        if (objectContainer == null)
        {
            Debug.LogError("ObjectContainer reference is missing in MapMakerContainer.");
            return;
        }

        GameObject leftwall = Instantiate(objectContainer.GetObstacleObject(DataBundle.ObstacleType.WALL),tempCreateParent);
        GameObject rightwall = Instantiate(objectContainer.GetObstacleObject(DataBundle.ObstacleType.WALL),tempCreateParent);

        leftwall.transform.position = new Vector3(5f, 0, 0);
        rightwall.transform.position = new Vector3(-5f, 0, 0);

        Vector3 scale = leftwall.transform.localScale;
        scale.y = _lenght;


        leftwall.transform.localScale = scale;
        rightwall.transform.localScale = scale;

        leftwall.transform.position = new Vector3(leftwall.transform.position.x, _lenght / 2f, leftwall.transform.position.z);  
        rightwall.transform.position = new Vector3(rightwall.transform.position.x, _lenght / 2f, rightwall.transform.position.z);   

        if (startBox == null)
        {
            Debug.LogError("StartBox reference is missing in MapMakerContainer.");
            return;
        }

        startBox.transform.position = new Vector3(0, _lenght - 5f, 0);
        cameraController.transform.position = new Vector3(0, startBox.transform.position.y, 0);
    }

    private void CreateBall(int _count)
    {
        if(objectContainer == null)
        {
            Debug.LogError("ObjectContainer reference is missing in MapMakerContainer.");
            return;
        }
    
        int count = Mathf.Clamp(_count, 1, 10);

        List<Transform> startposition = startBox.GetStartBallPosition();

        for (int i = 0; i < count; i++)
        {
            GameObject newball = Instantiate(ballPrefab);
            newball.transform.SetParent(tempCreateParent);
            newball.transform.position = startposition[i % startposition.Count].position;
        }
    }

    private void ClearAllObjects()
    {
        for (int i = tempCreateParent.childCount - 1; i >= 0; i--)
        {
            Object.DestroyImmediate(tempCreateParent.GetChild(i).gameObject);
        }
    }

}
