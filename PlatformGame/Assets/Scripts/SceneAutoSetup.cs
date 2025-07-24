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
        {
            var gmObj = Instantiate(gameManagerPrefab);
            // Procura o PlayerSpawn na cena e seta no GameManager
            var spawn = GameObject.Find("PlayerSpawn");
            if (spawn != null)
            {
                var gm = gmObj.GetComponent<GameManager>();
                if (gm != null)
                    gm.initialPlayerSpawnPoint = spawn.transform;
            }
        }

        // Garante Player
        if (FindObjectOfType<PlayerController>() == null && playerPrefab != null)
        {
            var playerObj = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            playerObj.tag = "Player"; // <-- Garante a tag correta!
        }

        // Garante CoinManager
        if (FindObjectOfType<CoinManager>() == null && coinManagerPrefab != null)
            Instantiate(coinManagerPrefab);

        // Garante UI (MenuUI)
        if (FindObjectOfType<MenuUI>() == null && uiPrefab != null)
            Instantiate(uiPrefab);
    }
} 