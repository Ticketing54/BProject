using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class CollisionBox : MonoBehaviour
{
    [SerializeField] private int count = 0;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI_Count;
    [SerializeField] private BallColor currentState;

    private  WaitForSeconds delayReplicateTime = new WaitForSeconds(0.1f);

    private BallColor CurrentState => currentState;

    private HashSet<GameObject> collisionData = new HashSet<GameObject>();

    public void Setup()
    {
        textMeshProUGUI_Count.text = $"{count} X";
    }

    private void OnTriggerExit(Collider other)
    {
        if (collisionData.Contains(other.gameObject))
            return;


        StartCoroutine(CoReplicateBall(other.gameObject));

    }
    
    private IEnumerator CoReplicateBall(GameObject _ball)
    {
        collisionData.Add(_ball);

        _ball.TryGetComponent<Rigidbody>(out Rigidbody rigidbody);

        Vector3 originLinear = rigidbody?.linearVelocity ?? Vector3.zero;
        Vector3 originAngular = rigidbody?.angularVelocity ?? Vector3.zero;


        for (int i = 0; i < count; i++)
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


    private void AddCollisionBox(GameObject _obj)
    {
        if (collisionData.Contains(_obj))
            return;

        collisionData.Add(_obj);
    }
}
