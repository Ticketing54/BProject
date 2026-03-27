using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{   
    [SerializeField] private Image image_Fade;
    [SerializeField] private float fadeDuration = 1f;

    private float timer = 0f;
    private Coroutine fadeCoroutine;


    public void StartFadeIn(Action _callBack=null)
    {
        if (image_Fade == null)
        {
            Debug.LogError("Fade Image is not assigned in UIManager.");
            return;
        }

        if(fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(CoFadeIn(_callBack));
    }

    public void StartFadeOut(Action _callBack=null)
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
            originColor.a = timer / fadeDuration;

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
