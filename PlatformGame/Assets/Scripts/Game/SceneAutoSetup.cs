using UnityEngine;

public class SceneAutoSetup : MonoBehaviour
{
    [Header("Prefabs Essenciais")]
    public GameObject gameManagerPrefab;
    public GameObject playerPrefab;
    public GameObject coinManagerPrefab;
    public GameObject uiPrefab;
    public GameObject mobileControlsPrefab;
    
    [Header("Spawn Point")]
    public Transform initialPlayerSpawnPoint; // Referência via Inspector para o ponto de spawn inicial

    void Awake()
    {
        // Garante GameManager
        if (GameManager.Instance == null && gameManagerPrefab != null)
        {
            var gmObj = Instantiate(gameManagerPrefab);
            // Configura o ponto de spawn inicial
            var gm = gmObj.GetComponent<GameManager>();
            if (gm != null)
            {
                if (initialPlayerSpawnPoint != null)
                {
                    gm.initialPlayerSpawnPoint = initialPlayerSpawnPoint;
                    Debug.Log($"SceneAutoSetup: initialPlayerSpawnPoint configurado via Inspector: {initialPlayerSpawnPoint.position}");
                }
                else
                {
                    // Fallback: procura o PlayerSpawn na cena
                    var spawn = GameObject.Find("PlayerSpawn");
                    if (spawn != null)
                    {
                        gm.initialPlayerSpawnPoint = spawn.transform;
                        Debug.Log($"SceneAutoSetup: PlayerSpawn encontrado na cena: {spawn.transform.position}");
                    }
                    else
                    {
                        Debug.LogWarning("SceneAutoSetup: Nenhum ponto de spawn encontrado! Configure initialPlayerSpawnPoint via Inspector.");
                    }
                }
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

        // Garante MobileControls (apenas em mobile)
        #if UNITY_ANDROID || UNITY_IOS
        if (FindObjectOfType<MobileTouchArea>() == null && mobileControlsPrefab != null)
            Instantiate(mobileControlsPrefab);
        #endif
    }
} 