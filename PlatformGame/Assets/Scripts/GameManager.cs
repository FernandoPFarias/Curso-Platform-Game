using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

[DefaultExecutionOrder(-1000)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject playerPrefab;
    public GameObject coinManagerPrefab;
    public GameObject uiPrefab;

    // Referências para instâncias
    private GameObject playerInstance;
    private GameObject coinManagerInstance;
    private GameObject uiInstance;

    public float playerHealth = 100f;
    public Vector3 lastCheckpointPosition = Vector3.zero;
    public float lastCheckpointYOffset = 0.3f;
    public int playerLives = 3;
    public float maxHealth = 100f; // Vida máxima centralizada

    public Transform initialPlayerSpawnPoint; // Referenciável via Inspector
    public CinemachineCamera virtualCamera; // Arraste a Cinemachine Camera aqui
    public HeartBarUIController heartUIController; // Referencie a UI de vida aqui

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"GameManager: Nova cena carregada: {scene.name}");
        lastCheckpointPosition = Vector3.zero;
        
        // Reconecta a câmera ao player após a mudança de cena
        StartCoroutine(ReconnectCameraAfterSceneLoad());
    }
    
    private System.Collections.IEnumerator ReconnectCameraAfterSceneLoad()
    {
        // Espera um frame para garantir que tudo foi carregado
        yield return null;
        
        Debug.Log("GameManager: Tentando reconectar câmera ao player...");
        
        // Primeiro, tenta usar o CameraManager se disponível
        var cameraManager = FindObjectOfType<CameraManager>();
        if (cameraManager != null)
        {
            Debug.Log("GameManager: CameraManager encontrado, usando para reconectar câmera");
            cameraManager.ReconnectCamera();
        }
        else
        {
            Debug.Log("GameManager: CameraManager não encontrado, usando método manual");
            
            // Procura pela câmera virtual na nova cena
            if (virtualCamera == null)
            {
                virtualCamera = FindObjectOfType<CinemachineCamera>();
                Debug.Log($"GameManager: CinemachineCamera encontrada? {virtualCamera != null}");
            }
            
            // Procura pelo player (que pode ter DontDestroyOnLoad)
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Debug.Log($"GameManager: Player encontrado: {player.name}");
                SetCameraFollow(player.transform);
                
                // Notifica o player que a câmera foi reconectada
                var playerController = player.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.OnCameraReconnected();
                }
            }
            else
            {
                Debug.LogWarning("GameManager: Player não encontrado na nova cena!");
            }
        }
    }

    private void OnSceneUnloaded(Scene scene)
    {
        SaveGame();
    }

    void OnApplicationQuit()
    {
        SaveGame();
    }

    public void SaveGame()
    {
        PlayerPrefs.SetInt("Moedas", CoinManager.Instance != null ? CoinManager.Instance.Coins : 0);
        PlayerPrefs.SetFloat("Vida", playerHealth);
        PlayerPrefs.SetInt("Vidas", playerLives);
        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("Moedas") && CoinManager.Instance != null)
            CoinManager.Instance.SetCoins(PlayerPrefs.GetInt("Moedas"));
        if (PlayerPrefs.HasKey("Vida"))
            playerHealth = PlayerPrefs.GetFloat("Vida");
        if (PlayerPrefs.HasKey("Vidas"))
            playerLives = PlayerPrefs.GetInt("Vidas");
    }

    public void SetCameraFollow(Transform playerTransform)
    {
        if (virtualCamera != null)
            virtualCamera.Follow = playerTransform;
    }
    
    // Método para forçar atualização da UI
    public void ForceUIUpdate()
    {
        Debug.Log("GameManager: Forçando atualização da UI...");
        
        // Atualiza a UI de vida usando o HeartBarUIController
        if (heartUIController != null)
        {
            // O HeartBarUIController atualiza automaticamente no Update, mas podemos forçar uma atualização
            Debug.Log("GameManager: UI de vida encontrada e funcionando");
        }
        else
        {
            Debug.LogWarning("GameManager: HeartUIController não encontrado! Verifique a referência no Inspector.");
        }
        
        // Atualiza a UI de moedas - o CoinManager usa eventos para atualizar automaticamente
        if (CoinManager.Instance != null)
        {
            // Força uma atualização chamando SetCoins com o valor atual
            CoinManager.Instance.SetCoins(CoinManager.Instance.Coins);
            Debug.Log("GameManager: UI de moedas atualizada");
        }
        else
        {
            Debug.LogWarning("GameManager: CoinManager não encontrado");
        }
    }

    // Método chamado após a transição de cena ser concluída
    public void OnSceneLoadedAfterTransition()
    {
        Debug.Log("GameManager: Cena carregada após transição, atualizando UI...");
        
        // Força atualização da UI
        ForceUIUpdate();
        
        // Reconecta a câmera se necessário
        StartCoroutine(ReconnectCameraAfterSceneLoad());
    }
    
    void Start()
    {
        // Inicializa o sistema de vidas extras
        playerLives = 3;
        playerHealth = maxHealth; // Começa com vida cheia
        lastCheckpointPosition = Vector3.zero;
        lastCheckpointYOffset = 0.3f;
        
        // Se não houver checkpoint salvo, usa o ponto de spawn inicial
        if (initialPlayerSpawnPoint != null)
        {
            lastCheckpointPosition = initialPlayerSpawnPoint.position;
        }
        
        // Atualiza o Follow da câmera para o player na cena
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            SetCameraFollow(player.transform);
            
        Debug.Log($"GameManager inicializado: Vidas = {playerLives}, Vida = {playerHealth}");
    }
    
    // Método para adicionar vidas extras
    public void AddExtraLife(int amount = 1)
    {
        playerLives += amount;
        Debug.Log($"Vida extra adicionada! Total de vidas: {playerLives}");
        ForceUIUpdate();
    }
    
    // Método para remover vidas extras
    public void RemoveExtraLife(int amount = 1)
    {
        playerLives = Mathf.Max(0, playerLives - amount);
        Debug.Log($"Vida extra removida! Vidas restantes: {playerLives}");
        ForceUIUpdate();
    }
    
    // Método para restaurar vida ao máximo
    public void RestoreFullHealth()
    {
        playerHealth = maxHealth;
        Debug.Log($"Vida restaurada ao máximo: {playerHealth}");
        ForceUIUpdate();
    }

    public void GameOver()
    {
        Debug.Log("GameOver chamado!");
        
        // Mostra a tela de Game Over
        var gameOverUI = FindObjectOfType<GameOverUI>();
        Debug.Log("GameOverUI encontrado? " + (gameOverUI != null));
        if (gameOverUI != null)
            gameOverUI.ShowGameOver();
        else
            Debug.LogWarning("GameOverUI não encontrado na cena!");
    }
    
    // Método para restart da fase atual
    public void RestartCurrentLevel()
    {
        Debug.Log("GameManager: Restart da fase atual!");
        Debug.Log($"GameManager: Cena atual = {SceneManager.GetActiveScene().name} (índice {SceneManager.GetActiveScene().buildIndex})");
        
        // Reseta o estado do jogo para a fase atual
        playerLives = 3;
        playerHealth = maxHealth;
        lastCheckpointPosition = Vector3.zero;
        lastCheckpointYOffset = 0.3f;
        
        Debug.Log($"GameManager: Estado resetado - Vidas: {playerLives}, Vida: {playerHealth}");
        
        // Zera moedas
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.SetCoins(0);
            Debug.Log("GameManager: Moedas zeradas");
        }
            
        SaveGame();
        
        // Recarrega a cena atual usando SceneTransitionManager se disponível
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log($"GameManager: Recarregando cena {currentSceneIndex}");
        
        if (SceneTransitionManager.Instance != null)
        {
            Debug.Log("GameManager: Usando SceneTransitionManager para restart");
            SceneTransitionManager.Instance.TransitionToScene(currentSceneIndex);
        }
        else
        {
            Debug.Log("GameManager: SceneTransitionManager não encontrado, usando carregamento direto");
            SceneManager.LoadScene(currentSceneIndex);
        }
    }
} 