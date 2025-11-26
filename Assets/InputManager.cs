using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    
    // Events
    public static Action<Vector2> OnDragEvent;

    public static Action OnClickDownEvent;
    public static Action OnClickUpEvent;
    public static Action GeneralClickEvent;

    private Vector2 prevPoint   = Vector2.zero;
    private Vector2 direction   = Vector2.zero;
    public static void SetGeneralClickEvent()
    {
        ResetAllEvent();
        OnClickUpEvent += GeneralClickEvent;
    }

    public static void ResetAllEvent()
    {
        OnDragEvent         = null;
        OnClickDownEvent    = null;
        OnClickUpEvent      = null;
    }

    public void OnDrag(PointerEventData eventData)
    {
        direction = eventData.position - prevPoint;

        OnDragEvent?.Invoke(direction);

        prevPoint = eventData.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        prevPoint = eventData.position;
        OnClickDownEvent?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        prevPoint = Vector2.zero;
        direction = Vector2.zero;

        OnClickUpEvent?.Invoke();
    }

}
