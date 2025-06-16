using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(ContactDamage))]
public class Enemy : MonoBehaviour
{
    [Header("Data (Conecte o Asset do Inimigo aqui)")]
    [SerializeField] private EnemyData _enemyData;

    [Header("Patrol Points")]
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;

    [Header("Sprite Settings")]
    [Tooltip("Marque esta caixa se o sprite original deste inimigo estiver virado para a esquerda.")]
    public bool startsFacingLeft = false; 

    // --- PROPRIEDADES PÚBLICAS (AS "ALÇAS") ---
    // Os scripts de comportamento usarão estas propriedades para controlar o inimigo.
    public Rigidbody2D Rb { get; private set; }
    public Animator Animator { get; private set; }
    public Transform PlayerTarget { get; private set; }
    public EnemyData EnemyData => _enemyData;
    public Transform PointA => _pointA;
    public Transform PointB => _pointB;

    // O comportamento ativo, lido da ficha de dados.
    private AIBehaviour behaviour;
    private AIState currentState;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        Animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        Rb.gravityScale = 3f;
        Rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (_enemyData == null || _enemyData.behaviour == null)
        {
            Debug.LogError($"O Inimigo {gameObject.name} não tem um EnemyData ou um AIBehaviour configurado!", this);
            this.enabled = false;
            return;
        }

        // Clona o comportamento para que cada inimigo na cena tenha seu próprio estado.
        behaviour = Instantiate(_enemyData.behaviour);
        behaviour.Initialize(this);

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            PlayerTarget = playerObject.transform;
        }
    }

    private void FixedUpdate()
    {
        // A única tarefa do corpo é dizer ao seu comportamento atual para agir.
        behaviour?.Tick(this);
    }

    private void Update()
    {
        if (Animator != null)
        {
            Animator.SetFloat("Speed", Mathf.Abs(Rb.linearVelocity.x));
        }
    }

    // MÉTODO FALTANTE ADICIONADO AQUI:
    public void ChangeState(AIState newState)
    {
        currentState?.OnExit();
        currentState = newState;
        currentState?.OnEnter();
    }


    // --- Métodos de Utilidade ---
    public void MoveTowards(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        Rb.linearVelocity = new Vector2(direction.x * _enemyData.moveSpeed, Rb.linearVelocity.y);
    }

    public void FlipTowards(Vector2 targetPosition)
    {
        float directionToTargetX = targetPosition.x - transform.position.x;

        if (Mathf.Abs(directionToTargetX) > 0.1f)
        {
            // Pega o sinal da direção para o alvo (-1 para esquerda, 1 para direita)
            float targetDirectionSign = Mathf.Sign(directionToTargetX);

            // Se o sprite original é virado para a esquerda, nós invertemos a lógica.
            if (startsFacingLeft)
            {
                transform.localScale = new Vector3(-targetDirectionSign, 1f, 1f);
            }
            else // Senão, usamos a lógica padrão.
            {
                transform.localScale = new Vector3(targetDirectionSign, 1f, 1f);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // ... (Seu código de Gizmo pode continuar aqui, lendo de _enemyData)
    }
}