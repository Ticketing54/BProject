using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject obj;
    public CinemachineTargetGroup group;
    public int count = 0;

    private HashSet<GameObject> collisionData = new HashSet<GameObject>();

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            Create();
        }


    }
    bool firstCreate = true;
    private void Create ()
    {
        GameObject ball = null;
        collisionData.Add(ball);
        ball = Instantiate<GameObject>(obj);

        Debug.Log(collisionData.Contains(ball));
        //StartCoroutine(CoCreate());
        //for(int i = 0; i < count; i++)
        //{
        //    GameObject newobj = GameObject.Instantiate(obj);
        //    newobj.transform.position = Vector3.zero;   
        //    newobj.SetActive(true);

        //    if(newobj.TryGetComponent<Rigidbody>(out Rigidbody rig))
        //    {
        //        rig.angularVelocity = Vector3.zero;
        //        rig.linearVelocity = Vector3.zero;
        //    }
        //}

        //float a = 1;
        //if (firstCreate)
        //{
        //    firstCreate = false;
        //    a = 200;
        //}
        //group.AddMember(newobj.transform, a, 1f);
    }

    WaitForSeconds waitTime = new WaitForSeconds(0.1f);

    private IEnumerator CoCreate()
    {
        for (int i = 0; i < count; i++)
        {
            GameObject newobj = GameObject.Instantiate(obj);
            newobj.transform.position = Vector3.zero;
            newobj.SetActive(true);

            if (newobj.TryGetComponent<Rigidbody>(out Rigidbody rig))
            {
                rig.angularVelocity = Vector3.zero;
                rig.linearVelocity = Vector3.zero;
            }
            yield return waitTime;
        }
    }

}
