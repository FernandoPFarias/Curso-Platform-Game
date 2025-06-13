using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    [Header("Listen to Event Channels")]
    public GameEvent OnGroundJumpEvent;
    public GameEvent OnAirJumpEvent;
    public GameEvent OnLandedEvent;
    public GameEvent OnAttackEvent;

    // --- Refer�ncias ---
    private Animator animator;
    private PlayerController playerController;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponentInParent<PlayerController>();
    }

    private void OnEnable()
    {
        // Inscreve cada m�todo ao seu respectivo canal de evento
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
        // Atualiza os par�metros cont�nuos a cada frame
        animator.SetFloat("HorizontalSpeed", Mathf.Abs(playerController.CurrentVelocity.x));
        animator.SetBool("isGrounded", playerController.IsGrounded);
        animator.SetFloat("VerticalSpeed", playerController.CurrentVelocity.y);
    }

    // --- M�TODOS QUE REAGEM AOS EVENTOS ---

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
        // Pega a "Ficha de Ataque" que o PlayerCombat est� usando no momento
        AttackData currentAttack = playerController.Combat.CurrentAttack;

        // CORRE��O L�GICA: Sai da fun��o se o ataque for nulo
        if (currentAttack == null) return;

        // CORRE��O DE SINTAXE: 'hitboxOffset' com 'o' min�sculo
        Vector2 hitboxCenter = (Vector2)playerController.transform.position +
                               new Vector2(currentAttack.hitboxOffSet.x * playerController.transform.localScale.x, currentAttack.hitboxOffSet.y);

        // Pede � f�sica da Unity uma lista de todos os colliders dentro do c�rculo
        Collider2D[] hits = Physics2D.OverlapBoxAll(hitboxCenter, currentAttack.hitboxSize, currentAttack.damageableLayers);

        // CORRE��O DE SINTAXE: 'hit' em vez de 'hitbox'
        foreach (Collider2D hit in hits)
        {
            // C�DIGO CORRIGIDO E DESCOMENTADO:
            if (hit.TryGetComponent<EnemyHealth>(out EnemyHealth enemy))
            {
               enemy.TakeDamage(currentAttack.attackdamage);
                Debug.Log($"Atingiu {hit.name} com o ataque {currentAttack.attackType}!");
            }
        }
    }
}