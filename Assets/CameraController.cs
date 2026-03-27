using System;
using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform target
    {
        get
        {
            if (GameManager.Instance == null)
                return null;

            return GameManager.Instance.GetCameraTarget();
        }
    }

    private Vector3 offset;
    private Coroutine cameraRoutine;

    private const float animationDuration = 5.0f;

    private float startPostionY = 0;
    private const float endPostionY = -15;


    private void Start()
    {
        offset = transform.position;
    }

    public void LookAtTarget()
    {
        if(cameraRoutine != null)
            StopCoroutine(cameraRoutine);

        cameraRoutine = StartCoroutine(CoTrackingTarget());
    }

    private IEnumerator CoTrackingTarget()
    {
        while (true)
        {
            if (target != null)
            {
                offset.y = target.transform.position.y;
                transform.position = offset;
            }

            yield return null;
        }
    }
    #region OPENING

    public void SetOpeningData(float _length)
    {
        startPostionY = _length - 5;
        transform.position = new Vector3(transform.position.x, endPostionY, transform.position.z);
    }

    public void StartOpening(Action _callback)
    {
        if(cameraRoutine != null)
            StopCoroutine(cameraRoutine);
        
        cameraRoutine = StartCoroutine(AnimateCamera(_callback));
    }

    #endregion

    IEnumerator AnimateCamera(Action _callback)
    {
        Debug.Log("AnimateCamera started");
        float duration = animationDuration; 
        float elapsed = 0;

        Vector3 animationStartPosition = new Vector3(transform.position.x, endPostionY, transform.position.z);
        Vector3 animationEndPosition = new Vector3(transform.position.x, startPostionY, transform.position.z);

        transform.position = animationStartPosition;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(animationStartPosition, animationEndPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = animationEndPosition;

        LookAtTarget();
    }


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
