using System.Collections;
using UnityEngine;


public class Ball : MonoBehaviour
{   
    [SerializeField] private Rigidbody rig;

    public void ClearForces()
    {
        if(rig == null)
        {
            Debug.LogError("Ball: Rigidbody reference is missing.");
            return;
        }

        rig.linearVelocity = Vector3.zero;
        rig.angularVelocity = Vector3.zero;
    }

    public void Move(Vector3 _position)
    {
        rig.MovePosition(_position);
    }

    public void ConstraintsPositionZ(bool _isLock)
    {
        rig.constraints = _isLock
       ? rig.constraints | RigidbodyConstraints.FreezePositionZ
       : rig.constraints & ~RigidbodyConstraints.FreezePositionZ;
    }
}
