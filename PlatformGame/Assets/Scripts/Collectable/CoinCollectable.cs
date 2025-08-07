using UnityEngine;

public class CoinCollectable : Collectable
{
    [Header("Visual Effects")]
    [Tooltip("Partícula de brilho que será desativada quando a moeda for coletada")]
    public ParticleSystem glowParticle;
    
    private Animator animator;
    private bool collected = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void Collect(GameObject collector)
    {
        if (collected) return;
        collected = true;
        
        // Desativa a partícula de brilho se estiver configurada
        if (glowParticle != null)
        {
            glowParticle.Stop();
            glowParticle.gameObject.SetActive(false);
        }
        
        CoinManager.Instance.AddCoin();
        if (animator != null)
        {
            animator.SetTrigger("T_CoinPick");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Chame este método via Animation Event no final da animação de coleta
    public void DestroyCoin()
    {
        Destroy(gameObject);
    }
} 