using UnityEngine;

[CreateAssetMenu(menuName = "AI Behaviours/Sentry Behaviour")]
public class SentryBehaviour : AIBehaviour
{
    [Header("Gizmos")]
    public float detectionRange = 5f;
    public float visionAngle = 60f; // cone de visão
    public Color gizmoColor = Color.yellow;
    public bool useConeVision = true; // se true, desenha cone; se false, desenha 360
    [Header("Visão")] 
    public Vector2 visionDirection = Vector2.right;
    public Vector2 visionOffset = Vector2.zero;

    private enum State { Idling, Alert, Chasing, Returning, Knockback }
    private State currentState;
    private State previousState;
    private float stateTimer;
    private float lastFireTime = -999f;
    private Transform projectileSpawnPoint;

    public override void Initialize(Enemy puppet)
    {
        // Tenta encontrar um ponto de spawn para o projétil (opcional)
        var spawn = puppet.transform.Find("ProjectileSpawn");
        projectileSpawnPoint = spawn != null ? spawn : puppet.transform;
        ChangeState(State.Idling, puppet);
    }

    public override void OnTakeDamage(Enemy puppet)
    {
        previousState = currentState;
        ChangeState(State.Knockback, puppet);
    }

    public override void Tick(Enemy puppet)
    {
        if (puppet.PlayerTarget == null && currentState != State.Idling)
        {
            ChangeState(State.Idling, puppet);
            return;
        }

        // Limite de área: só persegue se dentro dos limites
        bool playerInArea = true;
        if (puppet.SentryGuardPoint != null && puppet.SentryChaseLimit != null && puppet.PlayerTarget != null)
        {
            float minX = Mathf.Min(puppet.SentryGuardPoint.position.x, puppet.SentryChaseLimit.position.x);
            float maxX = Mathf.Max(puppet.SentryGuardPoint.position.x, puppet.SentryChaseLimit.position.x);
            float playerX = puppet.PlayerTarget.position.x;
            playerInArea = playerX >= minX && playerX <= maxX;
        }

        switch (currentState)
        {
            case State.Idling:
                if (IsPlayerDetected(puppet) && playerInArea)
                {
                    ChangeState(State.Alert, puppet);
                }
                break;

            case State.Alert:
                stateTimer -= Time.fixedDeltaTime;
                if (stateTimer <= 0)
                {
                    if (playerInArea)
                        ChangeState(State.Chasing, puppet);
                    else
                        ChangeState(State.Idling, puppet);
                }
                break;

            case State.Chasing:
                puppet.SetMoveSpeed(puppet.EnemyData.chaseSpeed); // Velocidade de perseguição
                if (!playerInArea || Vector2.Distance(puppet.transform.position, puppet.PlayerTarget.position) > puppet.EnemyData.giveUpRange)
                {
                    ChangeState(State.Returning, puppet);
                    break;
                }
                puppet.FlipTowards(puppet.PlayerTarget.position); // Sempre vira para o player

                if (puppet.EnemyData is RangedEnemyData rangedData)
                {
                    float distToPlayer = Vector2.Distance(puppet.transform.position, puppet.PlayerTarget.position);
                    if (distToPlayer > rangedData.attackRange)
                    {
                        // Player está fora do range de ataque, anda até o range
                        puppet.MoveTowards(puppet.PlayerTarget.position);
                    }
                    else
                    {
                        // Player está no range de ataque, para e atira
                        puppet.Rb.linearVelocity = Vector2.zero;
                        float fireRate = rangedData.fireRate > 0 ? rangedData.fireRate : 1f;
                        GameObject projectilePrefab = rangedData.projectilPrefab;
                        if (Time.time >= lastFireTime + 1f / fireRate && projectilePrefab != null)
                        {
                            puppet.AnimationManager?.animator.SetTrigger(puppet.EnemyData.attackTriggerName);
                            lastFireTime = Time.time;
                            FireProjectile(puppet, projectilePrefab, rangedData);
                        }
                    }
                    break;
                }

                // --- MELEE padrão ---
                Vector3 offset = (Vector3)puppet.EnemyData.attackOffset;
                float rootScaleX = puppet.transform.root.lossyScale.x;
                if (rootScaleX < 0)
                    offset.x = -offset.x;
                float attackDistance = Vector2.Distance(puppet.transform.position + offset, puppet.PlayerTarget.position);
                float buffer = 0.05f; // Pequeno buffer para evitar colar

                if (attackDistance < puppet.EnemyData.attackRange - buffer)
                {
                    puppet.Rb.linearVelocity = Vector2.zero;
                    puppet.AnimationManager?.animator.SetTrigger(puppet.EnemyData.attackTriggerName);
                }
                else
                {
                    puppet.MoveTowards(puppet.PlayerTarget.position);
                }
                break;

            case State.Returning:
                puppet.SetMoveSpeed(puppet.EnemyData.moveSpeed); // Velocidade normal de patrulha
                // NOVO: Se o player voltou para a área e está detectado, volta a perseguir imediatamente
                if (puppet.PlayerTarget != null && playerInArea && IsPlayerDetected(puppet))
                {
                    ChangeState(State.Chasing, puppet);
                    break;
                }
                // Volta para o ponto de guarda
                if (puppet.SentryGuardPoint != null)
                {
                    float stopOffset = 0.2f; // Para não colar na parede
                    float dist = Vector2.Distance(puppet.transform.position, puppet.SentryGuardPoint.position);
                    if (dist > stopOffset)
                    {
                        puppet.MoveTowards(puppet.SentryGuardPoint.position);
                    }
                    else
                    {
                        puppet.Rb.linearVelocity = Vector2.zero;
                        // Não cola no ponto exato
                        // puppet.transform.position = puppet.SentryGuardPoint.position;
                        // Flip para o centro da patrulha
                        if (puppet.PointA != null && puppet.PointB != null)
                        {
                            Vector3 patrolCenter = (puppet.PointA.position + puppet.PointB.position) / 2f;
                            puppet.FlipTowards(patrolCenter);
                        }
                        else
                        {
                            puppet.FlipToDefault();
                        }
                        ChangeState(State.Idling, puppet);
                    }
                }
                break;

            case State.Knockback:
                stateTimer -= Time.fixedDeltaTime;
                if (stateTimer <= 0)
                {
                    puppet.Rb.linearVelocity = Vector2.zero;
                    if (previousState == State.Chasing && IsPlayerDetected(puppet) && playerInArea)
                    {
                        ChangeState(State.Chasing, puppet);
                    }
                    else
                    {
                        ChangeState(State.Idling, puppet);
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
            case State.Idling:
                puppet.Rb.linearVelocity = Vector2.zero;
                puppet.AnimationManager?.TriggerIdle();
                puppet.AnimationManager?.UpdateSpeed(0f);
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
            case State.Returning:
                puppet.AnimationManager?.TriggerReturning();
                break;
            case State.Knockback:
                stateTimer = puppet.EnemyData.knockbackDuration;
                puppet.AnimationManager?.TriggerHurt();
                break;
        }
    }

    // Detecção usando cone de visão
    private bool IsPlayerDetected(Enemy puppet)
    {
        if (puppet.PlayerTarget == null) return false;
        Vector2 origin = (Vector2)puppet.transform.position + visionOffset;
        Vector2 toPlayer = (Vector2)puppet.PlayerTarget.position - origin;
        float distanceToPlayer = toPlayer.magnitude;
        if (distanceToPlayer > puppet.EnemyData.detectionRange) return false;
        Vector2 forward = (visionDirection == Vector2.zero ? Vector2.right : visionDirection).normalized;
        if (puppet.startsFacingLeft) forward = -forward;
        float angle = Vector2.Angle(forward, toPlayer.normalized);
        if (useConeVision && angle > visionAngle / 2f) return false;
        // Raycast para garantir que não há obstáculos
        RaycastHit2D hit = Physics2D.Raycast(origin, toPlayer.normalized, distanceToPlayer, LayerMask.GetMask("Player", "Ground"));
        return hit.collider != null && hit.collider.CompareTag("Player");
    }

    public override void DrawGizmos(Enemy enemy)
    {
        // Exibe detection/vision range, give up range e attack range para ambos os tipos
        if (enemy.EnemyData is RangedEnemyData rangedData)
        {
            // GIZMOS PARA RANGED
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(enemy.transform.position, rangedData.visionRange); // Vision range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(enemy.transform.position, enemy.EnemyData.giveUpRange); // Give up range
            Gizmos.color = Color.magenta;
            Vector3 attackPos = enemy.transform.position + (Vector3)rangedData.attackOffset;
            Gizmos.DrawWireSphere(attackPos, rangedData.attackRange); // Attack range
#if UNITY_EDITOR
            UnityEditor.Handles.Label(enemy.transform.position + Vector3.up * 1.5f, $"Fire Rate: {rangedData.fireRate:F2}/s");
#endif
        }
        else
        {
            // GIZMOS PARA MELEE
            Gizmos.color = gizmoColor;
            Vector3 pos = enemy.transform.position + (Vector3)visionOffset;
            Vector3 forward = (visionDirection == Vector2.zero ? Vector2.right : visionDirection).normalized;
            if (enemy.startsFacingLeft) forward = -forward;
            if (!useConeVision)
            {
                Gizmos.DrawWireSphere(pos, detectionRange);
            }
            else
            {
                float halfAngle = visionAngle / 2f;
                Quaternion leftRayRotation = Quaternion.AngleAxis(-halfAngle, Vector3.forward);
                Quaternion rightRayRotation = Quaternion.AngleAxis(halfAngle, Vector3.forward);
                Vector3 leftRay = leftRayRotation * forward * detectionRange;
                Vector3 rightRay = rightRayRotation * forward * detectionRange;
                Gizmos.DrawRay(pos, leftRay);
                Gizmos.DrawRay(pos, rightRay);
                DrawWireArc(pos, Vector3.forward, leftRay, visionAngle, detectionRange);
            }
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(enemy.transform.position, enemy.EnemyData.giveUpRange); // Give up range
            Gizmos.color = Color.magenta;
            Vector3 attackPos = enemy.transform.position + (Vector3)enemy.EnemyData.attackOffset;
            Gizmos.DrawWireSphere(attackPos, enemy.EnemyData.attackRange);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(enemy.transform.position, enemy.EnemyData.detectionRange);
        }
    }

    // Método auxiliar para desenhar arco
    private void DrawWireArc(Vector3 center, Vector3 normal, Vector3 from, float angle, float radius, int segments = 32)
    {
        float angleStep = angle / segments;
        Vector3 prevPoint = center + from.normalized * radius;
        for (int i = 1; i <= segments; i++)
        {
            Vector3 nextDir = Quaternion.AngleAxis(angleStep * i, normal) * from.normalized;
            Vector3 nextPoint = center + nextDir * radius;
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }

    // Método para instanciar o projétil
    private void FireProjectile(Enemy puppet, GameObject projectilePrefab, RangedEnemyData rangedData)
    {
        if (projectilePrefab == null) return;
        Vector3 spawnPos = projectileSpawnPoint != null ? projectileSpawnPoint.position : puppet.transform.position;
        Vector2 direction = (puppet.PlayerTarget.position - spawnPos).normalized;
        GameObject proj = GameObject.Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        var bomb = proj.GetComponent<BombProjectile>();
        if (bomb != null)
        {
            bomb.SetDamage(rangedData.projectileDamage);
            bomb.LaunchArc(puppet.PlayerTarget.position);
        }
        else
        {
            var projScript = proj.GetComponent<ProjectileBase>();
            if (projScript != null)
            {
                projScript.SetDamage(rangedData.projectileDamage);
                projScript.SetDirection(direction);
            }
        }
    }
}