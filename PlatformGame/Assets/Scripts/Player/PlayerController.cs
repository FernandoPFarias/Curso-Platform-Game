using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 16f;

    [Header("Jump Settings")]
    public int maxJumps = 2;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Event Channels to Raise")]
    public GameEvent OnGroundJumpEvent; // Evento para o primeiro pulo
    public GameEvent OnAirJumpEvent;    // Evento para o pulo duplo
    public GameEvent OnLandedEvent;     // Evento para aterrissagem

    // --- Propriedades Públicas de Estado ---
    public Vector2 CurrentVelocity => rb.linearVelocity;
    public bool IsGrounded => isGrounded;
    public bool IsFacingRight => isFacingRight;

    // --- Componentes e Referências ---
    private Rigidbody2D rb;
    private InputSystem_Actions playerControls;

    // --- Variáveis de Estado Interno ---
    private Vector2 moveDirection = Vector2.zero;
    private int jumpsRemaining;
    private bool isGrounded;
    private bool wasGrounded;
    private bool isFacingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerControls = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        playerControls.Player.Enable();
        playerControls.Player.Jump.performed += OnJumpPerformed;
        playerControls.Player.Move.performed += ctx => moveDirection = ctx.ReadValue<Vector2>();
        playerControls.Player.Move.canceled += ctx => moveDirection = Vector2.zero;
    }

    private void OnDisable()
    {
        playerControls.Player.Disable();
        playerControls.Player.Jump.performed -= OnJumpPerformed;
        playerControls.Player.Move.performed -= ctx => moveDirection = ctx.ReadValue<Vector2>();
        playerControls.Player.Move.canceled -= ctx => moveDirection = Vector2.zero;
    }

    private void FixedUpdate()
    {
        wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocity.y);
    }

    private void Update()
    {
        // Se acabamos de aterrissar...
        if (!wasGrounded && isGrounded)
        {
            jumpsRemaining = maxJumps;
            OnLandedEvent?.Raise(); // ANUNCIA O POUSO
        }
        HandleFlip();
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        // Se não temos pulos restantes, não fazemos nada.
        if (jumpsRemaining <= 0)
        {
            return;
        }

        // A NOVA LÓGICA:
        // Se nosso número de pulos é o MÁXIMO possível, então este SÓ PODE ser um pulo do chão.
        // Esta verificação é mais confiável do que checar 'isGrounded' neste exato momento.
        if (jumpsRemaining == maxJumps)
        {
            OnGroundJumpEvent?.Raise(); // Dispara o evento de pulo no chão
        }
        else
        {
            // Se temos pulos, mas não é o máximo, então SÓ PODE ser um pulo aéreo.
            OnAirJumpEvent?.Raise(); // Dispara o evento de pulo aéreo
        }

        // A lógica da física é executada da mesma forma para qualquer pulo válido.
        jumpsRemaining--;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void HandleFlip()
    {
        if ((isFacingRight && moveDirection.x < 0f) || (!isFacingRight && moveDirection.x > 0f))
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, 1f, 1f);
    }
}