using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CollisionBox : MonoBehaviour
{
    [SerializeField] GameManager.BallColor currentState;
    [SerializeField] private int count = 0;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI_Count;
    [SerializeField] private Renderer render;
    private GameManager.BallColor CurrentState => currentState;

    private HashSet<GameObject> instantiateBall  = new HashSet<GameObject>();

    public void Setup()
    {
        textMeshProUGUI_Count.text = $"{count} X";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (instantiateBall.Contains(other.gameObject))
            return;


    }


    private void OnValidate()
    {
        if (textMeshProUGUI_Count == null || render == null)
            return;

        Setup();
    }

}
