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
    [Header("Tempo de respiro após fuga")]
    public float waitBeforeChase = 1.5f; // Tempo de espera após fugir antes de perseguir novamente

    private enum BossState { Chasing, Attacking, Cooldown }
    private BossState currentState = BossState.Chasing;

    private bool isActive = false;
    private float lastAttackTime;
    private float lastAttack1Time;
    private float lastAttack2Time;
    private float stateTimer;
    private bool ignoringPlayerCollision = false;
    private Collider2D bossCollider;
    private Collider2D playerCollider;
    private float lastPushTime = -1f; // Cooldown do empurrão
    private Coroutine aiRoutine;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private bool forceReset = false;

    public override void Initialize(Enemy puppet)
    {
        isActive = false;
        lastAttackTime = -timeBetweenAttacks;
        lastAttack1Time = -1f;
        lastAttack2Time = -1f;
        currentState = BossState.Chasing;
        stateTimer = 0f;
        ignoringPlayerCollision = false;
        bossCollider = puppet.GetComponent<Collider2D>();
        if (puppet.PlayerTarget != null)
            playerCollider = puppet.PlayerTarget.GetComponent<Collider2D>();
        Debug.Log($"[BossBehaviour] Inicializado para {puppet.gameObject.name}");
        // Salva posição inicial
        initialPosition = puppet.transform.position;
        initialRotation = puppet.transform.rotation;
        forceReset = false;
        // Inicia a corrotina principal de IA
        if (aiRoutine != null) puppet.StopCoroutine(aiRoutine);
        aiRoutine = puppet.StartCoroutine(BossAI(puppet));
    }

    // Método público para resetar o boss
    public void ResetToInitialPosition(Enemy puppet)
    {
        forceReset = true;
        puppet.transform.position = initialPosition;
        puppet.transform.rotation = initialRotation;
        isActive = false;
        currentState = BossState.Chasing;
        stateTimer = 0f;
        Debug.Log("[BossBehaviour] Boss resetado para posição inicial e aguardando ativação.");
    }

    public override void Tick(Enemy puppet)
    {
        // Não faz nada aqui, a lógica está na corrotina
    }

    private IEnumerator BossAI(Enemy puppet)
    {
        while (true)
        {
            // Reset forçado: volta para posição inicial e espera ativação
            if (forceReset)
            {
                puppet.Rb.linearVelocity = Vector2.zero;
                puppet.AnimationManager.TriggerIdle();
                // Aguarda até player entrar na área de ativação
                while (!isActive)
                {
                    if (puppet.PlayerTarget != null && puppet.activationPoint != null)
                    {
                        float dist = Vector2.Distance(puppet.PlayerTarget.position, puppet.activationPoint.position);
                        if (dist <= puppet.activationRange)
                        {
                            isActive = true;
                            forceReset = false;
                            Debug.Log("[BossBehaviour] Boss reativado após reset!");
                        }
                    }
                    yield return null;
                }
            }
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
                yield return null;
                continue;
            }
            if (puppet.PlayerTarget == null)
            {
                puppet.AnimationManager.TriggerIdle();
                puppet.Rb.linearVelocity = Vector2.zero;
                yield return null;
                continue;
            }
            // Calcule o offset já invertido
            Vector2 offset = Vector2.zero;
            float attackRange = 1f;
            int attackToUse = -1;
            var attacks = puppet.EnemyData.attacks;
            float rootScaleX = puppet.transform.root.lossyScale.x;
            if (puppet.EnemyData.tier == EnemyTier.Boss && attacks != null && attacks.Length > 0)
            {
                if (Time.time - lastAttack1Time > attacks[0].cooldown)
                {
                    attackRange = attacks[0].range;
                    attackToUse = 0;
                    offset = attacks[0].offset;
                }
                else if (attacks.Length > 1 && Time.time - lastAttack2Time > attacks[1].cooldown)
                {
                    attackRange = attacks[1].range;
                    attackToUse = 1;
                    offset = attacks[1].offset;
                }
                else
                {
                    attackRange = attacks[0].range;
                    offset = attacks[0].offset;
                }
            }
            else
            {
                attackRange = puppet.EnemyData.attackRange;
                offset = puppet.EnemyData.attackOffset;
            }
            if (rootScaleX < 0) offset.x = -offset.x;
            // NOVO: usar minDistanceOffset como centro da zona de anti-grude
            Vector2 minDistCenter = (Vector2)puppet.transform.position + puppet.EnemyData.minDistanceOffset;
            Vector2 attackPoint = (Vector2)puppet.transform.position + offset;
            float minDistance = puppet.EnemyData.minDistanceToPlayer;
            float escapeSpeed = puppet.EnemyData.escapeSpeed;
            float distanceToPlayer = Vector2.Distance(minDistCenter, puppet.PlayerTarget.position);
            // 1. Se está muito perto, foge para o lado oposto ao player até sair da distância mínima
            if (distanceToPlayer < minDistance)
            {
                // Ignora colisão durante a fuga
                if (bossCollider != null && playerCollider != null)
                    Physics2D.IgnoreCollision(bossCollider, playerCollider, true);
                // Fuga dinâmica: recalcula direção e alvo a cada frame
                while (Vector2.Distance(minDistCenter, puppet.PlayerTarget.position) < minDistance)
                {
                    minDistCenter = (Vector2)puppet.transform.position + puppet.EnemyData.minDistanceOffset;
                    Vector2 fugaDir = (minDistCenter - (Vector2)puppet.PlayerTarget.position).normalized;
                    Vector2 fugaTarget = minDistCenter + fugaDir * minDistance * 1.2f; // Vai além da zona mínima
                    float fugaSpeed = escapeSpeed * 2f; // Fuga mais rápida
                    Vector2 moveDir = (fugaTarget - (Vector2)puppet.transform.position).normalized;
                    puppet.SetMoveSpeed(fugaSpeed);
                    puppet.Rb.MovePosition((Vector2)puppet.transform.position + moveDir * fugaSpeed * Time.fixedDeltaTime);
                    puppet.AnimationManager.animator.SetTrigger("T_Run");
                    yield return null;
                }
                // Reativa colisão ao sair da zona
                if (bossCollider != null && playerCollider != null)
                    Physics2D.IgnoreCollision(bossCollider, playerCollider, false);
                // NOVO: Espera um tempo antes de voltar a perseguir
                puppet.AnimationManager.TriggerIdle();
                puppet.Rb.linearVelocity = Vector2.zero;
                yield return new WaitForSeconds(waitBeforeChase);
                continue;
            }
            // 2. Se está na distância de ataque, ataca
            else if (distanceToPlayer < attackRange && attackToUse != -1)
            {
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
                // Espera o tempo do ataque
                yield return new WaitForSeconds(attackDuration);
                // Vai para cooldown
                currentState = BossState.Cooldown;
                stateTimer = cooldownDuration;
                yield return new WaitForSeconds(cooldownDuration);
                currentState = BossState.Chasing;
                continue;
            }
            // 3. Caso contrário, persegue
            else
            {
                puppet.SetMoveSpeed(chaseSpeed);
                puppet.MoveTowards(puppet.PlayerTarget.position);
                puppet.AnimationManager.animator.SetTrigger("T_Run");
                yield return null;
            }
        }
    }

    public override void DrawGizmos(Enemy enemy)
    {
        // Range de ataque
        var attacks = enemy.EnemyData.attacks;
        float rootScaleX = enemy.transform.root.lossyScale.x;
        if (enemy.EnemyData.tier == EnemyTier.Boss && attacks != null && attacks.Length > 0)
        {
            for (int i = 0; i < attacks.Length; i++)
            {
                Gizmos.color = i == 0 ? Color.magenta : Color.cyan;
                Vector3 offset = (Vector3)attacks[i].offset;
                if (rootScaleX < 0) offset.x = -offset.x;
                Vector3 attackPos = enemy.transform.position + offset;
                Gizmos.DrawWireSphere(attackPos, attacks[i].range);
                #if UNITY_EDITOR
                UnityEditor.Handles.Label(attackPos + Vector3.up * attacks[i].range, $"Attack {i} Range");
                #endif
            }
        }
        else
        {
            Gizmos.color = Color.magenta;
            Vector3 offset = (Vector3)enemy.EnemyData.attackOffset;
            if (rootScaleX < 0) offset.x = -offset.x;
            Vector3 attackPos = enemy.transform.position + offset;
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

        // No DrawGizmos, desenhar minDistanceToPlayer
        if (enemy.EnemyData.minDistanceToPlayer > 0f)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(enemy.transform.position, enemy.EnemyData.minDistanceToPlayer);
            #if UNITY_EDITOR
            UnityEditor.Handles.Label(enemy.transform.position + Vector3.up * enemy.EnemyData.minDistanceToPlayer, "Min Distance to Player");
            #endif
        }
    }
} 