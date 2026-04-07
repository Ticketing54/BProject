using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int CreateBallCount = 1;

    GameManager gameManager => GameManager.Instance;

    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(CoSpawnBall());
        }
    }

    private IEnumerator CoSpawnBall()
    {
        int count = 0;

        while(count < CreateBallCount)
        {
            yield return new WaitForSeconds(0.1f);
            
            Vector3 sprayPosition = Vector3.zero;
            sprayPosition.x = Random.Range(-0.5f, 0.5f);
            sprayPosition+= transform.position;

            Ball newball = gameManager.CreateBall(sprayPosition);

            count++;
        }
    }
}
