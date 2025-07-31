using UnityEngine;
using System.Collections;

// Gerencia a saúde do jogador, respawn e lógica de morte
public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private bool isDead = false;
    private bool isInvincible = false;
    public float invincibleTime = 2f; // tempo de invencibilidade após respawn

    private Animator animator;

    public float CurrentHealth => GameManager.Instance != null ? GameManager.Instance.playerHealth : (GameManager.Instance != null ? GameManager.Instance.maxHealth : 100f);

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        // Garante que a vida do GameManager nunca passe do máximo
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.playerHealth > maxHealth || GameManager.Instance.playerHealth <= 0)
                GameManager.Instance.playerHealth = maxHealth;
        }
    }

    private void OnDestroy()
    {
        // Nada a fazer, GameManager já tem a vida atual
    }

    // Aplica dano ao jogador
    public void TakeDamage(float amount)
    {
        if (isDead || isInvincible) return;
        if (GameManager.Instance != null)
        {
            GameManager.Instance.playerHealth -= amount;
            if (GameManager.Instance.playerHealth < 0)
                GameManager.Instance.playerHealth = 0;
        }
        Debug.Log($"JOGADOR tomou {amount} de dano! Vida atual: {CurrentHealth}");

        if (CurrentHealth <= 0)
        {
            // Morte real - perde vida extra e respawna com vida cheia
            DieAndRespawn();
            return;
        }

        animator?.SetTrigger("Hurt");
    }


    // Aplica penalidade de vida e respawna (usado por DeathZone)
    public void ApplyPenaltyAndRespawn(float penalty)
    {
        Debug.Log($"ApplyPenaltyAndRespawn: Iniciando com penalidade {penalty}, vida atual {CurrentHealth}, isDead: {isDead}");
        
        if (isDead) 
        {
            Debug.Log("ApplyPenaltyAndRespawn: Player já está morto, ignorando");
            return;
        }
        
        isDead = true;
        
        if (GameManager.Instance != null)
        {
            float newHealth = CurrentHealth - penalty;
            GameManager.Instance.playerHealth = newHealth;
            Debug.Log($"ApplyPenaltyAndRespawn: Penalidade aplicada: {penalty}. Nova vida: {newHealth}");
            
            // Se a penalidade deixou a vida em 0 ou menos, trata como morte real
            if (newHealth <= 0)
            {
                Debug.Log($"ApplyPenaltyAndRespawn: Vida chegou a {newHealth}, tratando como morte real");
                if (GameManager.Instance.playerLives > 0)
                {
                    GameManager.Instance.playerLives--;
                    GameManager.Instance.playerHealth = GameManager.Instance.maxHealth;
                    Debug.Log($"ApplyPenaltyAndRespawn: Player perdeu uma vida por DeathZone! Vidas restantes: {GameManager.Instance.playerLives}");
                    Debug.Log($"ApplyPenaltyAndRespawn: Vida restaurada para: {GameManager.Instance.playerHealth}");
                }
                else
                {
                    Debug.Log("ApplyPenaltyAndRespawn: Game Over - sem mais vidas");
                    // Game Over
                    GameManager.Instance.GameOver();
                    animator?.SetTrigger("Death");
                    GetComponent<PlayerController>().enabled = false;
                    GetComponent<PlayerCombat>().enabled = false;
                    GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                    GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
                    GetComponent<Collider2D>().enabled = false;
                    Debug.Log("Jogador foi derrotado! Fim de jogo - Sem mais vidas extras.");
                    return;
                }
            }
            else
            {
                Debug.Log($"ApplyPenaltyAndRespawn: Vida restante após penalidade: {newHealth}");
            }
        }
        else
        {
            Debug.LogError("ApplyPenaltyAndRespawn: GameManager.Instance é null!");
        }
        
        var fade = FindObjectOfType<FadeController>();
        if (fade != null)
        {
            Debug.Log("ApplyPenaltyAndRespawn: FadeController encontrado, iniciando fade");
            fade.Fade(
                onBlack: () => {
                    // Se morreu por penalidade, respawn com vida cheia; senão, com vida restante
                    float respawnHealth = (GameManager.Instance.playerHealth <= 0) ? GameManager.Instance.maxHealth : GameManager.Instance.playerHealth;
                    Debug.Log($"ApplyPenaltyAndRespawn: Callback onBlack executado! Respawnando com vida: {respawnHealth}");
                    RespawnAtCheckpoint(respawnHealth);
                },
                onComplete: () => {
                    Debug.Log("ApplyPenaltyAndRespawn: Fade completo!");
                }
            );
            
            // Fallback: se o fade não funcionar em 2 segundos, respawna mesmo assim
            StartCoroutine(FallbackRespawn());
        }
        else
        {
            Debug.Log("ApplyPenaltyAndRespawn: FadeController não encontrado, respawnando direto");
            float respawnHealth = (GameManager.Instance.playerHealth <= 0) ? GameManager.Instance.maxHealth : GameManager.Instance.playerHealth;
            Debug.Log($"ApplyPenaltyAndRespawn: Respawnando com vida: {respawnHealth}");
            RespawnAtCheckpoint(respawnHealth);
        }
    }

    // Lida com morte e respawn completo (usado por inimigos/projéteis)
    public void DieAndRespawn()
    {
        if (isDead) return;
        isDead = true;
        
        if (GameManager.Instance != null && GameManager.Instance.playerLives > 0)
        {
            GameManager.Instance.playerLives--;
            GameManager.Instance.playerHealth = GameManager.Instance.maxHealth;
            Debug.Log($"Player perdeu uma vida! Vidas restantes: {GameManager.Instance.playerLives}");
            Debug.Log($"Vida restaurada para: {GameManager.Instance.playerHealth}");
        }
        else if (GameManager.Instance != null)
        {
            // Game Over
            GameManager.Instance.GameOver();
            animator?.SetTrigger("Death");
            GetComponent<PlayerController>().enabled = false;
            GetComponent<PlayerCombat>().enabled = false;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            GetComponent<Collider2D>().enabled = false;
            Debug.Log("Jogador foi derrotado! Fim de jogo - Sem mais vidas extras.");
            return;
        }
        
        var fade = FindObjectOfType<FadeController>();
        if (fade != null)
        {
            fade.Fade(
                onBlack: () => {
                    RespawnAtCheckpoint(GameManager.Instance.maxHealth);
                }
            );
        }
        else
        {
            RespawnAtCheckpoint(GameManager.Instance.maxHealth);
        }
    }

    // DeathZone chama este método
    public void RespawnFromDeathZone(float penalty)
    {
        ApplyPenaltyAndRespawn(penalty);
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;
        DieAndRespawn();
    }

    // Respawna o jogador no checkpoint
    private void RespawnAtCheckpoint(float healthAfterPenalty)
    {
        Debug.Log($"RespawnAtCheckpoint: Iniciando respawn com vida {healthAfterPenalty}");
        
        var rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic; // Desabilita física
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        float yOffset = GameManager.Instance != null ? GameManager.Instance.lastCheckpointYOffset : 0.3f;
        Vector3 spawnPosition = GameManager.Instance.lastCheckpointPosition + Vector3.up * yOffset;
        transform.position = spawnPosition;
        
        Debug.Log($"RespawnAtCheckpoint: Posição de spawn: {spawnPosition}");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.playerHealth = healthAfterPenalty;
            if (GameManager.Instance.playerHealth > GameManager.Instance.maxHealth)
                GameManager.Instance.playerHealth = GameManager.Instance.maxHealth;
            Debug.Log($"RespawnAtCheckpoint: Vida atual = {GameManager.Instance.playerHealth}, Vidas = {GameManager.Instance.playerLives}");
            Debug.Log($"RespawnAtCheckpoint: HeartUIController existe? {GameManager.Instance.heartUIController != null}");
        }
        else
        {
            Debug.LogError("RespawnAtCheckpoint: GameManager.Instance é null!");
        }
        
        isDead = false;
        GetComponent<PlayerController>().enabled = true;
        GetComponent<PlayerCombat>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
        animator?.SetTrigger("Respawn");
        StartCoroutine(InvincibilityCoroutine());
        StartCoroutine(ReenablePhysicsNextFrame(rb));
        Debug.Log("Player respawned at checkpoint!");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetCameraFollow(transform);
            GameManager.Instance.ForceUIUpdate();
            Debug.Log("RespawnAtCheckpoint: Camera e UI atualizadas");
        }

        // NOVO: Resetar todos os bosses após respawn
        var allEnemies = GameObject.FindObjectsOfType<Enemy>();
        Debug.Log($"RespawnAtCheckpoint: Encontrados {allEnemies.Length} inimigos na cena");
        foreach (var enemy in allEnemies)
        {
            if (enemy.EnemyData != null && enemy.EnemyData.tier == EnemyTier.Boss)
            {
                var bossBehaviour = enemy.GetBehaviour() as BossBehaviour;
                if (bossBehaviour != null)
                {
                    Debug.Log($"RespawnAtCheckpoint: Resetando boss {enemy.name}");
                    bossBehaviour.ResetToInitialPosition(enemy);
                }
            }
        }
    }

    private System.Collections.IEnumerator ReenablePhysicsNextFrame(Rigidbody2D rb)
    {
        yield return null; // espera 1 frame
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private System.Collections.IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        float elapsed = 0f;
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        bool visible = true;
        float blinkInterval = 0.1f;

        while (elapsed < invincibleTime)
        {
            visible = !visible;
            foreach (var sr in renderers)
                sr.enabled = visible;
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }
        foreach (var sr in renderers)
            sr.enabled = true;
        isInvincible = false;
    }

    public void Heal(int amount)
    {
        if (isDead) return;
        if (GameManager.Instance != null)
        {
            GameManager.Instance.playerHealth += amount;
            if (GameManager.Instance.playerHealth > GameManager.Instance.maxHealth)
                GameManager.Instance.playerHealth = GameManager.Instance.maxHealth;
        }
        Debug.Log($"JOGADOR curado em {amount}! Vida atual: {CurrentHealth}");
    }

    public void StartInvincibility()
    {
        StartCoroutine(InvincibilityCoroutine());
    }

    // Fallback para garantir que o respawn aconteça mesmo se o fade falhar
    private System.Collections.IEnumerator FallbackRespawn()
    {
        yield return new WaitForSeconds(2f); // Espera 2 segundos
        
        // Se ainda está morto, força o respawn
        if (isDead)
        {
            Debug.LogWarning("ApplyPenaltyAndRespawn: Fallback ativado - forçando respawn");
            float respawnHealth = (GameManager.Instance.playerHealth <= 0) ? GameManager.Instance.maxHealth : GameManager.Instance.playerHealth;
            RespawnAtCheckpoint(respawnHealth);
        }
    }
} 