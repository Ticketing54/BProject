using System;
using TMPro;
using UnityEngine;
public class ReplicateBox : MonoBehaviour
{
    [SerializeField] private int count = 0;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI_Count;
    [SerializeField] private BallColor currentState;

    private static readonly Color BlueAlpha     = new Color(0f, 0f, 1f, 0.49f);
    private static readonly Color OrangeAlpha   = new Color(1f, 0.48f, 0f, 0.49f);

    public Action<GameObject, int> ReplicateBall;

    private void Awake()
    {
        GameManager.ClickEvent += ConvertColor;
    }

    private void OnDestroy()
    {
        GameManager.ClickEvent -= ConvertColor;
    }

    private void ConvertColor()
    {
        currentState = currentState != BallColor.ORANGE ? BallColor.ORANGE : BallColor.BLUE;
    }

    private void OnTriggerExit(Collider other)
    {
        if (GameManager.CurrentBallColor != currentState)
        {
            GameObject.Destroy(other.gameObject);
            return;
        }

        ReplicateBall?.Invoke(other.gameObject, count);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        textMeshProUGUI_Count.text = $"X{count}";
    }

#endif
}
