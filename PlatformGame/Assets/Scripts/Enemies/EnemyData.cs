using UnityEngine;

//defini que tipo de inimigo � para que se eu quiser eu possa por drops
public enum EnemyTier {Mob, MiniBoss, Boss }

public class EnemyData : ScriptableObject
{
    [Header("Basic Info")]
    public string enemyName;
    public EnemyTier tier;

    [Header("Core Info")]
    public float maxHealth;
    public float moveSpeed;

    [Header("Combat Feedback")]
    public float hurtDuration = 0.5f;

    [Header("Effects")]
    public GameObject deathParticlesPrefab; // por particulas de morte se o inimigo tiver
    public GameEvent deathSoundEvent;      // por os sons de morte se o inimigo tiver

    [Header("Damage Types")]
    public float contactDamage; // vai ser o dano do toque



    [Header("AI Logic")]
    public float detectionRange; // Dist�ncia para "ver" o jogador e come�ar a perseguir
    public float giveUpRange;    // Dist�ncia para o jogador fugir e o inimigo desistir
    public float alertDuration = 1f; //tempo que o inimigo vai parar para seguir o player

    [Header("Animation")]
    public string attackTriggerName;

}
