using UnityEngine;
using UnityEngine.InputSystem;

public class PushableObject : MonoBehaviour
{
    public float pushSpeed = 3f;
    public bool isGrabbed = false;
    private Transform playerTransform;
    private Rigidbody2D rb;
    public bool podeArremessar = true;
    public Vector2 forcaArremesso = new Vector2(8f, 4f);
    private InputSystem_Actions playerActions;
    private InputAction throwAction;
    private bool playerEncostado = false;
    [SerializeField] public float pushForce = 15f; // Força do empurro padrão
    [SerializeField] public float maxPushSpeed = 4f; // Velocidade máxima da caixa ao empurrar

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Trava o movimento em X por padrão
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

    public void TryGrab(Transform player)
    {
        isGrabbed = true;
        playerTransform = player;
        var controller = player.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.enabled = false;
            controller.podeAtacar = false;
            controller.segurandoCaixa = true;
            playerActions = controller.PlayerControls;
            throwAction = playerActions.Player.Throw;
            throwAction.Enable();
        }
        var anim = player.GetComponentInChildren<Animator>();
        if (anim != null) anim.SetBool("Empurrando", true);

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void Release()
    {
        isGrabbed = false;
        if (playerTransform != null)
        {
            var controller = playerTransform.GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.enabled = true;
                controller.podeAtacar = true;
                controller.segurandoCaixa = false;
                if (throwAction != null) throwAction.Disable();
            }
            var anim = playerTransform.GetComponentInChildren<Animator>();
            if (anim != null) anim.SetBool("Empurrando", false);
        }
        playerTransform = null;
        rb.linearVelocity = Vector2.zero;

        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isGrabbed && other.transform == playerTransform)
        {
            playerEncostado = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (isGrabbed && other.transform == playerTransform)
        {
            playerEncostado = false;
        }
    }

    void FixedUpdate()
    {
        if (isGrabbed && playerTransform != null && playerEncostado)
        {
            float move = Input.GetAxisRaw("Horizontal");
            // Aplica força de empurro
            rb.AddForce(new Vector2(move * pushForce, 0f), ForceMode2D.Force);

            // Limita a velocidade máxima da caixa
            if (Mathf.Abs(rb.linearVelocity.x) > maxPushSpeed)
                rb.linearVelocity = new Vector2(Mathf.Sign(rb.linearVelocity.x) * maxPushSpeed, rb.linearVelocity.y);

            // Arremesso com Input System (Throw)
            if (podeArremessar && throwAction != null && throwAction.WasPressedThisFrame())
            {
                Debug.Log("Botão de arremesso pressionado!");
                Arremessar(playerTransform);
            }
        }
    }

    public void Arremessar(Transform player)
    {
        float direcaoX = Mathf.Sign(player.localScale.x);
        // Libera X antes do impulso
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        Vector2 forca = new Vector2(forcaArremesso.x * direcaoX, 0f);
        rb.AddForce(forca, ForceMode2D.Impulse);
        Debug.Log($"Arremessou a pedra! Força: {forca}, Direção: {direcaoX}");
        Release(); // Agora solta depois de aplicar a força
    }
} 