using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

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
                    instance = new GameManager();
            }

            return instance;
        }
    }

    [SerializeField] private Material ballMaterial;
    [SerializeField] private GameObject ballPrefab;

    private static BallColor ballColor = BallColor.RED;
    public static BallColor CurrentBallColor => ballColor;

    public HashSet<Ball> aliveBall = new HashSet<Ball>();

    public CinemachineTargetGroup group;

    public static Action ClickEvent;
    public Ball LowestBall { get; private set; }


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

        FindLowestBall();
    }

    public void RemoveAlliveBall(Ball _ball)
    {
        if (aliveBall.Contains(_ball))
            aliveBall.Remove(_ball);

        FindLowestBall();
    }

    private void FindLowestBall()
    {
        List<Ball> ballList = new List<Ball>(aliveBall);

        foreach(Ball ball in ballList)
        {
            if (ball == null)
                continue;

            if (LowestBall.transform.position.y > ball.transform.position.y)
                LowestBall = ball;
        }
    }

    private void ChangeBallColor()
    {
        ballColor = ballColor == BallColor.RED ? BallColor.BLUE : BallColor.RED;
    }

    #endregion
}
