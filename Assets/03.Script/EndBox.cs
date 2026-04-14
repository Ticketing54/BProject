using UnityEngine;

public class EndBox : MonoBehaviour
{
    private int count = 0;
    private bool firstTouch = false;

    private void OnEnable()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.ResetObject += ResetObject;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetObject -= ResetObject;
        }
    }

    private void ResetObject()
    {
        count = 0;
        firstTouch = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        count++;
        GameManager.Instance.SetScore(count);
        
        if(other.TryGetComponent<Ball>(out Ball endBall))
        {
            if (!firstTouch)
            {
                firstTouch = !firstTouch;
                GameManager.Instance.FixCameraEndPosition();
            }

            endBall.ConstraintsPositionZ(false);
        }
    }

}
