using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverPanel;

    public void ShowGameOver()
    {
        Debug.Log("ShowGameOver chamado!");
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        else
            Debug.LogWarning("gameOverPanel está null!");
        Time.timeScale = 0f; // Pausa o jogo
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;

        // Destroi o player antigo se existir
        var player = GameObject.FindWithTag("Player");
        if (player != null)
            Destroy(player);

        SceneManager.LoadScene(0); // Carrega a primeira fase do jogo
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Troque para o nome da sua cena de menu principal
    }
} 