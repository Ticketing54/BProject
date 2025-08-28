using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplicateController : MonoBehaviour
{
    [SerializeField] private List<ReplicateBox> replicateBox_List;

    private HashSet<GameObject> collisionData = new HashSet<GameObject>();
    private static WaitForSeconds delayReplicateTime = new WaitForSeconds(0.1f);

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        foreach(ReplicateBox box in replicateBox_List)
        {
            box.ReplicateBall = ReplicateBall;
        }

    }

    private void ReplicateBall(GameObject _ball, int _count)
    {
        if (collisionData.Contains(_ball))
            return;

        StartCoroutine(CoReplicateBall(_ball,_count));
    }

    private IEnumerator CoReplicateBall(GameObject _ball, int _count)
    {
        collisionData.Add(_ball);

        _ball.TryGetComponent<Rigidbody>(out Rigidbody rigidbody);

        Vector3 originLinear = rigidbody?.linearVelocity ?? Vector3.zero;
        Vector3 originAngular = rigidbody?.angularVelocity ?? Vector3.zero;


        for (int i = 0; i < _count; i++)
        {
            GameObject ball = Instantiate<GameObject>(_ball);
            collisionData.Add(ball);

            ball.transform.position = _ball.transform.position;

            if (ball.TryGetComponent<Rigidbody>(out Rigidbody rig))
            {
                rig.angularVelocity = originAngular;
                rig.linearVelocity = originLinear;
            }

            yield return delayReplicateTime;
        }
    }


#if UNITY_EDITOR
    [Header("[Editor]")]
    [SerializeField] private ReplicateBox leftBox;
    [SerializeField] private ReplicateBox rightBox;
    [Range(0,1)][SerializeField] private float ratio;
    [SerializeField] private bool isMove = false;


    private Transform BoddyTransform => this.transform;

    private void OnValidate()
    {
        UpdatePositionAndScale();
    }

    private void UpdatePositionAndScale()
    {
        if (BoddyTransform == null)
            return;

        if(leftBox!= null)
        {
            Vector3 leftBoxPosition = BoddyTransform.position;
            leftBoxPosition.x = BoddyTransform.position.x - BoddyTransform.lossyScale.x * 0.5f +
                BoddyTransform.lossyScale.x * ratio * 0.5f;

            Vector3 leftBoxLocalScale = 
                new Vector3(ratio, BoddyTransform.lossyScale.y, 1);

            leftBox.transform.position = leftBoxPosition;
            leftBox.transform.localScale = leftBoxLocalScale;
            
        }

        
        if (rightBox != null)
        {
            Vector3 rightBoxPosition = BoddyTransform.position;
            rightBoxPosition.x = BoddyTransform.position.x + BoddyTransform.lossyScale.x * 0.5f -
                BoddyTransform.transform.lossyScale.x * (1 - ratio) * 0.5f;

            Vector3 rightBoxLocalScale = 
                new Vector3(1 - ratio, BoddyTransform.lossyScale.y, 1);

            rightBox.transform.position = rightBoxPosition;
            rightBox.transform.localScale = rightBoxLocalScale;
        }

        if(isMove)
        {

        }

    }

#endif

}
