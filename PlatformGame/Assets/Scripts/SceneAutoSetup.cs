using UnityEngine;

public class SceneAutoSetup : MonoBehaviour
{
    [Header("Prefabs Essenciais")]
    public GameObject gameManagerPrefab;
    public GameObject playerPrefab;
    public GameObject coinManagerPrefab;
    public GameObject uiPrefab;

    void Awake()
    {
        // Garante GameManager
        if (GameManager.Instance == null && gameManagerPrefab != null)
            Instantiate(gameManagerPrefab);

        // Garante Player
        if (FindObjectOfType<PlayerController>() == null && playerPrefab != null)
            Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

        // Garante CoinManager
        if (FindObjectOfType<CoinManager>() == null && coinManagerPrefab != null)
            Instantiate(coinManagerPrefab);

        // Garante UI (MenuUI)
        if (FindObjectOfType<MenuUI>() == null && uiPrefab != null)
            Instantiate(uiPrefab);
    }
} 