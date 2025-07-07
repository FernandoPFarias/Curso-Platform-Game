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

    public Transform initialPlayerSpawnPoint; // Referenciável via Inspector
    public CinemachineCamera virtualCamera; // Arraste a Cinemachine Camera aqui

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
        lastCheckpointPosition = Vector3.zero;
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

    void Start()
    {
        playerLives = 3;
        playerHealth = 100f;
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
        // Se quiser, zere outros dados aqui futuramente
    }

    public void GameOver()
    {
        Debug.Log("GameOver chamado!");
        // Zera moedas e outros itens futuramente
        if (CoinManager.Instance != null)
            CoinManager.Instance.SetCoins(0);
        playerLives = 3;
        playerHealth = 100f;
        SaveGame();

        // Mostra a tela de Game Over
        var gameOverUI = FindObjectOfType<GameOverUI>();
        Debug.Log("GameOverUI encontrado? " + (gameOverUI != null));
        if (gameOverUI != null)
            gameOverUI.ShowGameOver();
        else
            Debug.LogWarning("GameOverUI não encontrado na cena!");
    }
} 