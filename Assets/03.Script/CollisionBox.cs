using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CollisionBox : MonoBehaviour
{
    [SerializeField] private int count = 0;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI_Count;
    [SerializeField] private Renderer render;

    private HashSet<Ball> instantiateBall = new HashSet<Ball>();

    public void Setup()
    {
        textMeshProUGUI_Count.text = $"{count} X";
    }

    
    public void AddBall(Ball _ball)
    {
        if (instantiateBall.Contains(_ball))
            return;

        instantiateBall.Add(_ball);
    }


    public bool CheckDuplicate(Ball _ball) => instantiateBall.Contains(_ball);


    private void OnTriggerEnter(Collider other)
    {

    }


    private void OnValidate()
    {
        if (textMeshProUGUI_Count == null || render == null)
            return;

        Setup();
    }

}
