using UnityEngine;

public class HealthCollectable : Collectable
{
    public int healAmount = 1;
    public override void Collect(GameObject collector)
    {
        var playerHealth = collector.GetComponent<PlayerHealth>();
        if (playerHealth != null)
            playerHealth.Heal(healAmount);
        Destroy(gameObject);
    }
} 