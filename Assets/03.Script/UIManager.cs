using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image image_Fade;
    [SerializeField] private float fadeDuration = 1f;

    [SerializeField] private TextMeshProUGUI score_TextMeshProUGUI;

    [Header("EndUI")]
    [SerializeField] private CanvasGroup end_CanvasGroup;
    [SerializeField] private Button retry_Button;
    [SerializeField] private Button next_Button;


    public Action OnClick_NextStage;
    public Action Onclick_Retry;

    private float timer = 0f;
    private Coroutine fadeCoroutine;

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged += UpdateScoreUI;
            GameManager.Instance.OnOpenResultPage += OpenResultPage;
            GameManager.Instance.OnFadeInRequest += StartFadeIn;
            GameManager.Instance.OnFadeOutRequest += StartFadeOut;
            GameManager.Instance.ResetObject += ScoreBoadReset;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged -= UpdateScoreUI;
            GameManager.Instance.OnOpenResultPage -= OpenResultPage;
            GameManager.Instance.OnFadeInRequest -= StartFadeIn;
            GameManager.Instance.OnFadeOutRequest -= StartFadeOut;
            GameManager.Instance.ResetObject -= ScoreBoadReset;
        }
    }

    #region Score

    private void ScoreBoadReset()
    {
        score_TextMeshProUGUI.text = string.Empty;
    }

    public void UpdateScoreUI(int _score, int _goalScore)
    {
        string goalMessage = _score + " / " + _goalScore;

        score_TextMeshProUGUI.text = goalMessage;
    }


    public void OnClick_RetryButton()
    {
        end_CanvasGroup.gameObject.SetActive(false);
        GameManager.Instance.NextStage(false);
    }

    public void OnClick_NextStageButton()
    {
        end_CanvasGroup.gameObject.SetActive(false);
        GameManager.Instance.NextStage(true);
    }

    public void OpenResultPage(bool _isNext)
    {
        StartCoroutine(CoOpoenScoreUI(_isNext));
    }

    private IEnumerator CoOpoenScoreUI(bool _isNext)
    {
        next_Button.interactable = _isNext;
        end_CanvasGroup.alpha = 0;
        end_CanvasGroup.gameObject.SetActive(true);

        while (end_CanvasGroup.alpha < 1)
        {
            end_CanvasGroup.alpha += Time.deltaTime;
            yield return null;
        }
    }

    #endregion

    public void StartFadeIn(Action _callBack = null)
    {
        if (image_Fade == null)
        {
            Debug.LogError("Fade Image is not assigned in UIManager.");
            return;
        }

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(CoFadeIn(_callBack));
    }

    public void StartFadeOut(Action _callBack = null)
    {
        if (image_Fade == null)
        {
            Debug.LogError("Fade Image is not assigned in UIManager.");
            return;
        }

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(CoFadeOut(_callBack));
    }

    private IEnumerator CoFadeIn(Action _callBack)
    {
        timer = 0;
        Color originColor = image_Fade.color;
        originColor.a = 0f;

        while (true)
        {
            yield return null;

            timer += Time.deltaTime;
            originColor.a = Mathf.Clamp01(timer / fadeDuration);

            image_Fade.color = originColor;

            if (originColor.a >= 1f)
            {
                _callBack?.Invoke();
                yield break;
            }

        }
    }

    private IEnumerator CoFadeOut(Action _callBack)
    {
        timer = 0;
        Color originColor = image_Fade.color;
        originColor.a = 1f;

        while (true)
        {
            yield return null;

            timer += Time.deltaTime;
            originColor.a = 1 - (timer / fadeDuration);

            image_Fade.color = originColor;

            if (originColor.a <= 0f)
            {
                _callBack?.Invoke();
                yield break;
            }

        }
    }

}
