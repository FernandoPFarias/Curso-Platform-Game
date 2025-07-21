using UnityEngine;
using System.Collections;

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

    public void TakeDamage(float damageAmount)
    {
        if (isDead || isInvincible) return;
        if (GameManager.Instance != null)
        {
            GameManager.Instance.playerHealth -= damageAmount;
            if (GameManager.Instance.playerHealth < 0)
                GameManager.Instance.playerHealth = 0;
        }
        Debug.Log($"JOGADOR tomou {damageAmount} de dano! Vida atual: {CurrentHealth}");

        if (CurrentHealth <= 0)
        {
            DieAndRespawn();
            return;
        }

        animator?.SetTrigger("Hurt");
    }

    // Método para penalidade (DeathZone)
    public void ApplyPenaltyAndRespawn(float penalty)
    {
        if (isDead) return;
        isDead = true;
        if (GameManager.Instance != null)
        {
            float newHealth = Mathf.Max(1f, CurrentHealth - penalty);
            GameManager.Instance.playerHealth = newHealth;
            Debug.Log($"Penalidade aplicada: {penalty}. Nova vida: {newHealth}");
        }
        var fade = FindObjectOfType<FadeController>();
        if (fade != null)
        {
            fade.Fade(
                onBlack: () => {
                    RespawnAtCheckpoint(GameManager.Instance.playerHealth);
                }
            );
        }
        else
        {
            RespawnAtCheckpoint(GameManager.Instance.playerHealth);
        }
    }

    // Método para morte (vida zerada)
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

    private void RespawnAtCheckpoint(float healthAfterPenalty)
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic; // Desabilita física
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        float yOffset = GameManager.Instance != null ? GameManager.Instance.lastCheckpointYOffset : 0.3f;
        transform.position = GameManager.Instance.lastCheckpointPosition + Vector3.up * yOffset;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.playerHealth = healthAfterPenalty;
            if (GameManager.Instance.playerHealth > GameManager.Instance.maxHealth)
                GameManager.Instance.playerHealth = GameManager.Instance.maxHealth;
            Debug.Log($"Respawn: Vida atual = {GameManager.Instance.playerHealth}, Vidas = {GameManager.Instance.playerLives}");
            Debug.Log($"Respawn: HeartUIController existe? {GameManager.Instance.heartUIController != null}");
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
            GameManager.Instance.SetCameraFollow(transform);
        if (GameManager.Instance != null)
            GameManager.Instance.ForceUIUpdate();

        // NOVO: Resetar todos os bosses após respawn
        var allEnemies = GameObject.FindObjectsOfType<Enemy>();
        foreach (var enemy in allEnemies)
        {
            if (enemy.EnemyData != null && enemy.EnemyData.tier == EnemyTier.Boss)
            {
                var bossBehaviour = enemy.GetBehaviour() as BossBehaviour;
                if (bossBehaviour != null)
                {
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

    public void HandlePlayerDeath(float penalty = 0f)
    {
        // Desabilita controles
        var playerController = GetComponent<PlayerController>();
        if (playerController != null)
            playerController.enabled = false;

        // Se morreu (vida <= 0), perde uma vida extra e restaura vida cheia
        if (GameManager.Instance != null && CurrentHealth <= 0)
        {
            if (GameManager.Instance.playerLives > 0)
            {
                GameManager.Instance.playerLives--;
                GameManager.Instance.playerHealth = GameManager.Instance.maxHealth;
                Debug.Log($"Player perdeu uma vida! Vidas restantes: {GameManager.Instance.playerLives}");
                Debug.Log($"Vida restaurada para: {GameManager.Instance.playerHealth}");
            }
            else
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
        }
        else if (GameManager.Instance != null && penalty > 0f)
        {
            // Penalidade: reduz vida, mantém valor atual
            float newHealth = Mathf.Max(1f, CurrentHealth - penalty);
            GameManager.Instance.playerHealth = newHealth;
            Debug.Log($"Penalidade aplicada: {penalty}. Nova vida: {newHealth}");
        }
        // Se não for penalidade nem morte, mantém vida atual

        var fade = FindObjectOfType<FadeController>();
        if (fade != null)
        {
            fade.Fade(
                onBlack: () => {
                    // Se morreu, respawn com vida cheia; se penalidade, respawn com vida restante
                    float respawnHealth = (CurrentHealth <= 0) ? GameManager.Instance.maxHealth : GameManager.Instance.playerHealth;
                    RespawnAtCheckpoint(respawnHealth);
                }
            );
        }
        else
        {
            float respawnHealth = (CurrentHealth <= 0) ? GameManager.Instance.maxHealth : GameManager.Instance.playerHealth;
            RespawnAtCheckpoint(respawnHealth);
        }
    }
} 