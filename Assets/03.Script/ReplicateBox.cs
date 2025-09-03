using System;
using TMPro;
using UnityEngine;
public class ReplicateBox : MonoBehaviour
{
    private static readonly Color BlueAlpha     = new Color(0f, 0f, 1f, 0.49f);
    private static readonly Color OrangeAlpha   = new Color(1f, 0.48f, 0f, 0.49f);

    [SerializeField] private GameObject cone;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI_Count;
    [SerializeField] private MeshRenderer meshRenderer;

    [HideInInspector] public int count = 0;
    [HideInInspector] public BallColor targetState;

    public Action<GameObject, int> ReplicateBall;


    private void OnEnable()
    {
        GameManager.ClickEvent += UpdateConeState;
        UpdateConeState();
    }

    private void OnDisable()
    {
        GameManager.ClickEvent -= UpdateConeState;
    }

    private void UpdateConeState()
    {
        cone.SetActive(GameManager.CurrentBallColor == targetState);
    }

    private void OnTriggerExit(Collider other)
    {
        if (GameManager.CurrentBallColor != targetState)
        {
            GameObject.Destroy(other.gameObject);
            return;
        }

        ReplicateBall?.Invoke(other.gameObject, count);
    }

#if UNITY_EDITOR
    [SerializeField] private Material blueMaterial;
    [SerializeField] private Material orangeMaterial;
    
    public void IsEnable(bool _isEnable)
    {
        textMeshProUGUI_Count.gameObject.SetActive(_isEnable);
        gameObject.SetActive(_isEnable);
    }

    public void UpdateData(Vector3 _position,Material _material)
    {
        textMeshProUGUI_Count.text = $"X{count}";
        textMeshProUGUI_Count.transform.position = _position - transform.forward;
        meshRenderer.material = _material;
    }

#endif
}
