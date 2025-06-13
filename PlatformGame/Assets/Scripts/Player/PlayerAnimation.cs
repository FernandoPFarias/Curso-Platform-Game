using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    [Header("Listen to Event Channels")]
    public GameEvent OnGroundJumpEvent;
    public GameEvent OnAirJumpEvent;
    public GameEvent OnLandedEvent;
    public GameEvent OnAttackEvent;

    // --- Referências ---
    private Animator animator;
    private PlayerController playerController;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponentInParent<PlayerController>();
    }

    private void OnEnable()
    {
        // Inscreve cada método ao seu respectivo canal de evento
        OnGroundJumpEvent?.RegisterListener(HandleGroundJump);
        OnAirJumpEvent?.RegisterListener(HandleAirJump);
        OnLandedEvent?.RegisterListener(HandleLanded);
        OnAttackEvent?.RegisterListener(HandleAttack);
    }

    private void OnDisable()
    {
        // Se desinscreve de todos os canais
        OnGroundJumpEvent?.UnregisterListener(HandleGroundJump);
        OnAirJumpEvent?.UnregisterListener(HandleAirJump);
        OnLandedEvent?.UnregisterListener(HandleLanded);
        OnAttackEvent?.UnregisterListener(HandleAttack);
    }

    private void Update()
    {
        // Atualiza os parâmetros contínuos a cada frame
        animator.SetFloat("HorizontalSpeed", Mathf.Abs(playerController.CurrentVelocity.x));
        animator.SetBool("isGrounded", playerController.IsGrounded);
        animator.SetFloat("VerticalSpeed", playerController.CurrentVelocity.y);
    }

    // --- MÉTODOS QUE REAGEM AOS EVENTOS ---

    private void HandleGroundJump()
    {
        animator.SetTrigger("GroundJump");
    }

    private void HandleAirJump()
    {
        animator.SetTrigger("DoubleJump");
    }



    private void HandleLanded()
    {
        animator.SetTrigger("Land");
    }

    private void HandleAttack()
    {
        animator.SetTrigger("T_IsBasicAttack");
    }




    public void PerformHitCheck()
    {
        // Pega a "Ficha de Ataque" que o PlayerCombat está usando no momento
        AttackData currentAttack = playerController.Combat.CurrentAttack;

        // CORREÇÃO LÓGICA: Sai da função se o ataque for nulo
        if (currentAttack == null) return;

        // CORREÇÃO DE SINTAXE: 'hitboxOffset' com 'o' minúsculo
        Vector2 hitboxCenter = (Vector2)playerController.transform.position +
                               new Vector2(currentAttack.hitboxOffSet.x * playerController.transform.localScale.x, currentAttack.hitboxOffSet.y);

        // Pede à física da Unity uma lista de todos os colliders dentro do círculo
        Collider2D[] hits = Physics2D.OverlapBoxAll(hitboxCenter, currentAttack.hitboxSize, currentAttack.damageableLayers);

        // CORREÇÃO DE SINTAXE: 'hit' em vez de 'hitbox'
        foreach (Collider2D hit in hits)
        {
            // CÓDIGO CORRIGIDO E DESCOMENTADO:
            if (hit.TryGetComponent<EnemyHealth>(out EnemyHealth enemy))
            {
               enemy.TakeDamage(currentAttack.attackdamage);
                Debug.Log($"Atingiu {hit.name} com o ataque {currentAttack.attackType}!");
            }
        }
    }
}