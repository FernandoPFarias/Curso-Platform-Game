using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

// Script para botões mobile (ataque, interação)
// Dispara a Input Action correspondente ao clicar
public class MobileButton : MonoBehaviour
{
    public enum ButtonType { Attack, Interact, Jump }
    public ButtonType buttonType;
    public InputSystem_Actions playerControls;
    private Button btn;

    void Awake()
    {
        btn = GetComponent<Button>();
        if (playerControls == null)
            playerControls = new InputSystem_Actions();
        btn.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        switch (buttonType)
        {
            case ButtonType.Attack:
                MobileInput.attackPressed = true;
                break;
            case ButtonType.Interact:
                MobileInput.interactPressed = true;
                break;
            case ButtonType.Jump:
                MobileInput.jumpPressed = true;
                break;
        }
    }
} 