using UnityEngine;

[CreateAssetMenu(menuName = "AI Behaviours/Patrol Chase Attack")]

public class PatrolChaseAttackBehaviour : AIBehaviour
{
    [Header("Gizmos")]
    public Color gizmoColor = Color.cyan;

    private enum State { Patrolling, Alert, Chasing, Knockback, Attacking }
    private State currentState;
    private State previousState;

    private Transform currentPatrolTarget;
    private float stateTimer;
    private float patrolPointReachDistance = 0.5f; // Distância para considerar que chegou ao ponto
    private Vector2 knockbackDirection; // image.png
    private bool isInKnockback = false;
    private float knockbackStartTime; // Para controlar a força do knockback
    private float lastFireTime = -999f;
    private Transform projectileSpawnPoint;

    public override void Initialize(Enemy puppet)
    {
        // Inicializa com o ponto A como primeiro alvo
        currentPatrolTarget = puppet.PointA;
        // Tenta encontrar um ponto de spawn para o projétil (opcional)
        var spawn = puppet.transform.Find("ProjectileSpawn");
        projectileSpawnPoint = spawn != null ? spawn : puppet.transform;
        ChangeState(State.Patrolling, puppet);
    }

    public override void OnTakeDamage(Enemy puppet)
    {
        // Calcula a direção do knockback baseada na posição do player
        if (puppet.PlayerTarget != null)
        {
            knockbackDirection = (puppet.transform.position - puppet.PlayerTarget.position).normalized;
            Debug.Log($"Knockback calculado em direção ao player: {knockbackDirection}");
        }
        else
        {
            // Se não há player, usa uma direção aleatória
            knockbackDirection = Random.insideUnitCircle.normalized;
            Debug.Log($"Knockback aleatório: {knockbackDirection}");
        }
        Debug.Log($"Iniciando knockback para {puppet.EnemyData.enemyName} com força: {puppet.EnemyData.knockbackForce}");
        previousState = currentState;
        ChangeState(State.Knockback, puppet);
    }

    public override void Tick(Enemy puppet)
    {
        if (puppet.PlayerTarget == null)
        {
            if (currentState != State.Patrolling)
                ChangeState(State.Patrolling, puppet);
            return;
        }

        switch (currentState)
        {
            case State.Patrolling:
                if (IsPlayerDetected(puppet))
                {
                    if (currentState != State.Alert)
                        ChangeState(State.Alert, puppet);
                    break;
                }
                puppet.MoveTowards(currentPatrolTarget.position);
                if (Vector2.Distance(puppet.transform.position, currentPatrolTarget.position) < patrolPointReachDistance)
                {
                    currentPatrolTarget = (currentPatrolTarget == puppet.PointA) ? puppet.PointB : puppet.PointA;
                    Debug.Log($"Troquei para o ponto: {(currentPatrolTarget == puppet.PointA ? "A" : "B")}");
                }
                break;

            case State.Alert:
                stateTimer -= Time.fixedDeltaTime;
                if (stateTimer <= 0)
                {
                    ChangeState(State.Chasing, puppet);
                }
                break;

            case State.Chasing:
                if (!IsPlayerInChaseRange(puppet))
                {
                    ChangeState(State.Patrolling, puppet);
                    break;
                }
                // Ataca se estiver no range de ataque
                Vector3 offset = (Vector3)puppet.EnemyData.attackOffset;
                float rootScaleX = puppet.transform.root.lossyScale.x;
                if (rootScaleX < 0)
                    offset.x = -offset.x;
                float attackDistance = Vector2.Distance(puppet.transform.position + offset, puppet.PlayerTarget.position);
                if (attackDistance < puppet.EnemyData.attackRange)
                {
                    puppet.Rb.linearVelocity = Vector2.zero;
                    puppet.FlipTowards(puppet.PlayerTarget.position);
                    // Checa cooldown
                    float fireRate = 1f;
                    GameObject projectilePrefab = null;
                    if (puppet.EnemyData is RangedEnemyData rangedData)
                    {
                        fireRate = rangedData.fireRate > 0 ? rangedData.fireRate : 1f;
                        projectilePrefab = rangedData.projectilPrefab;
                    }
                    if (Time.time >= lastFireTime + 1f / fireRate && projectilePrefab != null)
                    {
                        puppet.AnimationManager?.animator.SetTrigger(puppet.EnemyData.attackTriggerName);
                        lastFireTime = Time.time;
                    }
                }
                else
                {
                    // No Tick, após calcular distâncias:
                    Vector2 minDistCenter = (Vector2)puppet.transform.position + puppet.EnemyData.minDistanceOffset;
                    float minDistance = puppet.EnemyData.minDistanceToPlayer;
                    float escapeSpeed = puppet.EnemyData.escapeSpeed;
                    float distanceToPlayer = Vector2.Distance(minDistCenter, puppet.PlayerTarget.position);

                    if (distanceToPlayer < minDistance)
                    {
                        // Player está muito perto: afasta rapidamente a partir do centro com offset, sem ignorar colisão
                        Vector2 dir = ((Vector2)minDistCenter - (Vector2)puppet.PlayerTarget.position).normalized;
                        Vector2 newPos = (Vector2)puppet.transform.position + dir * escapeSpeed * Time.fixedDeltaTime;
                        puppet.Rb.MovePosition(newPos);
                        puppet.AnimationManager?.animator.SetTrigger("T_Run");
                        return;
                    }
                    puppet.MoveTowards(puppet.PlayerTarget.position);
                }
                break;

            case State.Knockback:
                stateTimer -= Time.fixedDeltaTime;
                // Durante o stun, não faz nada (não move, não patrulha)
                if (stateTimer <= 0)
                {
                    puppet.Rb.linearVelocity = Vector2.zero;
                    isInKnockback = false;
                    // Após knockback, volta para o estado anterior
                    if (previousState == State.Chasing && IsPlayerDetected(puppet))
                    {
                        ChangeState(State.Chasing, puppet);
                    }
                    else
                    {
                        ChangeState(State.Patrolling, puppet);
                    }
                }
                break;
        }
    }

