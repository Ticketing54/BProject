using System.Collections;
using UnityEngine;


public class Ball : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Rigidbody rig;

    private void Awake()
    {
        GameManager.Instance.AddAliveBall(this);
    }

    private void OnDestroy()
    {
        GameManager.Instance.RemoveAlliveBall(this);
    }

}
