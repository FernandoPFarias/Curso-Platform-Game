using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Checkpoint Settings")]
    public bool isActivated = false;
    public Transform spawnPoint; // Ponto de spawn quando ativado
    public LayerMask playerLayer = 1; // Layer do player (padrão: Default)
    
    [Header("Campfire")]
    public CampfireController campfireController; // Referência para o controlador da fogueira
    public Collider2D interactionCollider; // Referência para o collider de interação (filho)
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip checkpointSound; // Som de checkpoint ativado
    
    [Header("UI Feedback")]
    public CheckpointUI checkpointUI; // UI de feedback
    
    private bool playerInRange = false;
    
    void Start()
    {
        // Garante que o checkpoint começa desativado
        isActivated = false;
        
        // Se não foi configurado via Inspector, tenta encontrar automaticamente
        if (interactionCollider == null)
        {
            // Procura por collider nos filhos
            interactionCollider = GetComponentInChildren<Collider2D>();
            if (interactionCollider != null)
            {
                Debug.Log("Checkpoint: Collider encontrado automaticamente nos filhos");
            }
            else
            {
                Debug.LogWarning("Checkpoint: Nenhum Collider2D encontrado nos filhos! Configure manualmente via Inspector.");
            }
        }
        
        // Verifica configuração do collider
        CheckColliderConfiguration();
    }
    
    private void CheckColliderConfiguration()
    {
        if (interactionCollider != null)
        {
            Debug.Log($"Checkpoint: Collider configurado - Name: {interactionCollider.name}, IsTrigger: {interactionCollider.isTrigger}, Enabled: {interactionCollider.enabled}");
        }
        else
        {
            Debug.LogWarning("Checkpoint: interactionCollider é null!");
        }
        
        // Verifica se há colliders nos filhos
        var childColliders = GetComponentsInChildren<Collider2D>();
        Debug.Log($"Checkpoint: Total de Collider2D nos filhos: {childColliders.Length}");
        foreach (var col in childColliders)
        {
            Debug.Log($"Checkpoint: Collider filho - Name: {col.name}, IsTrigger: {col.isTrigger}, Enabled: {col.enabled}");
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Checkpoint: OnTriggerEnter2D - Object: {other.name}, Layer: {other.gameObject.layer}, PlayerLayer: {playerLayer}");
        
        // Verifica se é o player usando LayerMask
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            playerInRange = true;
            Debug.Log("Checkpoint: Player entrou no range");
        }
        else
        {
            Debug.Log($"Checkpoint: Objeto {other.name} não é player - Layer: {other.gameObject.layer}, PlayerLayer: {playerLayer}");
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"Checkpoint: OnTriggerExit2D - Object: {other.name}");
        
        // Verifica se é o player usando LayerMask
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            playerInRange = false;
            Debug.Log("Checkpoint: Player saiu do range");
        }
    }
    
    // Método público para ser chamado pelo PlayerInteraction
    public void TryInteract()
    {
        // Verifica manualmente se o player está próximo
        CheckPlayerProximity();
        
        if (playerInRange && !isActivated)
        {
            Debug.Log("Checkpoint: TryInteract chamado, ativando checkpoint!");
            ActivateCheckpoint();
        }
        else
        {
            Debug.Log($"Checkpoint: TryInteract chamado mas não ativou - playerInRange: {playerInRange}, isActivated: {isActivated}");
        }
    }
    
    private void CheckPlayerProximity()
    {
        // Procura o player na cena
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            Debug.Log($"Checkpoint: Player encontrado a {distance} unidades de distância");
            
            // Se o player está próximo (menos de 2 unidades), força playerInRange = true
            if (distance < 2f)
            {
                playerInRange = true;
                Debug.Log("Checkpoint: Player está próximo, forçando playerInRange = true");
            }
        }
        else
        {
            Debug.LogWarning("Checkpoint: Player não encontrado na cena!");
        }
    }
    
    public void ActivateCheckpoint()
    {
        if (isActivated) return;
        
        Debug.Log("Checkpoint: Ativando checkpoint!");
        
        // Ativa o checkpoint
        isActivated = true;
        
        // Salva a posição no GameManager
        if (GameManager.Instance != null)
        {
            Vector3 checkpointPosition = spawnPoint != null ? spawnPoint.position : transform.position;
            GameManager.Instance.lastCheckpointPosition = checkpointPosition;
            GameManager.Instance.lastCheckpointYOffset = 0.3f;
            Debug.Log($"Checkpoint: Spawn point salvo em {checkpointPosition}");
        }
        
        // Acende a fogueira se configurada
        if (campfireController != null)
        {
            campfireController.LightFire();
            Debug.Log("Checkpoint: Fogueira acesa!");
        }
        else
        {
            Debug.LogWarning("Checkpoint: campfireController não configurado!");
        }
        
        // Toca som
        if (audioSource != null && checkpointSound != null)
        {
            audioSource.PlayOneShot(checkpointSound);
        }
        
        // Mostra UI de feedback
        if (checkpointUI != null)
        {
            checkpointUI.ShowCheckpointSaved();
        }
    }
    
    // Método público para verificar se está ativado
    public bool IsActivated()
    {
        return isActivated;
    }
    
    // Método para resetar o checkpoint (útil para testes)
    public void ResetCheckpoint()
    {
        isActivated = false;
        
        // Apaga a fogueira se configurada
        if (campfireController != null)
        {
            campfireController.ExtinguishFire();
        }
    }
}