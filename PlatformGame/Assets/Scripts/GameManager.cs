using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float playerHealth = 100f; // Valor inicial padrão
    public Vector3 lastCheckpointPosition = Vector3.zero;
    public float lastCheckpointYOffset = 0.3f;
    public int playerLives = 3; // Valor inicial de vidas
    // Adicione outros dados persistentes aqui (ex: moedas)

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

    private void Start()
    {
        LoadGame();
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