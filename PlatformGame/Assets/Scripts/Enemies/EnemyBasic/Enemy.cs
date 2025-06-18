using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(EnemyHealth), typeof(ContactDamage))]
public class Enemy : MonoBehaviour
{
    [Header("Data")]
    public EnemyData enemyData;
    [Header("Patrol Points")]
    public Transform pointA;
    public Transform pointB;
    [Header("Animation Reference")]
    public EnemyAnimator animationManager;
    [Header("Sprite Settings")]
    public bool startsFacingLeft = false;

    public Rigidbody2D Rb { get; private set; }
    public EnemyAnimator AnimationManager => animationManager;
    public Transform PlayerTarget { get; private set; }
    public EnemyData EnemyData => enemyData;
    public Transform PointA => pointA;
    public Transform PointB => pointB;

    private AIBehaviour behaviour;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        if (animationManager == null)
        {
            animationManager = GetComponentInChildren<EnemyAnimator>();
            Debug.LogWarning($"EnemyAnimator não foi atribuído via inspector em {gameObject.name}. Procurando automaticamente...");
        }
        if (animationManager == null)
        {
            Debug.LogError($"EnemyAnimator não encontrado! Atribua manualmente via inspector ou verifique se existe o componente EnemyAnimator nos filhos de {gameObject.name}.");
        }
    }

    private void Start()
    {
        if (enemyData == null || enemyData.behaviour == null)
        {
            Debug.LogError($"EnemyData ou Behaviour não configurado em {gameObject.name}!");
            this.enabled = false;
            return;
        }

        PlayerTarget = GameObject.FindGameObjectWithTag("Player")?.transform;
        behaviour = Instantiate(enemyData.behaviour);
        behaviour.Initialize(this);

        // Garante que o EnemyHealth tenha acesso ao EnemyData
        var enemyHealth = GetComponent<EnemyHealth>();
        if (enemyHealth != null && enemyHealth.enemyData == null)
        {
            enemyHealth.enemyData = enemyData;
        }
    }

    private void FixedUpdate()
    {
        behaviour?.Tick(this);
    }

    public AIBehaviour GetBehaviour() => behaviour;

    public void MoveTowards(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        Rb.linearVelocity = new Vector2(direction.x * enemyData.moveSpeed, Rb.linearVelocity.y);
        FlipTowards(targetPosition);
    }

    public void FlipTowards(Vector2 targetPosition)
    {
        float directionToTargetX = targetPosition.x - transform.position.x;
        if (Mathf.Abs(directionToTargetX) > 0.1f)
        {
            float targetDirectionSign = Mathf.Sign(directionToTargetX);
            transform.localScale = new Vector3(startsFacingLeft ? -targetDirectionSign : targetDirectionSign, 1f, 1f);
        }
    }

    private void OnDrawGizmos()
    {
        if (enemyData != null && enemyData.behaviour != null)
        {
            enemyData.behaviour.DrawGizmos(this);
        }
    }
}