    private void ChangeState(State newState, Enemy puppet)
    {
        currentState = newState;
        switch (currentState)
        {
            case State.Patrolling:
                puppet.AnimationManager?.TriggerWalk();
                break;
            case State.Alert:
                puppet.Rb.linearVelocity = Vector2.zero;
                puppet.AnimationManager?.TriggerAlert();
                if (puppet.PlayerTarget != null) puppet.FlipTowards(puppet.PlayerTarget.position);
                stateTimer = puppet.EnemyData.alertDuration;
                break;
            case State.Chasing:
                puppet.AnimationManager?.TriggerChase();
                break;
            case State.Knockback:
                stateTimer = puppet.EnemyData.knockbackDuration;
                knockbackStartTime = Time.time;
                isInKnockback = true;
                puppet.AnimationManager?.TriggerHurt();
                // Aplica o knockback só uma vez ao entrar no estado
                Vector2 knockbackVelocity = knockbackDirection * puppet.EnemyData.knockbackForce.x;
                knockbackVelocity.y = puppet.EnemyData.knockbackForce.y;
                puppet.Rb.linearVelocity = knockbackVelocity;
                break;
        }
    }

    // Detecção usa detectionRange do EnemyData
    private bool IsPlayerDetected(Enemy puppet)
    {
        return Vector2.Distance(puppet.transform.position, puppet.PlayerTarget.position) < puppet.EnemyData.detectionRange;
    }

    // Give up usa giveUpRange do EnemyData
    private bool IsPlayerInChaseRange(Enemy puppet)
    {
        return Vector2.Distance(puppet.transform.position, puppet.PlayerTarget.position) < puppet.EnemyData.giveUpRange;
    }

    public override void DrawGizmos(Enemy enemy)
    {
        Gizmos.color = gizmoColor;
        Vector3 pos = enemy.transform.position;
        // detectionRange
        Gizmos.DrawWireSphere(pos, enemy.EnemyData.detectionRange);
        // giveUpRange
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pos, enemy.EnemyData.giveUpRange);
        // attackRange
        Gizmos.color = Color.magenta;
        Vector3 offset = (Vector3)enemy.EnemyData.attackOffset;
        float rootScaleX = enemy.transform.root.lossyScale.x;
        if (rootScaleX < 0)
            offset.x = -offset.x;
        Vector3 attackPos = enemy.transform.position + offset;
        Gizmos.DrawWireSphere(attackPos, enemy.EnemyData.attackRange);

        // No DrawGizmos, desenhar minDistanceToPlayer com offset
        if (enemy.EnemyData.minDistanceToPlayer > 0f)
        {
            Vector3 minDistCenter = enemy.transform.position + (Vector3)enemy.EnemyData.minDistanceOffset;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(minDistCenter, enemy.EnemyData.minDistanceToPlayer);
            #if UNITY_EDITOR
            UnityEditor.Handles.Label(minDistCenter + Vector3.up * enemy.EnemyData.minDistanceToPlayer, "Min Distance to Player");
            #endif
        }
    }

    // Método para instanciar o projétil
    private void FireProjectile(Enemy puppet, GameObject projectilePrefab)
    {
        if (projectilePrefab == null) return;
        Vector3 spawnPos = projectileSpawnPoint != null ? projectileSpawnPoint.position : puppet.transform.position;
        Vector2 direction = (puppet.PlayerTarget.position - spawnPos).normalized;
        GameObject proj = GameObject.Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        // Se o projétil tiver um script Projectile, configure a direção, velocidade e dano
        var projScript = proj.GetComponent<Projectile>();
        if (projScript != null && puppet.EnemyData is RangedEnemyData rangedData)
        {
            projScript.speed = rangedData.projectileSpeed;
            projScript.damage = rangedData.projectileDamage;
            projScript.SetDirection(direction);
        }
    }
}