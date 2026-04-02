using UnityEngine;

public class EndBox : MonoBehaviour
{
    public int count = 0;

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
