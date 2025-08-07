using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float interactDistance = 1f;
    public LayerMask pushableLayer;
    private PushableObject grabbedObject;
    private int holdSide = 1; // 1 = direita, -1 = esquerda
    private LeverPuzzle nearbyLever;

    private InputSystem_Actions playerControls;

    void Awake()
    {
        playerControls = new InputSystem_Actions();
    }

    void OnEnable()
    {
        playerControls.Player.Enable();
        playerControls.Player.Throw.performed += OnThrow;
        playerControls.Player.Interact.performed += OnInteract;
    }

    void OnDisable()
    {
        playerControls.Player.Throw.performed -= OnThrow;
        playerControls.Player.Interact.performed -= OnInteract;
        playerControls.Player.Disable();
    }

    void Update()
    {
        // Detecta alavanca próxima continuamente
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, new Vector2(1, 1), 0);
        nearbyLever = null;
        foreach (var hit in hits)
        {
            var lever = hit.GetComponent<LeverPuzzle>();
            if (lever != null)
            {
                nearbyLever = lever;
                break;
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

    // Novo método para lidar com a interação
    private void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("OnInteract chamado! Context: " + context);
        
        // Primeiro, tenta interagir com checkpoint se estiver próximo
        var checkpoint = FindObjectOfType<Checkpoint>();
        if (checkpoint != null)
        {
            checkpoint.TryInteract();
            // Não retorna aqui, continua para verificar outros objetos
        }
        
        // Depois, tenta interagir com o portal se estiver próximo
        var portal = FindObjectOfType<Portal>();
        if (portal != null)
        {
            portal.TryInteract();
            // Não retorna aqui, continua para verificar outros objetos
        }
        
        // Depois, tenta interagir com uma alavanca próxima
        if (nearbyLever != null)
        {
            Debug.Log("Chamando Interact() na alavanca: " + nearbyLever.name);
            nearbyLever.Interact();
            return;
        }
        // Depois, lógica de empurrar/soltar objetos
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
                    // Atualiza referência no PlayerController
                    var controller = GetComponent<PlayerController>();
                    if (controller != null) controller.grabbedObject = grabbedObject;
                }
            }
        }
        else
        {
            grabbedObject.Release();
            // Limpa referência no PlayerController
            var controller = GetComponent<PlayerController>();
            if (controller != null) controller.grabbedObject = null;
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