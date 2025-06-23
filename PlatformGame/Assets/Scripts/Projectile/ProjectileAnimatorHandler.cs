using UnityEngine;

public class ProjectileAnimatorHandler : MonoBehaviour
{
    [SerializeField] private Animator animator; // Arraste o Animator correto aqui

    public void PlayLaunch()
    {
        if (animator != null)
            animator.SetTrigger("T_Bomb");
    }

    public void PlayExplosion()
    {
        Debug.Log("PlayExplosion chamado!");
        if (animator != null)
            animator.SetTrigger("T_Explosion");
    }

    // Chame este método via Animation Event no fim da animação de explosão
    public void DestroyProjectile()
    {
        Destroy(transform.root.gameObject); // destrói o projétil inteiro
    }
} 