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
        Ready,              // 스테이지 전환 준비
        Loading,            // 리소스 로드 중
        Active,             // 로드 완료 및 대기 상태 
        Cutscene,           // 카메라 이벤트 등 연출 
        Playing,            // 실제 게임 플레이
        Result,             // 스테이지 종료 및 결과 
    }

    // Ball Contatiner
    [SerializeField] private ObjectContainer objectContainer;

    // Camera
    [SerializeField] private CameraController cameraController;

    //StageContainer
    [SerializeField] private StageContainer stageContainer;

    //Test
    [SerializeField] private bool isTestMode = false;

    // Input

    public Action InputClickDown;
    public Action InputClickUp;

    public Vector2 Direction = Vector2.zero;

    private eGameState currentGameState = eGameState.None;
    private int stageScore = 0;
    private bool isRestart = false;

    public event Action<int, int> OnScoreChanged;
    public event Action<Action> OnFadeInRequest;
    public event Action<Action> OnFadeOutRequest;
    public event Action<bool> OnOpenResultPage;
    public event Action ResetObject;
    public event Action<bool> OnBlockInput;
    public event Action EditorExit;

    public event Action<List<Ball>, float> OnStartBoxSet;

    public StageData CurrentStageData => stageContainer.CurrentStageData ?? null;

    private void Awake()
    {
        if (objectContainer == null)
            Debug.LogError("Ball Container is not assigned in GameManager.");

        InputClickDown += ChangeBallColor;
    }


    private void Start()
    {
        if (isTestMode)
            return;

        ChangeGameState(eGameState.Ready);
    }
    public void SetTestStage(StageData _stageData) => objectContainer.StateSet(_stageData);
    public bool IsAllBallsEntered(int _count) => _count >= objectContainer.ActiveBallCount;

    public void SetScore(int _score)
    {
        stageScore = _score;
        OnScoreChanged?.Invoke(stageScore, CurrentStageData.goalScore);

        if (IsAllBallsEntered(_score) && !isTestMode)
            ChangeGameState(eGameState.Result);
    }

    public string GetScore()
    {
        return stageScore.ToString();
    }

    #region Camera

    public void FixCameraStartPosition() => cameraController.FixCameraPosition(CurrentStageData.stageLength - 5);
    public void FixCameraEndPosition() => cameraController.FixCameraPositionSmooth(DataBundle.ENDBOX_POSITION);



    public void LookatLowestBall() => cameraController.LookAtLowestBall();
    public List<Ball> GetActiveBallList => objectContainer?.ActiveBallList() ?? null;

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

    public void SetStartBox()
    {
        int count = Mathf.Clamp(CurrentStageData.startBallCount, 1, 10);

        List<Ball> startBallList = new List<Ball>();

        for (int i = 0; i < count; i++)
        {
            startBallList.Add(objectContainer.GetBall(Vector3.zero));
        }

        OnStartBoxSet?.Invoke(startBallList,CurrentStageData.stageLength);
    }

    public void NextStage(bool _isRestart)
    {
        if (!_isRestart)
            PlayerPrefs.SetInt(CurrentStageData.name, 1);

        isRestart = _isRestart;

        ChangeGameState(eGameState.Ready);
    }


    public void ChangeGameState(eGameState _nextState)
    {
        currentGameState = _nextState;

        switch (currentGameState)
        {
            case eGameState.None:
                break;
            case eGameState.Ready:
                OnBlockInput?.Invoke(true);
                stageScore = 0;
                OnFadeInRequest?.Invoke(() => ChangeGameState(eGameState.Loading));
                break;
            case eGameState.Loading:                // 리소스 로드 및 스테이지 배치

                stageContainer.SetStage(isRestart);  //  스테이지 정보 저장

                isRestart = false;

                ResetObject?.Invoke();

                SetStartBox();

                cameraController.SetOpeningData(CurrentStageData.stageLength);

                objectContainer.StateSet(CurrentStageData);

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
                FixCameraStartPosition();
                // 입력 활성화
                break;
            case eGameState.Result:
#if UNITY_EDITOR
                if (isTestMode)
                {
                    EditorExit?.Invoke();
                    return;
                }
#endif
                Result();
                break;
        }
    }

    private void Result()
    {
        StartCoroutine(CoResult());
    }

    private IEnumerator CoResult()
    {
        yield return new WaitForSeconds(3f);

        OnOpenResultPage?.Invoke(CurrentStageData.goalScore <= stageScore);
    }
}
