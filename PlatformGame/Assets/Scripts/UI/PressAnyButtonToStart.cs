using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System.Linq;

public class PressAnyButtonToStart : MonoBehaviour
{
    private bool started = false;
    private MenuUI menuUI;

    void Awake()
    {
        menuUI = FindObjectOfType<MenuUI>();
    }

    void Update()
    {
        if (started || menuUI == null) return;

        // Qualquer tecla do teclado
        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
            StartGame();

        // Qualquer botão do controle
        if (Gamepad.current != null && Gamepad.current.allControls.Any(c => c is ButtonControl btn && btn.wasPressedThisFrame))
            StartGame();

        // Qualquer toque na tela (mobile)
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            StartGame();
    }

    void StartGame()
    {
        started = true;
        menuUI.StartGame();
    }
} 