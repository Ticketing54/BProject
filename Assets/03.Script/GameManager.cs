using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
                    //GameObject gameManager = new GameObject("GameManager");
                    //instance = gameManager.AddComponent<GameManager>();

                    if (Application.isPlaying)
                    {
                        Debug.LogError("GameManager instance not found in the scene");

                    }

                }
            }

            return instance;
        }
    }

    public enum eGameState
    {
        None,
        Intro,              // 최초 시작 연출 
        Ready,              // 스테이지 전환 준비
        Loading,            // 리소스 로드 중
        Active,             // 로드 완료 및 대기 상태 
        Cutscene,           // 카메라 이벤트 등 연출 
        Playing,            // 실제 게임 플레이
        Result,             // 스테이지 종료 및 결과 
        Test
    }

    // Ball Contatiner
    [SerializeField] private ObjectContainer objectContainer;

    // Camera
    [SerializeField] private CameraController cameraController;

    // UImanager
    [SerializeField] private UIManager uiManager;
    [SerializeField] private StageContainer stageContainer;

    // StartBox
    [SerializeField] private StartBox startBox;
    [SerializeField] private EndBox endBox;
    [SerializeField] private bool isTestMode = false;

    // Input

    public Action InputClickDown;
    public Action InputClickUp;

    public Vector2 Direction = Vector2.zero;

    private eGameState currentGameState = eGameState.None;
    private int playLevel = 1;
    private int stageScore = 0;
    private StageData currentStageData;

    public event Action<int,int> OnScoreChanged;
    public event Action<Action> OnFadeInRequest;
    public event Action<Action> OnFadeOutRequest;
    public event Action<bool> OnOpenResultPage;
    public event Action ResetObject;
    public event Action<bool> OnBlockInput;

    private void Awake()
    {
        if (objectContainer == null)
            Debug.LogError("Ball Container is not assigned in GameManager.");

        InputClickDown += ChangeBallColor;
    }


    private void Start()
    {
        if (isTestMode)
        {
            ChangeGameState(eGameState.Test);
            return;
        }

        ChangeGameState(eGameState.Intro);
    }

    public bool IsAllBallsEntered(int _count) => _count >= objectContainer.ActiveBallCount;

    public void SetScore(int _score)
    {
        stageScore = _score;
        OnScoreChanged?.Invoke(stageScore, currentStageData.goalScore);

        if (IsAllBallsEntered(_score) && !isTestMode)
            ChangeGameState(eGameState.Result);
    }

    public string GetScore()
    {
        return stageScore.ToString();
    }
    private void SetTargetToEndBox(int _score,int _goalScore)
    {
        if (_score == 1)
        {
            camearaTarget = endBox.transform;
        }
    }
    #region Camera

    private Transform camearaTarget;

    public void SetCameraTarget(Transform _Target = null)
    {
        camearaTarget = _Target;
        cameraController.LookAtTarget();
    }

    public Transform GetCameraTarget()
    {
        if (camearaTarget == null)
        {
            return FindLowestBall()?.transform;
        }

        return camearaTarget;
    }
    private Ball FindLowestBall()
    {
        List<Ball> ballList = objectContainer.ActiveBallList();

        if (ballList == null)
            return null;
       
        Ball lowestBall = ballList.Count == 0 ? null : ballList[0];

        foreach (Ball ball in ballList)
        {
            if (ball == null)
                continue;

            if (lowestBall.transform.position.y > ball.transform.position.y)
                lowestBall = ball;
        }

        return lowestBall;
    }


    #endregion

    #region Ball

    public void ChangeBallColor() => objectContainer?.ChangeBallColor();
    public DataBundle.BallColor CurrentBallColor => objectContainer.CurrentBallColor;
    public void ReturnBall(Ball _ball)
    {
        objectContainer?.ReleaseBall(_ball);

        if (objectContainer.ActiveBallCount <= 0 && currentGameState != eGameState.Loading)
            ChangeGameState(eGameState.Result);

    }
    public Ball CreateBall(Vector3 _position) => objectContainer?.GetBall(_position);

    #endregion

    public Material GetObstacleMaterial(DataBundle.BallColor _targetColor)
    {
        if (objectContainer == null)
            return null;

        return objectContainer.GetObstacle_Material(_targetColor);
    }

    public void CreateStartBall(int _count)
    {
        if (startBox == null)
        {
            Debug.LogError("StartBox reference is missing in ObjectContainer.");
            return;
        }

        int count = Mathf.Clamp(_count, 1, 10);

        List<Transform> startposition = startBox.GetStartBallPosition();

        for (int i = 0; i < count; i++)
        {
            objectContainer.GetBall(startposition[i % startposition.Count].position);
        }
    }

    public void NextStage(bool _isRestart)
    {
        //playLevel = _isRestart ? playLevel : playLevel + 1;
        ChangeGameState(eGameState.Ready);
    }


    public void ChangeGameState(eGameState _nextState)
    {
        currentGameState = _nextState;

        switch (currentGameState)
        {
            case eGameState.None:
                break;
            case eGameState.Test:
                TestMode();
                break;
            case eGameState.Intro:
                Intro();
                break;
            case eGameState.Ready:          // 스테이지 전환 준비 (UI FadeIn)
                OnFadeInRequest?.Invoke(() => ChangeGameState(eGameState.Loading));
                break;
            case eGameState.Loading:        // 리소스 로드 및 스테이지 배치

                ResetObject?.Invoke();

                currentStageData = stageContainer.GetStageData(playLevel);

                startBox.SetStartPosition(currentStageData.stageLength);

                CreateStartBall(currentStageData.startBallCount);

                cameraController.SetOpeningData(currentStageData.stageLength);

                objectContainer.StateSet(currentStageData);

                ChangeGameState(eGameState.Active);

                break;
            case eGameState.Active:
                OnFadeOutRequest?.Invoke(() => ChangeGameState(eGameState.Cutscene));
                break;
            case eGameState.Cutscene:
                cameraController.StartOpening(() => { ChangeGameState(eGameState.Playing); });
                break;
            case eGameState.Playing:
                OnBlockInput?.Invoke(false);
                SetCameraTarget(startBox.transform);
                // 입력 활성화
                break;
            case eGameState.Result:
                OnBlockInput?.Invoke(true);
                Result();
                break;
        }
    }

    private void TestMode()
    {
        SetCameraTarget(startBox.transform);
    }

    private void Intro()
    {
        playLevel = PlayerPrefs.GetInt("PlayerLevel", 1);               //  플레이어 레벨 불러오기 (기본값 1)

        currentStageData = stageContainer.GetStageData(playLevel);   // 스테이지 데이터 불러오기

        startBox.SetStartPosition(currentStageData.stageLength);               // 스타트 박스 위치 설정

        CreateStartBall(currentStageData.startBallCount);                      // 시작 볼 생성

        objectContainer.StateSet(currentStageData);                            // 오브젝트 컨테이너에 스테이지 데이터 전달 및 스테이지 배치 

        SetCameraTarget(startBox.transform);                            // 카메라 타겟 설정

    }

    private void Result()
    {
        StartCoroutine(CoResult());
    }

    private IEnumerator CoResult()
    {
        yield return new WaitForSeconds(3f);
        
        OnOpenResultPage?.Invoke(currentStageData.goalScore <= stageScore);
    }


    

}
