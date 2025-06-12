using UnityEngine;

// ... (RequireComponent continua o mesmo)
public class Enemy : MonoBehaviour
{
    [Header("Data")]
    public EnemyData enemyData;
    [Header("Patrol Points")]
    public Transform pointA;
    public Transform pointB;

    // Propriedades públicas para que os estados possam acessar os componentes do inimigo
    public Rigidbody2D Rb { get; private set; }
    public Transform PlayerTarget { get; private set; }
    public EnemyData EnemyData => enemyData; // Forma curta de expor a variável
    public Transform PointA => pointA;
    public Transform PointB => pointB;

    private AIState currentState; // O especialista que está trabalhando agora

    private void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        Rb.gravityScale = 2f;
        Rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        PlayerTarget = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Contrata o primeiro especialista: o de Patrulha
        ChangeState(new PatrolState(this));
    }

    private void FixedUpdate()
    {
        // A única tarefa do gerente é dizer ao especialista atual para trabalhar.
        currentState?.Tick();
    }


    // O gerente pode contratar um novo especialista a qualquer momento.
    public void ChangeState(AIState newState)
    {
        currentState?.OnExit();
        currentState = newState;
        currentState?.OnEnter();
    }

    // Métodos de utilidade que os especialistas podem usar
    public void MoveTowards(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        Rb.linearVelocity = new Vector2(direction.x * enemyData.moveSpeed, Rb.linearVelocity.y);
        FlipTowards(targetPosition);
    }

    public void FlipTowards(Vector2 targetPosition)
    {
        float directionX = targetPosition.x - transform.position.x;
        if (directionX > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (directionX < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    // O dano por toque ainda pode viver aqui, pois é uma propriedade do corpo físico do inimigo.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ... (seu código de dano por toque continua aqui)
    }

    // O Gizmo continua aqui também
    private void OnDrawGizmosSelected()
    {
        // ... (seu código de gizmo continua aqui)
    }
}