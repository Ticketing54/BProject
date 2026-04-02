using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;


public class ObjectContainer : MonoBehaviour
{
    [Serializable]
    private class ObstaclePrefabData
    {
        public DataBundle.ObstacleType prefabtype = DataBundle.ObstacleType.NONE;
        public GameObject prefab ;
    }

    [Header("Ball Settings")]
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private Material ballMaterial;
    [SerializeField] private DataBundle.BallColor ballColor;

    [Header("Obstacles")]
    [SerializeField] private List<ObstaclePrefabData> prefab_List;
    [SerializeField] private Material obstacle_OrangeMaterial;
    [SerializeField] private Material obstacle_BlueMaterial;

    private List<GameObject> wall_List = new List<GameObject>();

    public DataBundle.BallColor CurrentBallColor => ballColor;

    private ObjectPool<Ball> ballPool;

    private HashSet<Ball> activeBall = new HashSet<Ball>();
    private List<GameObject> activeObstacles = new List<GameObject>();

    private Vector3 ballReleasePoint = new Vector3(-100, -100, -100);

    private void Awake()
    {
        Setup();
    }

    #region BALL

    private Ball CreateBall() => Instantiate(ballPrefab);

    private void PoolGetBall(Ball _ball)
    {
        if (!activeBall.Contains(_ball))
            activeBall.Add(_ball);
        else
            Debug.LogError(_ball.name + " is already in activeBall set.");

        _ball.gameObject.SetActive(true);
    }

    private void PoolDestroyBall(Ball _ball) => Destroy(_ball.gameObject);

    private void PoolReleaseBall(Ball _ball)
    {
        _ball.gameObject.transform.position = ballReleasePoint;
        _ball.gameObject.SetActive(false);

        if (activeBall.Contains(_ball))
            activeBall.Remove(_ball);
        else
            Debug.LogError(_ball.name + " is not found in activeBall set.");

    }

    private void Setup()
    {
        if (ballPrefab == null)
        {   
            Debug.LogError("Ball Prefab is not assigned in the inspector.");
            return;
        }

        activeBall = new HashSet<Ball>();
        ballPool = new ObjectPool<Ball>(
            createFunc: CreateBall,
            actionOnGet: PoolGetBall,
            actionOnRelease: this.PoolReleaseBall,
            actionOnDestroy: PoolDestroyBall,
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 20
        );

        if (ballMaterial == null)
        {
            Debug.LogError("Material component is missing on BallPrefab's MeshRenderer.");
            return;
        }

        ballMaterial.color = DataBundle.GetColor(ballColor);
    }

    public void ChangeBallColor()
    {
        ballColor = ballColor == DataBundle.BallColor.ORANGE ? DataBundle.BallColor.BLUE : DataBundle.BallColor.ORANGE;

        ballMaterial.color = DataBundle.GetColor(ballColor);
    }

    public Ball GetBall(Vector3 _position)
    {
        if (ballPool == null)
        {
            Debug.LogError("Ball Pool is not initialized.");
            return null;
        }

        Ball newBall = ballPool.Get();

        newBall.Move(_position);

        return newBall;
    }

    public void ReleaseBall(Ball _ball)
    {
        if (ballPool == null)
        {
            Debug.LogError("Ball Pool is not initialized.");
            return;
        }

        _ball.gameObject.SetActive(false);

        ballPool.Release(_ball);
    }

    public List<Ball> ActiveBallList()
    {
        if (activeBall == null || activeBall.Count == 0)
            return null;

        return activeBall.ToList();

    }

    public void ReleaseAllActiveBalls()
    {
        if (activeBall == null || activeBall.Count == 0)
            return;

        foreach (Ball ball in activeBall.ToList())
        {
            ReleaseBall(ball);
        }
    }

    #endregion

    #region Obstacle
    public Material GetObstacle_Material(DataBundle.BallColor _targetColor) 
        => _targetColor== DataBundle.BallColor.BLUE ? obstacle_BlueMaterial : obstacle_OrangeMaterial;
    public GameObject GetObstacleObject(DataBundle.ObstacleType _type)
    {
        int index = prefab_List.FindIndex(o => o.prefabtype == _type);

        if (index < 0)
            return null;

        return prefab_List[index].prefab;
    }

    public void StateSet(StageData _info)
    {
        ResetAllObject();

        SetWallLength(_info.stageLength);

        foreach (ObstacleData data in _info.obstacleData)
        {
            CreateObstacle(data);
        }
    }


    public void CreateObstacle(ObstacleData _data)
    {
        ObstaclePrefabData prefabData = prefab_List.Find((o) => o.prefabtype == _data.prefabtype);

        if(prefabData == null)
        {
            Debug.LogError("No matching prefab found for ObstacleType: " + _data.prefabtype);
            return;
        }

        GameObject newObstacle = Instantiate(prefabData.prefab);

        activeObstacles.Add(newObstacle);

        if (newObstacle.TryGetComponent(out IObstacle obstacleComponent))
        {
            obstacleComponent.ApplyData(_data);
        }
    }

    public void ResetAllObject()
    {
        for (int i = activeObstacles.Count - 1; i >= 0; i--)
        {
            GameObject obstacle = activeObstacles[i];

            if (obstacle == null)
                continue;

            Destroy(obstacle);           
        }

        activeObstacles.Clear();
    }

    public void SetWallLength(float _length)
    {
        if(wall_List.Count != 2)
        {
            for (int i = wall_List.Count - 1; i >= 0; i--)
            {
                if (wall_List[i] != null)
                {
                    Destroy(wall_List[i]);
                }
            }

            wall_List.Clear();

            GameObject leftwall = Instantiate(GetObstacleObject(DataBundle.ObstacleType.WALL));
            GameObject rightwall = Instantiate(GetObstacleObject(DataBundle.ObstacleType.WALL));

            leftwall.transform.position = new Vector3(5f, 0, 0);
            rightwall.transform.position = new Vector3(-5f, 0, 0);

            wall_List.Add(leftwall);
            wall_List.Add(rightwall);
        }

        foreach (GameObject wall in wall_List)
        {
            Vector3 scale = wall.transform.localScale;
            scale.y = _length;
            wall.transform.localScale = scale;
            wall.transform.position = new Vector3(wall.transform.position.x, _length / 2f, wall.transform.position.z);
        }
    }

    public void SetWallLength_CreateMode(float _length)
    {
        for (int i = wall_List.Count - 1; i >= 0; i--)
        {
            if (wall_List[i] != null)
            {
                DestroyImmediate(wall_List[i]);
            }
        }

        wall_List.Clear();

        GameObject leftwall = Instantiate(GetObstacleObject(DataBundle.ObstacleType.WALL));
        GameObject rightwall = Instantiate(GetObstacleObject(DataBundle.ObstacleType.WALL));

        leftwall.transform.position = new Vector3(5f, 0, 0);
        rightwall.transform.position = new Vector3(-5f, 0, 0);

        wall_List.Add(leftwall);
        wall_List.Add(rightwall);


        foreach (GameObject wall in wall_List)
        {
            Vector3 scale = wall.transform.localScale;
            scale.y = _length;
            wall.transform.localScale = scale;
            wall.transform.position = new Vector3(wall.transform.position.x, _length / 2f, wall.transform.position.z);
        }
    }
    #endregion
}
