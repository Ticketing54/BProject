using UnityEngine;

public class ColorTest : MonoBehaviour
{
    public Material aa;

    bool test;
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            test = !test;

            aa.color = test ? Color.red : Color.blue;
                
        }

    }
}
