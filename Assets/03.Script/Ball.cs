using System.Collections;
using UnityEngine;


public class Ball : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Rigidbody rig;

    private void OnEnable()
    {
        GameManager.Instance.AddAliveBall(this);
    }

    private void OnDisable()
    {
        GameManager.Instance.RemoveAlliveBall(this);
    }
}
