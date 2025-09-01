using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

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
            if (instance == null)
            {
                instance = FindAnyObjectByType<GameManager>();

                if (instance == null)
                {
                    GameObject gameManager = new GameObject("GameManager");
                    instance = gameManager.AddComponent<GameManager>();
                }

                    
            }

            return instance;
        }
    }

    [SerializeField] private Material ballMaterial;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private InputActionReference inputAction;

    private static BallColor ballColor = BallColor.RED;
    public static BallColor CurrentBallColor => ballColor;

    public HashSet<Ball> aliveBall = new HashSet<Ball>();

    public static Action ClickEvent;
    public static Action<Transform> cameraTargetUpdate;

    private void OnEnable()
    {
        ClickEvent += ChangeBallColor;
    }

    private void OnDisable()
    {
        ClickEvent -= ChangeBallColor;
    }

    #region Ball

    public void AddAliveBall(Ball _ball)
    {
        if (!aliveBall.Contains(_ball))
            aliveBall.Add(_ball);
    }

    public void RemoveAlliveBall(Ball _ball)
    {
        if (aliveBall.Contains(_ball))
            aliveBall.Remove(_ball);
    }

    public Transform FindLowestBall()
    {
        List<Ball> ballList = new List<Ball>(aliveBall);

        Ball lowestBall = ballList.Count == 0 ? null : ballList[0];

        foreach(Ball ball in ballList)
        {
            if (ball == null)
                continue;

            if (lowestBall.transform.position.y > ball.transform.position.y)
                lowestBall = ball;
        }

        return lowestBall.transform;
    }

    private void ChangeBallColor()
    {
        ballColor = ballColor == BallColor.RED ? BallColor.BLUE : BallColor.RED;
    }

    #endregion
}
