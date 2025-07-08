using UnityEngine;
using UnityEngine.UI;

public class UIHealthManager : MonoBehaviour
{
    [Header("UI Components")]
    public HeartBarUIController heartBarController;
    public LivesUI livesUI;
    
    [Header("Debug")]
    public bool enableDebugLogs = true;
    
    private float lastHealth = -1f;
    private int lastLives = -1;
    
    void Start()
    {
        // Encontrar componentes automaticamente se não foram atribuídos
        if (heartBarController == null)
            heartBarController = FindObjectOfType<HeartBarUIController>();
            
        if (livesUI == null)
            livesUI = FindObjectOfType<LivesUI>();
            
        if (enableDebugLogs)
            Debug.Log($"UIHealthManager: HeartBarController = {heartBarController != null}, LivesUI = {livesUI != null}");
    }
    
    void Update()
    {
        if (GameManager.Instance == null) return;
        
        // Verificar mudanças na vida
        if (GameManager.Instance.playerHealth != lastHealth)
        {
            lastHealth = GameManager.Instance.playerHealth;
            if (enableDebugLogs)
                Debug.Log($"UIHealthManager: Vida mudou para {lastHealth}");
        }
        
        // Verificar mudanças nas vidas
        if (GameManager.Instance.playerLives != lastLives)
        {
            lastLives = GameManager.Instance.playerLives;
            if (enableDebugLogs)
                Debug.Log($"UIHealthManager: Vidas mudaram para {lastLives}");
        }
        
        // Verificar se a UI está funcionando
        if (heartBarController == null)
        {
            heartBarController = FindObjectOfType<HeartBarUIController>();
            if (heartBarController != null && enableDebugLogs)
                Debug.Log("UIHealthManager: HeartBarController encontrado automaticamente");
        }
        
        if (livesUI == null)
        {
            livesUI = FindObjectOfType<LivesUI>();
            if (livesUI != null && enableDebugLogs)
                Debug.Log("UIHealthManager: LivesUI encontrado automaticamente");
        }
    }
    
    // Método público para forçar atualização da UI
    public void ForceUpdate()
    {
        if (enableDebugLogs)
            Debug.Log("UIHealthManager: Forçando atualização da UI");
            
        // Força a atualização dos componentes
        if (heartBarController != null)
        {
            // O HeartBarUIController já atualiza automaticamente no Update
            if (enableDebugLogs)
                Debug.Log("UIHealthManager: HeartBarController está ativo");
        }
        
        if (livesUI != null)
        {
            // O LivesUI já atualiza automaticamente no Update
            if (enableDebugLogs)
                Debug.Log("UIHealthManager: LivesUI está ativo");
        }
    }
} 