using Mono.Cecil.Cil;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject ballprefab;

    public Transform spawnPosition;


    private void Spawn()
    {
        if (!ballprefab || !spawnPosition)
        {
            Debug.LogError("Check Referrece !");
            return;
        }
            

        GameObject temp = Instantiate(ballprefab);

        temp.transform.position = spawnPosition.position;
    }


    private void OnGUI()
    {
        if(GUI.Button(new Rect(10,10,200,100), "Spawn"))
        {

            Spawn();

        }
    }


}
