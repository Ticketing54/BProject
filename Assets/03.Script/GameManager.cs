using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public enum BallColor
{
    RED,
    BLUE
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindAnyObjectByType<GameManager>();

                if(instance == null )
                    instance = new GameManager();
            }
                
            return instance;
        }
    }

    [SerializeField] private Material ballMaterial;
    [SerializeField] private Ball ballPrefab;

    private BallColor ballColor = BallColor.RED;
    public BallColor CurrentBallColor => ballColor;

    private Dictionary<GameObject, CollisionBox> createBox;

    private ObjectPool<Ball> ballPool;

    private Ball temp;

    private void Awake()
    {
        ballPool = new ObjectPool<Ball>(Pool_Create, Pool_Get, BallPool_Release, BallPool_Destroy, true, 10);


    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            temp = ballPool.Get();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            ballPool.Release(temp);
        }
    }
    #region BallPool

    private Ball Pool_Create() => Instantiate<Ball>(ballPrefab);

    private void Pool_Get(Ball _ball)
    {
        Debug.Log("Get");
    }

    private void BallPool_Release(Ball _ball)
    {
        Debug.Log("Release");
    }

    private void BallPool_Destroy(Ball _ball)
    {
        Debug.Log("Destroy");
    }

    #endregion

    public void CreateBall(GameObject _obj, CollisionBox _createBox)
    {

    }


    

}
