using UnityEngine;

public class EndBox : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget;
    private int count = 0;

    private void Awake()
    {
        GameManager.Instance.ResetObject += ResetObject;
    }

    private void ResetObject()
    {
        count = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        count++;
        GameManager.Instance.SetScore(count);
        
        if(other.TryGetComponent<Ball>(out Ball endBall))
        {
            endBall.ConstraintsPositionZ(false);
        }
    }

}
