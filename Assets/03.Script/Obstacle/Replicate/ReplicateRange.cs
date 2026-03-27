using System;
using TMPro;
using UnityEngine;
public class ReplicateRange : MonoBehaviour
{
    [Header("Visual & UI")]
    [SerializeField] private TextMeshProUGUI textMeshProUGUI_Count;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Canvas canvas;

    private DataBundle.BallColor targetColor;
    private int count;

    public DataBundle.BallColor TargetColor=> targetColor;
    public int Count => count;

    private Action<Ball, ReplicateRange> CheckBall;

    private void Awake()
    {
        if (ReferenceCheck())
            return;
    }

    public void Setup(Action<Ball, ReplicateRange> _duplicate)
    {
        CheckBall = _duplicate;
    }

    private bool ReferenceCheck()
    {
        if (textMeshProUGUI_Count == null)
        {
            Debug.LogError("ReplicateBox: TextMeshProUGUI_Count reference is missing.");
            return false;
        }
        if (meshRenderer == null)
        {
            Debug.LogError("ReplicateBox: MeshRenderer reference is missing.");
            return false;
        }
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ReplicateBox OnTriggerEnter");

        if (other.TryGetComponent<Ball>(out Ball ball))
        {
            CheckBall?.Invoke(ball, this);
        }
    }


    public void UpdateData(DataBundle.BallColor _targetColor, int _count)
    {
        if (!textMeshProUGUI_Count || !canvas || !meshRenderer)
            return;

        targetColor = _targetColor;
        count = Mathf.Clamp(_count, 0, 100);

        textMeshProUGUI_Count.text = $"X{count}";
        Color color = _targetColor == DataBundle.BallColor.BLUE ? DataBundle.COLOR_ALPHA_BLUE : DataBundle.COLOR_ALPHA_ORANGE;


        MaterialPropertyBlock propBlock = new MaterialPropertyBlock();

        meshRenderer.GetPropertyBlock(propBlock);

        propBlock.SetColor("_BaseColor", color);

        meshRenderer.SetPropertyBlock(propBlock);

        canvas.worldCamera = Camera.main;
    }

}
