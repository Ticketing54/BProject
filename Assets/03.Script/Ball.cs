using System.Collections;
using UnityEngine;


public class Ball : MonoBehaviour
{   
    [SerializeField] private Rigidbody rig;

    public void Sleep(Transform _transform)
    {
        transform.SetParent(_transform, true);
        rig.isKinematic = true;
        rig.Sleep();
    }

    public void WakeUp()
    {
        transform.SetParent(null,true);
        rig.isKinematic = false;
        rig.linearVelocity = Vector3.zero;
        rig.angularVelocity = Vector3.zero;
        rig.WakeUp();
        rig.AddForce(Vector3.up * 0.1f, ForceMode.Impulse);
    }

    public void Move(Vector3 _position)
    {
        if (rig == null)
        {
            Debug.LogError("Ball: Rigidbody reference is missing.");
            return;
        }

        rig.position = _position;
        rig.angularVelocity = Vector3.zero;
        rig.linearVelocity = Vector3.zero;
        transform.position = _position;
    }

    public void ConstraintsPositionZ(bool _isLock)
    {
        rig.constraints = _isLock
       ? rig.constraints | RigidbodyConstraints.FreezePositionZ
       : rig.constraints & ~RigidbodyConstraints.FreezePositionZ;
    }
}
