using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public float CurrentHealth => currentHealth;
    private bool isDead = false;
    private bool isInvincible = false;
    public float invincibleTime = 2f; // tempo de invencibilidade após respawn

    private Animator animator;

    private void Start()
    {
        // Inicializa a vida do GameManager, se existir
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.playerHealth > 0)
                currentHealth = GameManager.Instance.playerHealth;
            else
                currentHealth = maxHealth;
        }
        else
        {
            currentHealth = maxHealth;
        }

        animator = GetComponentInChildren<Animator>();
    }

    private void OnDestroy()
    {
        // Salva a vida atual no GameManager ao destruir o player
        if (GameManager.Instance != null)
        {
            GameManager.Instance.playerHealth = currentHealth;
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead || isInvincible) return;
        currentHealth -= damageAmount;
        Debug.Log($"JOGADOR tomou {damageAmount} de dano! Vida atual: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        animator?.SetTrigger("Hurt");
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        if (GameManager.Instance != null && GameManager.Instance.playerLives > 0)
        {
            GameManager.Instance.playerLives--;
            currentHealth = maxHealth; // Vida cheia ao renascer
            RespawnAtCheckpoint(currentHealth);
            return;
        }
        else
        {
            // Game Over
            if (GameManager.Instance != null)
                GameManager.Instance.GameOver();
            animator?.SetTrigger("Death");
            GetComponent<PlayerController>().enabled = false;
            GetComponent<PlayerCombat>().enabled = false;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            GetComponent<Collider2D>().enabled = false;
            Debug.Log("Jogador foi derrotado! Fim de jogo.");
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

        currentHealth = healthAfterPenalty;
        isDead = false;
        GetComponent<PlayerController>().enabled = true;
        GetComponent<PlayerCombat>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
        animator?.SetTrigger("Respawn");
        StartCoroutine(InvincibilityCoroutine());
        StartCoroutine(ReenablePhysicsNextFrame(rb));
        Debug.Log("Player respawned at checkpoint!");

        // Garante que a câmera siga o player após respawn
        if (GameManager.Instance != null)
            GameManager.Instance.SetCameraFollow(transform);
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
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        Debug.Log($"JOGADOR curado em {amount}! Vida atual: {currentHealth}");
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

        // Aplica penalidade de vida, se houver
        float newHealth = Mathf.Max(1f, CurrentHealth - penalty);
        if (GameManager.Instance != null)
            GameManager.Instance.playerHealth = newHealth;

        var fade = FindObjectOfType<FadeController>();
        if (fade != null)
        {
            fade.Fade(
                onBlack: () => {
                    RespawnAtCheckpoint(GameManager.Instance != null ? GameManager.Instance.playerHealth : maxHealth);
                }
            );
        }
        else
        {
            RespawnAtCheckpoint(GameManager.Instance != null ? GameManager.Instance.playerHealth : maxHealth);
        }
    }
} 