using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cinemachineCamera;

    private void Awake()
    {
        
    }

    private void OnDestroy()
    {
        
    }

    private void TargetUpdate(Transform _target)
    {
        cinemachineCamera.Target.TrackingTarget = _target;
    }           
}
