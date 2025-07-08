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

        // Só ativa a lógica de vidas extras se a vida chegou a zero por dano
        if (CurrentHealth <= 0)
        {
            Die();
            return;
        }

        animator?.SetTrigger("Hurt");
    }
    
    // Método separado para respawn da DeathZone (não perde vida extra)
    public void RespawnFromDeathZone()
    {
        if (isDead) return;
        isDead = true;
        
        Debug.Log("Respawn da DeathZone - não perde vida extra");
        
        // Respawn no checkpoint sem perder vida extra
        HandlePlayerDeath(0); // Sem penalidade
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        // Verifica se ainda tem vidas extras
        if (GameManager.Instance != null && GameManager.Instance.playerLives > 0)
        {
            // Perde uma vida extra
            GameManager.Instance.playerLives--;
            
            // Restaura a vida atual para 100
            GameManager.Instance.playerHealth = GameManager.Instance.maxHealth;
            
            Debug.Log($"Player perdeu uma vida! Vidas restantes: {GameManager.Instance.playerLives}");
            Debug.Log($"Vida restaurada para: {GameManager.Instance.playerHealth}");
            
            // Força atualização da UI
            if (GameManager.Instance.heartUIController != null)
                GameManager.Instance.ForceUIUpdate();
            
            // Respawn no checkpoint
            HandlePlayerDeath(0); // Sem penalidade, pois a vida já foi restaurada
            return;
        }
        else
        {
            // Game Over - não tem mais vidas extras
            if (GameManager.Instance != null)
                GameManager.Instance.GameOver();
            animator?.SetTrigger("Death");
            GetComponent<PlayerController>().enabled = false;
            GetComponent<PlayerCombat>().enabled = false;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            GetComponent<Collider2D>().enabled = false;
            Debug.Log("Jogador foi derrotado! Fim de jogo - Sem mais vidas extras.");
            // Aqui você pode chamar a tela de Game Over futuramente
        }
    }

    private void RespawnAtCheckpoint(float healthAfterPenalty)
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic; // Desabilita física
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // Usa o offset salvo no GameManager para altura do respawn
        float yOffset = GameManager.Instance != null ? GameManager.Instance.lastCheckpointYOffset : 0.3f;
        transform.position = GameManager.Instance.lastCheckpointPosition + Vector3.up * yOffset;

        if (GameManager.Instance != null && healthAfterPenalty > 0)
        {
            GameManager.Instance.playerHealth = healthAfterPenalty;
            if (GameManager.Instance.playerHealth > GameManager.Instance.maxHealth)
                GameManager.Instance.playerHealth = GameManager.Instance.maxHealth;
            
            // Debug: Verificar se a UI está sendo atualizada
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
            
        // Força atualização da UI após respawn
        if (GameManager.Instance != null)
            GameManager.Instance.ForceUIUpdate();
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

        // Aplica penalidade de vida apenas se não for perda de vida extra
        // (quando perde vida extra, a vida já foi restaurada para 100 no método Die())
        if (GameManager.Instance != null && penalty > 0f)
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
                    // Se perdeu vida extra, respawn com vida cheia (100)
                    // Se foi penalidade, respawn com a vida após penalidade
                    // Se foi DeathZone, mantém a vida atual
                    float respawnHealth;
                    if (penalty > 0f)
                    {
                        respawnHealth = GameManager.Instance.playerHealth;
                    }
                    else if (isDead && GameManager.Instance.playerHealth <= 0)
                    {
                        // Perdeu vida extra - respawn com vida cheia
                        respawnHealth = GameManager.Instance.maxHealth;
                    }
                    else
                    {
                        // DeathZone ou outro respawn - mantém vida atual
                        respawnHealth = GameManager.Instance.playerHealth;
                    }
                    RespawnAtCheckpoint(respawnHealth);
                }
            );
        }
        else
        {
            float respawnHealth;
            if (penalty > 0f)
            {
                respawnHealth = GameManager.Instance.playerHealth;
            }
            else if (isDead && GameManager.Instance.playerHealth <= 0)
            {
                // Perdeu vida extra - respawn com vida cheia
                respawnHealth = GameManager.Instance.maxHealth;
            }
            else
            {
                // DeathZone ou outro respawn - mantém vida atual
                respawnHealth = GameManager.Instance.playerHealth;
            }
            RespawnAtCheckpoint(respawnHealth);
        }
    }
} 