using System;
using System.Collections.Generic;
using Unity.Cinemachine;
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
    [SerializeField] private GameObject ballPrefab;

    private BallColor ballColor = BallColor.RED;
    public BallColor CurrentBallColor => ballColor;


    public CinemachineTargetGroup group;

    public static Action ClickEvent;

    private void Awake()
    {
        //ballPool = new ObjectPool<GameObject>(Pool_Create, null, BallPool_Release, null, true, 10, 10);
        
    }

    private void OnEnable()
    {
        ClickEvent += ChangeBallColor;
    }

    private void OnDisable()
    {
        ClickEvent -= ChangeBallColor;
    }

    private void ChangeBallColor()
    {
        ballColor= ballColor == BallColor.RED ? BallColor.BLUE : BallColor.RED;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            CreateBall(new Vector3(0, 55, 0));
        }
    }

    #region BallPool

    private GameObject Pool_Create() => Instantiate<GameObject>(ballPrefab);

    private void BallPool_Release(GameObject _ball)
    {
        if(group.FindMember(_ball.transform) == -1)
            group.RemoveMember(_ball.transform);

        _ball.gameObject.SetActive(false);

        //Debug.Log("Release");
    }

    #endregion

    public void CreateBall(Vector3 _position, Action<GameObject> _applyCollisionBox = null)
    {
        //if (ballPool == null)
        //    return ;

        GameObject ball = Pool_Create(); //ballPool.Get();

        _applyCollisionBox?.Invoke(ball);

        ball.transform.position = _position;

        if (group.FindMember(ball.transform) == -1)
            group.AddMember(ball.transform,1,1);
    }

    public void ReleaseBall(GameObject _ball)
    {
        //if (ballPool == null)
        //    return;

        Destroy(_ball);

        //ballPool.Release(_ball);
    }

}
