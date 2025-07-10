using UnityEngine;

public enum EnemyTier { Normal, MiniBoss, Boss }

public abstract class EnemyData : ScriptableObject
{
    [Header("Basic Info")]
    public string enemyName;
    public EnemyTier tier;

    [Header("Core Stats")]
    public float maxHealth;
    public float moveSpeed;
    public float chaseSpeed = 6f;

    [Header("Damage Types")]
    public float contactDamage;

    [Header("Combat Feedback")]
    public float hurtDuration = 0.5f;
    public Vector2 knockbackForce = new Vector2(5f, 3f);
    public float knockbackDuration = 0.3f;

    [Header("AI Logic")]
    public float detectionRange;
    public float giveUpRange;
    public float alertDuration = 1f;

    [Header("AI Behaviour")]
    public AIBehaviour behaviour;

    [Header("Effects")]
    public GameObject deathParticlesPrefab;
    public GameEvent onDeathEvent;

    [System.Serializable]
    public class AttackInfo
    {
        public string name; // Ex: "Attack1", "Attack2"
        public float damage;
        public float range;
        public Vector2 offset;
        public float cooldown;
        public string triggerName; // Ex: "T_Attack1"
    }

    [Header("Attack Settings (Comum)")]
    public string attackTriggerName = "T_DoAttack";
    public float attackRange = 1f;
    public Vector2 attackOffset = Vector2.zero;

    [Header("Attack Settings (Boss)")]
    // Preencha este array apenas para bosses!
    public AttackInfo[] attacks;
}