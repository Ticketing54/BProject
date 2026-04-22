using UnityEngine;


public class Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody rig;
    [SerializeField] private TrailRenderer trailRenderer;
    
    public void FixedObject(Rigidbody _parent)
    {
        FixedJoint fixedJoint = gameObject.AddComponent<FixedJoint>();
        fixedJoint.connectedBody = _parent;
        fixedJoint.enableCollision = false;
        rig.useGravity = false;
    }

    public void RealsedObject()
    {
        if(TryGetComponent<FixedJoint>(out FixedJoint joint))
        {
            Destroy(joint);
        }

        rig.useGravity = true;
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

        trailRenderer.Clear();  // ¿‹ªÛ «ÿ∞·
    }

    public void ConstraintsPositionZ(bool _isLock)
    {
        rig.constraints = _isLock
       ? rig.constraints | RigidbodyConstraints.FreezePositionZ
       : rig.constraints & ~RigidbodyConstraints.FreezePositionZ;
    }
}
