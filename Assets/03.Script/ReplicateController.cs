using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplicateController : MonoBehaviour
{
    [System.Serializable]
    private class BoxData
    {
        public BallColor color;
        public int count;
        public ReplicateBox targetBox;
        public bool isEnable = true;
    }

    [Header("LeftBox")]
    [SerializeField] private BoxData leftBoxData;

    [Header("RightBox")]
    [SerializeField] private BoxData rightBoxData;

    [Header("Ect")]
    [SerializeField] private Material blueMaterial;
    [SerializeField] private Material orangeMaterial;

    private HashSet<GameObject> collisionData = new HashSet<GameObject>();
    private static WaitForSeconds delayReplicateTime = new WaitForSeconds(0.1f);

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        if (leftBoxData.targetBox != null)
            leftBoxData.targetBox.ReplicateBall = ReplicateBall;

        if (rightBoxData.targetBox != null)
            rightBoxData.targetBox.ReplicateBall = ReplicateBall;
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
        

        GameObject.Destroy(_ball);
    }


#if UNITY_EDITOR
    [Range(0,1)][SerializeField] private float ratio;
    [SerializeField] private bool isMove = false;


    private Transform BoddyTransform => this.transform;

    private void OnValidate()
    {
        UpdatePositionAndScale();
    }

    private Material GetReplicateMaterial(BallColor _color)
    {
        return _color == BallColor.ORANGE ? orangeMaterial : blueMaterial;
    }

    private void UpdatePositionAndScale()
    {
        if (BoddyTransform == null)
            return;

        if (leftBoxData.targetBox != null)
        {
            ReplicateBox replicateLeftBox = leftBoxData.targetBox;

            Vector3 leftBoxPosition = BoddyTransform.position;
            leftBoxPosition.x = BoddyTransform.position.x - BoddyTransform.lossyScale.x * 0.5f +
                BoddyTransform.lossyScale.x * ratio * 0.5f;

            Vector3 leftBoxLocalScale =
                new Vector3(ratio, BoddyTransform.lossyScale.y, 1);

            replicateLeftBox.transform.position = leftBoxPosition;
            replicateLeftBox.transform.localScale = leftBoxLocalScale;

            replicateLeftBox.targetState = leftBoxData.color;
            replicateLeftBox.count = leftBoxData.count;

            replicateLeftBox.IsEnable(leftBoxData.isEnable);
            replicateLeftBox.UpdateData(replicateLeftBox.transform.position, GetReplicateMaterial(replicateLeftBox.targetState));

        }

        if (rightBoxData.targetBox != null)
        {
            ReplicateBox replicateRightBox = rightBoxData.targetBox;

            Vector3 rightBoxPosition = BoddyTransform.position;
            rightBoxPosition.x = BoddyTransform.position.x + BoddyTransform.lossyScale.x * 0.5f -
                BoddyTransform.transform.lossyScale.x * (1 - ratio) * 0.5f;

            Vector3 rightBoxLocalScale =
                new Vector3(1 - ratio, BoddyTransform.lossyScale.y, 1);

            replicateRightBox.transform.position = rightBoxPosition;
            replicateRightBox.transform.localScale = rightBoxLocalScale;

            replicateRightBox.targetState = rightBoxData.color;
            replicateRightBox.count = rightBoxData.count;

            replicateRightBox.IsEnable(rightBoxData.isEnable);
            replicateRightBox.UpdateData(replicateRightBox.transform.position,GetReplicateMaterial(replicateRightBox.targetState));
        }

        if(isMove)
        {

        }

    }

#endif

}
