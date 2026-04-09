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
    public float aa = 5;
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
            if (target != null && target.gameObject.activeSelf)
            {   
                offset.y = target.transform.position.y;
                offset.y = Mathf.Lerp(transform.position.y, offset.y, Time.deltaTime * aa);
                transform.position = offset;
            }

            yield return null;
        }
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

        LookAtTarget();

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
