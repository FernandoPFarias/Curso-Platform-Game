using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 16f;

    [Header("Jump Settings")]
    public int maxJumps = 2;

    [Header("Ground Check")]
    public Vector2 groundCheckBoxSize = new Vector2(0.5f, 0.1f);
    public Vector2 groundCheckBoxOffset = new Vector2(0f, -0.5f);
    public LayerMask groundLayer;
    
    [Header("Coyote Time (Buffer de Pulo)")]
    [SerializeField] private float coyoteTime = 0.1f; // Tempo de tolerância após sair do chão
    private float coyoteTimeCounter;

    [Header("Event Channels to Raise")]
    public GameEvent OnGroundJumpEvent; // Evento para o primeiro pulo
    public GameEvent OnAirJumpEvent;    // Evento para o pulo duplo
    public GameEvent OnLandedEvent;     // Evento para aterrissagem

    // --- Propriedades Públicas de Estado ---
    public Vector2 CurrentVelocity => rb.linearVelocity;
    public bool IsGrounded => isGrounded;
    public bool IsFacingRight => isFacingRight;
    public bool podeVirar = true;
    public bool podeAtacar = true;
    public bool segurandoCaixa = false;
    public PushableObject grabbedObject = null; // Pedra atualmente segurada

    // --- Componentes e Referências ---
    private Rigidbody2D rb;
    private InputSystem_Actions playerControls;
    public InputSystem_Actions PlayerControls => playerControls;

    // --- Variáveis de Estado Interno ---
    private Vector2 moveDirection = Vector2.zero;
    private int jumpsRemaining;
    private bool isGrounded;
    private bool wasGrounded;
    private bool isFacingRight = true;

    public PlayerCombat Combat { get; private set; }

    public static PlayerController Instance;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        rb = GetComponent<Rigidbody2D>();
        playerControls = new InputSystem_Actions();
        Combat = GetComponent<PlayerCombat>();
    }

    void Start()
    {
        ForceGroundCheck();
    }

    void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var spawn = GameObject.Find("PlayerSpawn");
        if (spawn != null)
            transform.position = spawn.transform.position;
    }
    
    // Método chamado quando a câmera é reconectada após mudança de cena
    public void OnCameraReconnected()
    {
        Debug.Log("PlayerController: Câmera reconectada!");
        
        // Reabilita o input do player se estava desabilitado
        if (!enabled)
        {
            enabled = true;
            Debug.Log("PlayerController: Input reabilitado após mudança de cena");
        }
        
        // Força um ground check após a mudança de cena
        ForceGroundCheck();
        
        // Notifica o GameManager para atualizar a UI se necessário
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ForceUIUpdate();
        }
    }

    public void ForceGroundCheck()
    {
        Vector2 checkPos = (Vector2)transform.position + groundCheckBoxOffset;
        isGrounded = Physics2D.OverlapBox(checkPos, groundCheckBoxSize, 0f, groundLayer);
        wasGrounded = isGrounded;
        if (isGrounded)
            jumpsRemaining = maxJumps;
    }

    private void OnEnable()
    {
        playerControls.Player.Enable();
        playerControls.Player.Jump.performed += OnJumpPerformed;
        playerControls.Player.Move.performed += ctx => {
            moveDirection = ctx.ReadValue<Vector2>();
            Debug.Log("Move: " + moveDirection);
        };
        playerControls.Player.Move.canceled += ctx => moveDirection = Vector2.zero;
    }

    private void OnDisable()
    {
        if (Instance == this)
            Instance = null;
        playerControls.Player.Disable();
        playerControls.Player.Jump.performed -= OnJumpPerformed;
        playerControls.Player.Move.performed -= ctx => moveDirection = ctx.ReadValue<Vector2>();
        playerControls.Player.Move.canceled -= ctx => moveDirection = Vector2.zero;
    }

    private void FixedUpdate()
    {
        wasGrounded = isGrounded;
        Vector2 checkPos = (Vector2)transform.position + groundCheckBoxOffset;
        isGrounded = Physics2D.OverlapBox(checkPos, groundCheckBoxSize, 0f, groundLayer);

        // Coyote time: buffer para permitir pulo logo após sair do chão
        if (isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.fixedDeltaTime;

        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocity.y);
    }

    private void Update()
    {
#if UNITY_ANDROID || UNITY_IOS
        // Movimento mobile
        Vector2 mobileMove = MobileInput.move;
        if (mobileMove != Vector2.zero)
        {
            moveDirection = mobileMove;
        }
        // Ataque mobile
        if (MobileInput.attackPressed)
        {
            if (Combat != null)
                Combat.SendMessage("OnBasicAttack", null, SendMessageOptions.DontRequireReceiver);
            MobileInput.attackPressed = false;
        }
        // Interação mobile
        if (MobileInput.interactPressed)
        {
            var interaction = GetComponent<PlayerInteraction>();
            if (interaction != null)
                interaction.SendMessage("OnInteract", null, SendMessageOptions.DontRequireReceiver);
            MobileInput.interactPressed = false;
        }
#endif
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
        Debug.Log($"PULO PRESSIONADO! jumpsRemaining: {jumpsRemaining} | context: {context}");
        // Permite pulo se ainda houver coyote time, mesmo que jumpsRemaining seja 0
        if (jumpsRemaining <= 0 && coyoteTimeCounter <= 0f)
        {
            return;
        }

        // Se nosso número de pulos é o MÁXIMO possível, então este SÓ PODE ser um pulo do chão.
        if (jumpsRemaining == maxJumps || coyoteTimeCounter > 0f)
        {
            OnGroundJumpEvent?.Raise(); // Dispara o evento de pulo no chão
        }
        else
        {
            OnAirJumpEvent?.Raise(); // Dispara o evento de pulo aéreo
        }

        jumpsRemaining--;
        coyoteTimeCounter = 0f; // Zera o buffer ao pular
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void HandleFlip()
    {
        if (!podeVirar) return;
        if (segurandoCaixa) return; // Não flipar enquanto segura a pedra
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        var collectable = other.GetComponent<Collectable>();
        if (collectable != null)
        {
            collectable.Collect(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 checkPos = (Vector2)transform.position + groundCheckBoxOffset;
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireCube(checkPos, groundCheckBoxSize);
    }
}