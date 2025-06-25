using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float interactDistance = 1f;
    public LayerMask pushableLayer;
    private PushableObject grabbedObject;
    private int holdSide = 1; // 1 = direita, -1 = esquerda

    private InputSystem_Actions playerControls;

    void Awake()
    {
        playerControls = new InputSystem_Actions();
    }

    void OnEnable()
    {
        playerControls.Player.Enable();
        playerControls.Player.Throw.performed += OnThrow;
    }

    void OnDisable()
    {
        playerControls.Player.Throw.performed -= OnThrow;
        playerControls.Player.Disable();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (grabbedObject == null)
            {
                // Detecta objeto empurrável próximo usando OverlapBox
                Vector2 boxCenter = (Vector2)transform.position + Vector2.right * transform.localScale.x * interactDistance * 0.5f;
                Vector2 boxSize = new Vector2(interactDistance, 1f);
                Collider2D obj = Physics2D.OverlapBox(boxCenter, boxSize, 0f, pushableLayer);
                if (obj != null)
                {
                    grabbedObject = obj.GetComponent<PushableObject>();
                    if (grabbedObject != null)
                    {
                        // Salva o lado
                        holdSide = (transform.position.x < obj.transform.position.x) ? -1 : 1;
                        grabbedObject.TryGrab(transform, holdSide);
                    }
                }
            }
            else
            {
                grabbedObject.Release();
                grabbedObject = null;
            }
        }
    }

    private void OnThrow(InputAction.CallbackContext ctx)
    {
        if (grabbedObject != null)
        {
            grabbedObject.Arremessar(transform);
            grabbedObject = null;
        }
    }

    // Visualização do OverlapBox no editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector2 boxCenter = (Vector2)transform.position + Vector2.right * transform.localScale.x * interactDistance * 0.5f;
        Vector2 boxSize = new Vector2(interactDistance, 1f);
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
} 