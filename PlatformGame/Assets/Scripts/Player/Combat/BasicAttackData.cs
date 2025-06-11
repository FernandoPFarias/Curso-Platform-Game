using UnityEngine;


[CreateAssetMenu(menuName ="Attacks/Basic Attack")]
public class BasicAttackData : AttackData
{
    public override void Execute(GameObject attacker)
    {
        // Pega o Animator do atacante para disparar a animação correta.
        Animator animator = attacker.GetComponentInChildren<Animator>();

        if (animator != null && !string.IsNullOrEmpty(animationTrrigerName))
        {
            animator.SetTrigger(animationTrrigerName);       
        }

        // Dispara os eventos de som e partícula associados a ESTE ataque.
        onAttackSfx?.Raise();
        onAttackVfx?.Raise();

        // A lógica de dano (ativar a hitbox) seria chamada aqui,
        // geralmente através de Animation Events disparados pela animação.
    }
}
