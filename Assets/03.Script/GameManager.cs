using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public enum BallColor
{
    ORANGE,
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

    [SerializeField] private Material ball_Material;
    [SerializeField] private Material replicate_Original;
    [SerializeField] private Material replicate_Inverted;

    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private InputActionReference inputAction;

    private static BallColor ballColor = BallColor.BLUE;
    public static BallColor CurrentBallColor => ballColor;

    public HashSet<Ball> aliveBall = new HashSet<Ball>();

    public static Action ClickEvent;
    public static Action<Transform> cameraTargetUpdate;


    private static readonly Color Blue = new Color(0f, 0f, 1f, 1f);
    private static readonly Color Orange = new Color(1f, 0.48f, 0f, 1f);
    

    private void OnEnable()
    {
        ClickEvent += ChangeBallColor;

        inputAction.action.performed += (_) => { ClickEvent?.Invoke(); };
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
        ballColor = ballColor == BallColor.ORANGE ? BallColor.BLUE : BallColor.ORANGE;

        ball_Material.color = ballColor == BallColor.ORANGE ? Orange : Blue;
    }

    #endregion
}
