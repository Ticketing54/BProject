using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Coroutine cameraRoutine;

    private const float animationDuration = 5.0f;

    private float startPostionY = 0;
    private const float endPostionY = -15;
    public float moveSpeed = 5;

    public void FixCameraPosition(float _positionY)
    {
        if (cameraRoutine != null)
            StopCoroutine(cameraRoutine);

        Vector3 targetPosition = transform.position;
        targetPosition.y= _positionY;
        transform.position = targetPosition;
    }

    public void FixCameraPositionSmooth(float _positionY)
    {
        if (cameraRoutine != null)
            StopCoroutine(cameraRoutine);

        Vector3 destination = new Vector3(transform.position.x, _positionY, transform.position.z);

        cameraRoutine = StartCoroutine(CoSmoothMove(destination));
    }

    private IEnumerator CoSmoothMove(Vector3 _destination)
    {
        while (Mathf.Abs(transform.position.y - _destination.y) > 0.01f)
        {
            float newY = Mathf.Lerp(transform.position.y, _destination.y, Time.deltaTime * moveSpeed);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            yield return null;
        }

        transform.position = _destination;
    }


    public void LookAtLowestBall()
    {
        if(cameraRoutine != null)
            StopCoroutine(cameraRoutine);

        cameraRoutine = StartCoroutine(CoTrackingLowestBall());
    }

    private IEnumerator CoTrackingLowestBall()
    {
        Ball target;

        while (true)
        {
            target = FindLowestBall();

            if (target != null && target.gameObject.activeSelf)
            {
                float newY = Mathf.Lerp(transform.position.y, target.transform.position.y, Time.deltaTime * moveSpeed);
                transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            }

            yield return null;
        }
    }

    private Ball FindLowestBall()
    {
        List<Ball> ballList = GameManager.Instance.GetActiveBallList;

        if (ballList == null)
            return null;

        Ball lowestBall = ballList.Count == 0 ? null : ballList[0];

        foreach (Ball ball in ballList)
        {
            if (ball == null || !ball.gameObject.activeSelf)
                continue;

            if (lowestBall.transform.position.y > ball.transform.position.y)
                lowestBall = ball;
        }

        return lowestBall;
    }


    #region OPENING

    public void SetOpeningData(float _length)
    {
        if(cameraRoutine != null)
            StopCoroutine(cameraRoutine);

        startPostionY = _length - 5;
        transform.position = new Vector3(transform.position.x, endPostionY, transform.position.z);
    }

    public void StartOpening(Action _callback)
    {
        if(cameraRoutine != null)
            StopCoroutine(cameraRoutine);
        
        cameraRoutine = StartCoroutine(AnimateCamera(_callback));
    }
    IEnumerator AnimateCamera(Action _callback)
    {
        float duration = animationDuration;
        float elapsed = 0;

        Vector3 animationStartPosition = new Vector3(transform.position.x, endPostionY, transform.position.z);
        Vector3 animationEndPosition = new Vector3(transform.position.x, startPostionY, transform.position.z);

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(animationStartPosition, animationEndPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
            continue;
        }

        transform.position = animationEndPosition;

        LookAtLowestBall();

        _callback?.Invoke();
    }

    #endregion




#if UNITY_EDITOR
    private Color gizmoColor = Color.red;
    private float gizmoRadius = 0.5f;
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, gizmoRadius);

    }

#endif
}
