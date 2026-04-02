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
        GameManager.Instance.OnScoreChanged += UpdateScoreUI;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnScoreChanged -= UpdateScoreUI;
    }

    #region Score
    public void UpdateScoreUI(int _score)
    {
        score_TextMeshProUGUI.text = _score.ToString();
    }


    public void OnClick_RetryButton()
    {
        end_CanvasGroup.gameObject.SetActive(false);
        Onclick_Retry?.Invoke();
    }

    public void OnClick_NextStageButton()
    {
        end_CanvasGroup.gameObject.SetActive(false);
        OnClick_NextStage?.Invoke();
    }

    public void OpenEndUI(bool _isNext)
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
