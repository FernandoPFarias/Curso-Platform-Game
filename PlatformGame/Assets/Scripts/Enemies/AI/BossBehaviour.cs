using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "AI/BossBehaviour")]
public class BossBehaviour : AIBehaviour
{
    // Parâmetros de comportamento geral
    public float chaseSpeed = 4f;
    public float timeBetweenAttacks = 1.5f; // Tempo mínimo entre ataques globais
    public float attackDuration = 0.7f; // Tempo "travado" durante o ataque
    public float cooldownDuration = 1.0f; // Tempo parado após atacar

    private enum BossState { Chasing, Attacking, Cooldown }
    private BossState currentState = BossState.Chasing;

    private bool isActive = false;
    private float lastAttackTime;
    private float lastAttack1Time;
    private float lastAttack2Time;
    private float stateTimer;

    public override void Initialize(Enemy puppet)
    {
        isActive = false;
        lastAttackTime = -timeBetweenAttacks;
        lastAttack1Time = -1f;
        lastAttack2Time = -1f;
        currentState = BossState.Chasing;
        stateTimer = 0f;
        Debug.Log($"[BossBehaviour] Inicializado para {puppet.gameObject.name}");
    }

    public override void Tick(Enemy puppet)
    {
        // Ativação por proximidade
        if (!isActive)
        {
            if (puppet.PlayerTarget != null && puppet.activationPoint != null)
            {
                float dist = Vector2.Distance(puppet.PlayerTarget.position, puppet.activationPoint.position);
                if (dist <= puppet.activationRange)
                {
                    isActive = true;
                    Debug.Log("[BossBehaviour] Boss ativado por proximidade!");
                }
            }
            puppet.AnimationManager.TriggerIdle();
            puppet.Rb.linearVelocity = Vector2.zero;
            return;
        }

        if (puppet.PlayerTarget == null)
        {
            puppet.AnimationManager.TriggerIdle();
            puppet.Rb.linearVelocity = Vector2.zero;
            return;
        }

        // Escolhe dinamicamente o ataque disponível e usa seu range
        // Cálculo correto: distância do centro da hitbox ao player
        float attackRange = 1f;
        int attackToUse = -1;
        Vector2 hitboxCenter = puppet.transform.position;
        var attacks = puppet.EnemyData.attacks;
        if (puppet.EnemyData.tier == EnemyTier.Boss && attacks != null && attacks.Length > 0)
        {
            if (Time.time - lastAttack1Time > attacks[0].cooldown)
            {
                attackRange = attacks[0].range;
                attackToUse = 0;
                hitboxCenter = (Vector2)puppet.transform.position + attacks[0].offset;
            }
            else if (attacks.Length > 1 && Time.time - lastAttack2Time > attacks[1].cooldown)
            {
                attackRange = attacks[1].range;
                attackToUse = 1;
                hitboxCenter = (Vector2)puppet.transform.position + attacks[1].offset;
            }
            else
            {
                attackRange = attacks[0].range;
                hitboxCenter = (Vector2)puppet.transform.position + attacks[0].offset;
            }
        }
        else
        {
            attackRange = puppet.EnemyData.attackRange;
            hitboxCenter = (Vector2)puppet.transform.position + puppet.EnemyData.attackOffset;
        }

        float distanceToPlayer = Vector2.Distance(hitboxCenter, puppet.PlayerTarget.position);

        switch (currentState)
        {
            case BossState.Chasing:
                if (distanceToPlayer > attackRange)
                {
                    puppet.SetMoveSpeed(chaseSpeed);
                    puppet.MoveTowards(puppet.PlayerTarget.position);
                    puppet.AnimationManager.animator.SetTrigger("T_Run");
                }
                else if (attackToUse != -1)
                {
                    // Executa o ataque disponível
                    var atk = attacks[attackToUse];
                    puppet.Rb.linearVelocity = Vector2.zero;
                    puppet.AnimationManager.TriggerIdle();
                    if (attackToUse == 0)
                        lastAttack1Time = Time.time;
                    else if (attackToUse == 1)
                        lastAttack2Time = Time.time;
                    lastAttackTime = Time.time;
                    puppet.AnimationManager.animator.SetTrigger(atk.triggerName);
                    currentState = BossState.Attacking;
                    stateTimer = attackDuration;
                }
                else
                {
                    puppet.Rb.linearVelocity = Vector2.zero;
                    puppet.AnimationManager.TriggerIdle();
                }
                break;
            case BossState.Attacking:
                puppet.Rb.linearVelocity = Vector2.zero;
                puppet.AnimationManager.TriggerIdle();
                stateTimer -= Time.fixedDeltaTime;
                if (stateTimer <= 0f)
                {
                    currentState = BossState.Cooldown;
                    stateTimer = cooldownDuration;
                }
                break;
            case BossState.Cooldown:
                puppet.Rb.linearVelocity = Vector2.zero;
                puppet.AnimationManager.TriggerIdle();
                stateTimer -= Time.fixedDeltaTime;
                if (stateTimer <= 0f)
                {
                    currentState = BossState.Chasing;
                }
                break;
        }
    }

    public override void DrawGizmos(Enemy enemy)
    {
        // Range de ataque
        var attacks = enemy.EnemyData.attacks;
        if (enemy.EnemyData.tier == EnemyTier.Boss && attacks != null && attacks.Length > 0)
        {
            for (int i = 0; i < attacks.Length; i++)
            {
                Gizmos.color = i == 0 ? Color.magenta : Color.cyan;
                Vector3 attackPos = enemy.transform.position + (Vector3)attacks[i].offset;
                Gizmos.DrawWireSphere(attackPos, attacks[i].range);
                #if UNITY_EDITOR
                UnityEditor.Handles.Label(attackPos + Vector3.up * attacks[i].range, $"Attack {i} Range");
                #endif
            }
        }
        else
        {
            Gizmos.color = Color.magenta;
            Vector3 attackPos = enemy.transform.position + (Vector3)enemy.EnemyData.attackOffset;
            Gizmos.DrawWireSphere(attackPos, enemy.EnemyData.attackRange);
            #if UNITY_EDITOR
            UnityEditor.Handles.Label(attackPos + Vector3.up * enemy.EnemyData.attackRange, "Attack Range");
            #endif
        }
        // Range de detecção
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(enemy.transform.position, enemy.EnemyData.detectionRange);
        #if UNITY_EDITOR
        UnityEditor.Handles.Label(enemy.transform.position + Vector3.right * enemy.EnemyData.detectionRange, "Detection Range");
        #endif

        // Range de give up
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(enemy.transform.position, enemy.EnemyData.giveUpRange);
        #if UNITY_EDITOR
        UnityEditor.Handles.Label(enemy.transform.position + Vector3.left * enemy.EnemyData.giveUpRange, "Give Up Range");
        #endif
    }
} 