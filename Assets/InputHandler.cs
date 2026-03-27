using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{   
    private Vector2 prevPoint   = Vector2.zero;
    private Vector2 direction   = Vector2.zero;
    private GameManager gameManager => GameManager.Instance;

    private void Awake()
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        direction = eventData.position - prevPoint;
        prevPoint = eventData.position;

        gameManager.Direction = direction;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        prevPoint = eventData.position;

        gameManager.InputClickDown?.Invoke();

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        prevPoint = Vector2.zero;
        direction = Vector2.zero;

        gameManager.Direction = direction;
        gameManager.InputClickUp?.Invoke();
    }

}
