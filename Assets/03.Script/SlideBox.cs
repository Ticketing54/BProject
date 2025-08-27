using UnityEditor;
using UnityEngine;

public class SlideBox : MonoBehaviour
{
    [Header("BoxData")]
    [SerializeField] private ReplicateBox prefab;
    [SerializeField] private float moveSpeed = 0.1f;
    [Range(0, 1)][SerializeField] private float ratio;

    private ReplicateBox leftBox;
    private ReplicateBox rightBox;
    private ReplicateBox tempBox;
    private float leftOriginX;
    private float rightOriginX;

    private Transform SlideBoxTransform => this.transform;
    private Vector3 GetLocalXScale(float _x) => new Vector3(_x, 1, 1);

    private void Awake()
    {
        Setup();
    }

    private void Update()
    {
        UpdateScale();
    }

    private void UpdatePositionFromScale()
    {
        Vector3 position = Vector3.zero;

        position.x = leftBox.transform.localScale.x * 0.5f - 0.5f;
        leftBox.transform.localPosition = position;

        position = leftBox.transform.localPosition;
        position.x += leftBox.transform.localScale.x * 0.5f + rightBox.transform.localScale.x * 0.5f;
        rightBox.transform.localPosition = position;

        position = rightBox.transform.localPosition;
        position.x += rightBox.transform.localScale.x * 0.5f + tempBox.transform.localScale.x * 0.5f;
        tempBox.transform.localPosition = position;
    }

    private void UpdateScale()
    {
        if (leftBox.transform.localScale.x <= 0)
        {
            GameObject destroyBox = leftBox.gameObject;
            leftBox = rightBox;
            rightBox = tempBox;
            tempBox = GameObject.Instantiate<ReplicateBox>(leftBox);
            Destroy(destroyBox);

            tempBox.transform.SetParent(SlideBoxTransform);
            tempBox.transform.localScale = GetLocalXScale(0);

            float temp = leftOriginX;
            leftOriginX = rightOriginX;
            rightOriginX = temp;
        }

        Vector3 leftBoxScale = leftBox.transform.localScale;
        leftBoxScale.x -= moveSpeed * Time.deltaTime;
        leftBoxScale.x = leftBoxScale.x < 0 ? 0 : leftBoxScale.x;
        leftBox.transform.localScale = leftBoxScale;

        float tempX = leftOriginX - leftBoxScale.x;
        leftBoxScale.x = tempX;
        tempBox.transform.localScale = leftBoxScale;

        UpdatePositionFromScale();
    }

    private void Setup()
    {
        leftBox = GameObject.Instantiate<ReplicateBox>(prefab);
        leftBox.transform.SetParent(SlideBoxTransform);
        leftBox.transform.localScale = GetLocalXScale(ratio);
        leftOriginX = leftBox.transform.localScale.x;

        rightBox = GameObject.Instantiate<ReplicateBox>(prefab);
        rightBox.transform.SetParent(SlideBoxTransform);
        rightBox.transform.localScale = GetLocalXScale((1 - ratio));
        rightOriginX = rightBox.transform.localScale.x;

        tempBox = GameObject.Instantiate<ReplicateBox>(leftBox);
        tempBox.transform.SetParent(SlideBoxTransform);
        tempBox.transform.localScale = GetLocalXScale(0);

        UpdatePositionFromScale();
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (SlideBoxTransform == null)
            return;

        Vector3 leftBoxPosition = SlideBoxTransform.position;
        leftBoxPosition.x = SlideBoxTransform.position.x - SlideBoxTransform.lossyScale.x * 0.5f + SlideBoxTransform.transform.lossyScale.x * ratio * 0.5f;
        Vector3 rightBoxPosition = SlideBoxTransform.position;
        rightBoxPosition.x = SlideBoxTransform.position.x + SlideBoxTransform.lossyScale.x * 0.5f - SlideBoxTransform.transform.lossyScale.x * (1-ratio) * 0.5f;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(leftBoxPosition, new Vector3(SlideBoxTransform.transform.lossyScale.x * ratio,SlideBoxTransform.lossyScale.y, SlideBoxTransform.lossyScale.z));
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(rightBoxPosition, new Vector3(SlideBoxTransform.transform.lossyScale.x * (1-ratio),SlideBoxTransform.lossyScale.y, SlideBoxTransform.lossyScale.z));
    }

#endif

}
