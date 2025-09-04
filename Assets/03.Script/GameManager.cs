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
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private InputActionReference inputAction;

    private static BallColor ballColor = BallColor.BLUE;
    public static BallColor CurrentBallColor => ballColor;

    public HashSet<Ball> aliveBall = new HashSet<Ball>();

    public static Action ClickEvent;
    public static Action<Transform> cameraTargetUpdate;

    private static readonly Color Blue = new Color(0f, 0f, 1f, 1f);
    private static readonly Color Orange = new Color(1f, 0.48f, 0f, 1f);

    private InputAction boxControl      = new InputAction();
    private InputAction changeBallColor = new InputAction();

    #region GameState

    private Vector2 prevTouchPosition;
    public GameObject obj;
    private void Awake()
    {
        MoveBoxInputSetup();
        inputAction.action.Enable();
    }


    public void MoveBoxInputSetup()
    {
        //inputAction.action.Reset();
        inputAction.action.started      += MoveBox_Started;
        inputAction.action.performed    += MoveBox_Performed;
        inputAction.action.canceled     += MoveBox_Cancled;
    }

    private void MoveBox_Started(InputAction.CallbackContext _context)
    {
        Debug.Log("aa");
        prevTouchPosition = _context.ReadValue<Vector2>();

        
    }

    private void MoveBox_Performed(InputAction.CallbackContext _context)
    {
        Debug.Log("aa");
        Vector2 currentPosition = _context.ReadValue<Vector2>();

        Vector2 tempDirection = currentPosition - prevTouchPosition;
        float direction = Vector2.Dot(Vector2.right, tempDirection) < 0 ? -1 : 1;
        

        obj.transform.position += direction * Vector3.right * 0.01f;


    }

    private void MoveBox_Cancled(InputAction.CallbackContext _context)
    {

        
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void EndGame()
    {

    }

    #endregion

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

    //private void ChangeBallColor()
    //{
    //    ballColor = ballColor == BallColor.ORANGE ? BallColor.BLUE : BallColor.ORANGE;

    //    ball_Material.color = ballColor == BallColor.ORANGE ? Orange : Blue;
    //}

    #endregion
}
