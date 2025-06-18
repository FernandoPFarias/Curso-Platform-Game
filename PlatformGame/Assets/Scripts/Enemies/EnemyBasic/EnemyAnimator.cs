using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [Header("Animation Reference")]
    public Animator animator;

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
        // Se não foi atribuído via inspector, tenta encontrar automaticamente
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
            Debug.LogWarning($"Animator não foi atribuído via inspector em {gameObject.name}. Procurando automaticamente...");
        }
        
        if (animator == null)
        {
            Debug.LogError($"Animator não encontrado! Atribua manualmente via inspector ou verifique se existe um componente Animator nos filhos de {gameObject.name}.");
        }
        else
        {
            Debug.Log($"Animator configurado para {gameObject.name}: {animator.gameObject.name}");
        }
    }

    // --- MÉTODOS PÚBLICOS (A NOSSA "API") ---

    public void UpdateSpeed(float speed)
    {
        if (animator != null)
            animator.SetFloat(speedHash, speed);
    }

    public void SetIsActing(bool isActing)
    {
        if (animator != null)
            animator.SetBool(isActingHash, isActing);
    }

    public void TriggerIdle() 
    { 
        if (animator != null) 
            animator.SetTrigger(goToIdleHash); 
    }
    
    public void TriggerWalk() 
    { 
        if (animator != null) 
            animator.SetTrigger(startWalkHash); 
    }
    
    public void TriggerChase() 
    { 
        if (animator != null) 
            animator.SetTrigger(startChaseHash); 
    }
    
    public void TriggerAlert() 
    { 
        if (animator != null) 
            animator.SetTrigger(triggerAlertHash); 
    }
    
    public void TriggerAttack() 
    { 
        if (animator != null) 
            animator.SetTrigger(doAttackHash); 
    }
    
    public void TriggerHurt() 
    { 
        if (animator != null) 
            animator.SetTrigger(hurtHash); 
    }
    
    public void TriggerDeath() 
    { 
        if (animator != null) 
            animator.SetTrigger(deathHash); 
    }
}