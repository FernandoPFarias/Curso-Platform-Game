using UnityEngine;
using UnityEngine.InputSystem;

public class PushableObject : MonoBehaviour
{
    public float pushForce = 15f;
    public float maxPushSpeed = 4f;
    private bool isGrabbed = false;
    private Transform playerTransform;
    private Rigidbody2D rb;
    private int holdSide = 1;
    public bool podeArremessar = true;
    public Vector2 forcaArremesso = new Vector2(8f, 4f);
    private InputSystem_Actions playerActions;
    private InputAction throwAction;
    public float maxGrabDistance = 1.2f; // Distância máxima permitida para manter a pedra segurada
    [Header("Sensores de chão")]
    public Vector2 groundSensorAOffset = new Vector2(-0.2f, -0.5f);
    public float groundSensorARadius = 0.08f;
    public Vector2 groundSensorBOffset = new Vector2(0.2f, -0.5f);
    public float groundSensorBRadius = 0.08f;
    public LayerMask groundLayer;
    private bool foiArremessada = false;
    [Header("Comportamento com Inimigos")]
    public bool atravessavelPorInimigos = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

    void Start()
    {
        if (atravessavelPorInimigos)
        {
            var inimigos = FindObjectsOfType<Enemy>(); // Troque Enemy pelo nome do seu script de inimigo se necessário
            var col = GetComponent<Collider2D>();
            foreach (var inimigo in inimigos)
            {
                var inimigoCol = inimigo.GetComponent<Collider2D>();
                if (inimigoCol != null && col != null)
                    Physics2D.IgnoreCollision(col, inimigoCol);
            }
        }
    }

    public void TryGrab(Transform player, int side)
    {
        isGrabbed = true;
        playerTransform = player;
        holdSide = side;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        var controller = player.GetComponent<PlayerController>();
        if (controller != null) controller.segurandoCaixa = true;
        var anim = player.GetComponentInChildren<Animator>();
        if (anim != null) anim.SetBool("Empurrando", true);

        // Adiciona FixedJoint2D no player conectado à pedra
        var joint = player.GetComponent<FixedJoint2D>();
        if (joint == null) joint = player.gameObject.AddComponent<FixedJoint2D>();
        joint.connectedBody = rb;
        joint.enableCollision = false;
    }

    public void Release(bool travarX = true, bool zerarVelocidade = true)
    {
        isGrabbed = false;
        if (playerTransform != null)
        {
            var controller = playerTransform.GetComponent<PlayerController>();
            if (controller != null) {
                controller.segurandoCaixa = false;
                controller.grabbedObject = null; // Limpa referência ao soltar
            }
            var anim = playerTransform.GetComponentInChildren<Animator>();
            if (anim != null) anim.SetBool("Empurrando", false);
            // Remove FixedJoint2D do player
            var joint = playerTransform.GetComponent<FixedJoint2D>();
            if (joint != null) GameObject.Destroy(joint);
        }
        playerTransform = null;
        if (zerarVelocidade)
            rb.linearVelocity = Vector2.zero;
        if (travarX)
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        else
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void FixedUpdate()
    {
        if (isGrabbed && playerTransform != null)
        {
            float move = Input.GetAxisRaw("Horizontal");
            rb.AddForce(new Vector2(move * pushForce, 0f), ForceMode2D.Force);
            if (Mathf.Abs(rb.linearVelocity.x) > maxPushSpeed)
                rb.linearVelocity = new Vector2(Mathf.Sign(rb.linearVelocity.x) * maxPushSpeed, rb.linearVelocity.y);

            // Sensores de chão configuráveis
            Vector2 sensorAPos = (Vector2)transform.position + groundSensorAOffset;
            Vector2 sensorBPos = (Vector2)transform.position + groundSensorBOffset;
            bool sensorA = Physics2D.OverlapCircle(sensorAPos, groundSensorARadius, groundLayer);
            bool sensorB = Physics2D.OverlapCircle(sensorBPos, groundSensorBRadius, groundLayer);
            if (!sensorA && !sensorB)
            {
                Release();
            }

            // Solta se o player se afastar demais da pedra
            float dist = Vector2.Distance(transform.position, playerTransform.position);
            if (dist > maxGrabDistance)
            {
                Release();
                return;
            }
        }

        // Travar X após arremesso quando parar
        if (foiArremessada && Mathf.Abs(rb.linearVelocity.x) < 0.01f)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            foiArremessada = false;
        }
    }

    public void Arremessar(Transform player)
    {
        float direcaoX = Mathf.Sign(player.localScale.x);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        // Remove FixedJoint2D do player ANTES de aplicar a força
        var joint = player.GetComponent<FixedJoint2D>();
        if (joint != null) GameObject.Destroy(joint);
        // NÃO afasta a pedra!
        Vector2 forca = new Vector2(forcaArremesso.x * direcaoX, forcaArremesso.y);
        rb.AddForce(forca, ForceMode2D.Impulse);
        Debug.Log($"Arremessou a pedra! Força: {forca}, Direção: {direcaoX}");
        foiArremessada = true;
        Release(false, false);
    }

    public bool IsGrabbed => isGrabbed;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere((Vector2)transform.position + groundSensorAOffset, groundSensorARadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + groundSensorBOffset, groundSensorBRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxGrabDistance);
    }
} 