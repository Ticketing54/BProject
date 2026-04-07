using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private Image input_Image;
    private Vector2 prevPoint   = Vector2.zero;
    private Vector2 direction   = Vector2.zero;
    private GameManager gameManager => GameManager.Instance;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            BlockInput(true);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            BlockInput(false);
        }
    }
    private void OnEnable()
    {
        if (gameManager != null)
        {
            gameManager.OnBlockInput += BlockInput;
        }
    }

    private void OnDisable()
    {   
        if(gameManager!=null)
        {
            gameManager.OnBlockInput -= BlockInput;
        }
    }

    private void BlockInput(bool _isBlock) => input_Image.raycastTarget = !_isBlock;

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
