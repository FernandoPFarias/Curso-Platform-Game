using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(EnemyHealth), typeof(ContactDamage))]
public class Enemy : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private EnemyData _enemyData;
    [Header("Patrol Points")]
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;
    [Header("Sprite Settings")]
    public bool startsFacingLeft = false;

    public Rigidbody2D Rb { get; private set; }
    public EnemyAnimator AnimationManager { get; private set; }
    public Transform PlayerTarget { get; private set; }
    public EnemyData EnemyData => _enemyData;
    public Transform PointA => _pointA;
    public Transform PointB => _pointB;

    private AIBehaviour behaviour;



    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        AnimationManager = GetComponentInChildren<EnemyAnimator>();
    }

    private void Start()
    {
        if (_enemyData == null || _enemyData.behaviour == null) { this.enabled = false; return; }
        PlayerTarget = GameObject.FindGameObjectWithTag("Player")?.transform;
        behaviour = Instantiate(_enemyData.behaviour);
        behaviour.Initialize(this);
    }

    private void FixedUpdate()
    {
        behaviour?.Tick(this);
    }

    public AIBehaviour GetBehaviour() => behaviour;

    public void MoveTowards(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        Rb.linearVelocity = new Vector2(direction.x * _enemyData.moveSpeed, Rb.linearVelocity.y);
        FlipTowards(targetPosition); // O cérebro decide quando mover, o corpo sabe como virar
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

    // ... OnDrawGizmosSelected
}