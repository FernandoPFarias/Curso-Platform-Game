using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public void StartGame()
    {
        // Zera tudo antes de começar o jogo
        if (GameManager.Instance != null)
        {
            GameManager.Instance.playerLives = 3;
            GameManager.Instance.playerHealth = 100f;
            GameManager.Instance.lastCheckpointPosition = Vector3.zero;
            GameManager.Instance.lastCheckpointYOffset = 0.3f;
            GameManager.Instance.SaveGame();
        }
        if (CoinManager.Instance != null)
            CoinManager.Instance.SetCoins(0);

        SceneManager.LoadScene(1); // Carrega a primeira fase (Scenes/lvl_1)
    }
} 