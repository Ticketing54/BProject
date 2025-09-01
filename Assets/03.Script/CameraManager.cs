using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cinemachineCamera;

    private void Awake()
    {
        
    }

    private void TargetUpdate(Transform _target)
    {
        cinemachineCamera.Target.TrackingTarget = _target;
    }           
}
