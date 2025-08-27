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

}
