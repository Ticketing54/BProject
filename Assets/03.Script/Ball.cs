using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<CollisionBox>(out CollisionBox range))
        {
            if (!range.CheckDuplicate(this))
                StartCoroutine(CoCreateBall(range));
        }
    }



    private IEnumerator CoCreateBall(CollisionBox range)
    {
    //    for (int i = 0; i < range.Count; i++)
    //    {
    //        Ball newBall = Instantiate<Ball>(this);
    //        range.AddBall(newBall);

            yield return new WaitForSeconds(0.1f);
    //    }

    //    Destroy(this.gameObject);
    }

}
