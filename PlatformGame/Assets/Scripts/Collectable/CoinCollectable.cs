using UnityEngine;

public class CoinCollectable : Collectable
{
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