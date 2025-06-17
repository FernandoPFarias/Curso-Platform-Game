using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
    private Animator animator;

    // Converte os nomes dos parâmetros em "apelidos" numéricos para performance
    private readonly int speedHash = Animator.StringToHash("Speed");
    private readonly int isActingHash = Animator.StringToHash("isActing");
    private readonly int goToIdleHash = Animator.StringToHash("T_GoToIdle");
    private readonly int startWalkHash = Animator.StringToHash("T_StartPatrol"); // Usando seu nome
    private readonly int startChaseHash = Animator.StringToHash("T_StartChase"); // Usando seu nome
    private readonly int triggerAlertHash = Animator.StringToHash("T_Alert");
    private readonly int doAttackHash = Animator.StringToHash("T_DoAttack");
    private readonly int hurtHash = Animator.StringToHash("T_Hurt");
    private readonly int deathHash = Animator.StringToHash("T_Death");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // --- MÉTODOS PÚBLICOS (A NOSSA "API") ---

    public void UpdateSpeed(float speed)
    {
        animator.SetFloat(speedHash, speed);
    }

    public void SetIsActing(bool isActing)
    {
        animator.SetBool(isActingHash, isActing);
    }

    public void TriggerIdle() => animator.SetTrigger(goToIdleHash);
    public void TriggerWalk() => animator.SetTrigger(startWalkHash);
    public void TriggerChase() => animator.SetTrigger(startChaseHash);
    public void TriggerAlert() => animator.SetTrigger(triggerAlertHash);
    public void TriggerAttack() => animator.SetTrigger(doAttackHash);
    public void TriggerHurt() => animator.SetTrigger(hurtHash);
    public void TriggerDeath() => animator.SetTrigger(deathHash);
}