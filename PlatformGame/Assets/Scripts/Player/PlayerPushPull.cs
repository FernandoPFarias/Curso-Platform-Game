using System.Collections.Generic;
using UnityEngine;

public class PlayerPushPull : MonoBehaviour
{
    public float detectionDistance = 1f;
    public float pushSpeed = 3f;
    public List<Rigidbody2D> objetosEmpurraveis; // Arraste aqui no Inspector

    private bool isGrabbing = false;
    private Rigidbody2D grabbedRb;
    private Animator anim;
    private InputSystem_Actions playerControls;
    private bool interagirPressed = false;
    private PlayerController playerController;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        playerControls = new InputSystem_Actions();
        playerController = GetComponent<PlayerController>();
    }

    void OnEnable()
    {
        playerControls.Player.Enable();
        playerControls.Player.Interact.performed += OnInteragirPerformed;
    }

    void OnDisable()
    {
        playerControls.Player.Disable();
        playerControls.Player.Interact.performed -= OnInteragirPerformed;
    }

    private void OnInteragirPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        interagirPressed = true;
    }

    void Update()
    {
        if (interagirPressed)
        {
            if (!isGrabbing)
            {
                // Detecta objeto próximo da lista
                foreach (var obj in objetosEmpurraveis)
                {
                    if (obj == null) continue;
                    float dist = Vector2.Distance(transform.position, obj.position);
                    if (dist <= detectionDistance)
                    {
                        isGrabbing = true;
                        grabbedRb = obj;
                        anim.SetBool("Empurrando", true);
                        if (playerController != null) playerController.podeVirar = false;
                        break;
                    }
                }
            }
            else
            {
                // Solta a pedra ao apertar novamente
                isGrabbing = false;
                grabbedRb = null;
                anim.SetBool("Empurrando", false);
                if (playerController != null) playerController.podeVirar = true;
            }
            interagirPressed = false;
        }
    }

    void FixedUpdate()
    {
        if (isGrabbing && grabbedRb != null)
        {
            float dist = Vector2.Distance(transform.position, grabbedRb.position);
            if (dist > detectionDistance)
            {
                // Solta automaticamente se afastar demais
                isGrabbing = false;
                grabbedRb = null;
                anim.SetBool("Empurrando", false);
                return;
            }

            float move = playerControls.Player.Move.ReadValue<Vector2>().x;
            grabbedRb.linearVelocity = new Vector2(move * pushSpeed, grabbedRb.linearVelocity.y);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector3(detectionDistance * 2, 1f, 0.1f));
    }
} 