using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// Script para painel invisível de toque (lado esquerdo da tela)
// Atualiza a Input Action 'Move' do Input System
public class MobileTouchArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public InputSystem_Actions playerControls;
    private int pointerId = -1;
    private Vector2 startTouchPos;
    private bool isTouching = false;

    void Awake()
    {
        if (playerControls == null)
            playerControls = new InputSystem_Actions();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isTouching) return;
        isTouching = true;
        pointerId = eventData.pointerId;
        startTouchPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isTouching || eventData.pointerId != pointerId) return;
        Vector2 delta = eventData.position - startTouchPos;
        float threshold = 20f; // Pixels para considerar movimento
        Vector2 move = Vector2.zero;
        if (Mathf.Abs(delta.x) > threshold)
            move.x = Mathf.Sign(delta.x);
        // (Opcional) Se quiser pulo por swipe para cima, pode usar delta.y
        MobileInput.move = move;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerId != pointerId) return;
        isTouching = false;
        pointerId = -1;
        MobileInput.move = Vector2.zero;
    }
} 