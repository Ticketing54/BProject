using Unity.Cinemachine;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject obj;
    public CinemachineTargetGroup group;


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
        GameObject newobj = GameObject.Instantiate(obj);
        newobj.SetActive(true);

        float a = 1;
        if(firstCreate)
        {
            firstCreate = false;
            a = 200;
        }
        group.AddMember(newobj.transform, a, 1f);
    }

}
