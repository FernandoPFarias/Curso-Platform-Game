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
    private readonly int returningHash = Animator.StringToHash("T_Returning");

    // Flags para verificar se os parâmetros existem
    private bool speedParameterExists = false;
    private bool isActingParameterExists = false;
    private bool goToIdleParameterExists = false;
    private bool startWalkParameterExists = false;
    private bool startChaseParameterExists = false;
    private bool triggerAlertParameterExists = false;
    private bool doAttackParameterExists = false;
    private bool hurtParameterExists = false;
    private bool deathParameterExists = false;
    private bool returningParameterExists = false;

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
            ValidateAnimatorParameters();
        }
    }

    private void ValidateAnimatorParameters()
    {
        if (animator == null || animator.runtimeAnimatorController == null)
        {
            Debug.LogError($"Animator ou RuntimeAnimatorController não encontrado em {gameObject.name}!");
            return;
        }

        // Verifica se cada parâmetro existe no Animator Controller
        speedParameterExists = ParameterExists("Speed");
        isActingParameterExists = ParameterExists("isActing");
        goToIdleParameterExists = ParameterExists("T_GoToIdle");
        startWalkParameterExists = ParameterExists("T_StartPatrol");
        startChaseParameterExists = ParameterExists("T_StartChase");
        triggerAlertParameterExists = ParameterExists("T_Alert");
        doAttackParameterExists = ParameterExists("T_DoAttack");
        hurtParameterExists = ParameterExists("T_Hurt");
        deathParameterExists = ParameterExists("T_Death");
        returningParameterExists = ParameterExists("T_Returning");

        // Log dos parâmetros que não existem
        if (!speedParameterExists) Debug.LogWarning($"Parâmetro 'Speed' não encontrado no Animator Controller de {gameObject.name}");
        if (!isActingParameterExists) Debug.LogWarning($"Parâmetro 'isActing' não encontrado no Animator Controller de {gameObject.name}");
        if (!goToIdleParameterExists) Debug.LogWarning($"Parâmetro 'T_GoToIdle' não encontrado no Animator Controller de {gameObject.name}");
        if (!startWalkParameterExists) Debug.LogWarning($"Parâmetro 'T_StartPatrol' não encontrado no Animator Controller de {gameObject.name}");
        if (!startChaseParameterExists) Debug.LogWarning($"Parâmetro 'T_StartChase' não encontrado no Animator Controller de {gameObject.name}");
        if (!triggerAlertParameterExists) Debug.LogWarning($"Parâmetro 'T_Alert' não encontrado no Animator Controller de {gameObject.name}");
        if (!doAttackParameterExists) Debug.LogWarning($"Parâmetro 'T_DoAttack' não encontrado no Animator Controller de {gameObject.name}");
        if (!hurtParameterExists) Debug.LogWarning($"Parâmetro 'T_Hurt' não encontrado no Animator Controller de {gameObject.name}");
        if (!deathParameterExists) Debug.LogWarning($"Parâmetro 'T_Death' não encontrado no Animator Controller de {gameObject.name}");
        if (!returningParameterExists) Debug.LogWarning($"Parâmetro 'T_Returning' não encontrado no Animator Controller de {gameObject.name}");
    }

    private bool ParameterExists(string parameterName)
    {
        if (animator == null || animator.runtimeAnimatorController == null) return false;
        
        foreach (var param in animator.parameters)
        {
            if (param.name == parameterName) return true;
        }
        return false;
    }

    // --- MÉTODOS PÚBLICOS (A NOSSA "API") ---

    public void UpdateSpeed(float speed)
    {
        if (animator != null && speedParameterExists)
            animator.SetFloat(speedHash, speed);
    }

    public void SetIsActing(bool isActing)
    {
        if (animator != null && isActingParameterExists)
            animator.SetBool(isActingHash, isActing);
    }

    public void TriggerIdle() 
    { 
        if (animator != null && goToIdleParameterExists) 
            animator.SetTrigger(goToIdleHash); 
    }
    
    public void TriggerWalk() 
    { 
        if (animator != null && startWalkParameterExists) 
            animator.SetTrigger(startWalkHash); 
    }
    
    public void TriggerChase() 
    { 
        if (animator != null && startChaseParameterExists) 
            animator.SetTrigger(startChaseHash); 
    }
    
    public void TriggerAlert() 
    { 
        if (animator != null && triggerAlertParameterExists) 
            animator.SetTrigger(triggerAlertHash); 
    }
    
    public void TriggerAttack() 
    { 
        if (animator != null && doAttackParameterExists) 
            animator.SetTrigger(doAttackHash); 
    }
    
    public void TriggerHurt() 
    { 
        if (animator != null && hurtParameterExists) 
            animator.SetTrigger(hurtHash); 
    }
    
    public void TriggerDeath() 
    { 
        if (animator != null && deathParameterExists) 
            animator.SetTrigger(deathHash); 
    }

    public void TriggerReturning() 
    { 
        if (animator != null && returningParameterExists) 
            animator.SetTrigger(returningHash); 
    }

    // Chame este método via Animation Event no último frame da animação de morte
    public void OnDeathAnimationEnd()
    {
        Destroy(transform.root.gameObject);
    }
}