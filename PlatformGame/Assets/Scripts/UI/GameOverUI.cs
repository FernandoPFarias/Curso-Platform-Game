using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverPanel;
    public Button restartButton;
    public Button menuButton;

    void Start()
    {
        // Configura os botões se não estiverem configurados
        SetupButtons();
    }

    void SetupButtons()
    {
        // Procura pelos botões no painel de Game Over
        if (gameOverPanel != null)
        {
            // Procura pelo botão de restart
            if (restartButton == null)
            {
                restartButton = gameOverPanel.GetComponentInChildren<Button>();
            }
            
            // Se não encontrou botão de menu, cria um
            if (menuButton == null)
            {
                CreateMenuButton();
            }
        }
    }

    void CreateMenuButton()
    {
        if (gameOverPanel == null) return;

        // Cria um novo botão para o menu
        GameObject menuButtonObj = new GameObject("MenuButton");
        menuButtonObj.transform.SetParent(gameOverPanel.transform, false);
        
        // Adiciona componentes necessários
        RectTransform rectTransform = menuButtonObj.AddComponent<RectTransform>();
        Image image = menuButtonObj.AddComponent<Image>();
        Button button = menuButtonObj.AddComponent<Button>();
        
        // Configura o RectTransform
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = new Vector2(0, -100); // Posição abaixo do botão restart
        rectTransform.sizeDelta = new Vector2(200, 60);
        
        // Configura a imagem do botão
        image.color = new Color(0.2f, 0.2f, 0.2f, 1f);
        
        // Cria o texto do botão
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(menuButtonObj.transform, false);
        
        RectTransform textRectTransform = textObj.AddComponent<RectTransform>();
        textRectTransform.anchorMin = Vector2.zero;
        textRectTransform.anchorMax = Vector2.one;
        textRectTransform.offsetMin = Vector2.zero;
        textRectTransform.offsetMax = Vector2.zero;
        
        // Tenta usar TextMeshPro primeiro, senão usa Text normal
        try
        {
            TMPro.TextMeshProUGUI textComponent = textObj.AddComponent<TMPro.TextMeshProUGUI>();
            textComponent.text = "MENU";
            textComponent.fontSize = 36;
            textComponent.color = Color.white;
            textComponent.alignment = TMPro.TextAlignmentOptions.Center;
        }
        catch
        {
            // Fallback para Text normal
            UnityEngine.UI.Text textComponent = textObj.AddComponent<UnityEngine.UI.Text>();
            textComponent.text = "MENU";
            textComponent.fontSize = 36;
            textComponent.color = Color.white;
            textComponent.alignment = TextAnchor.MiddleCenter;
        }
        
        // Configura o botão
        button.onClick.AddListener(GoToMainMenu);
        
        menuButton = button;
    }

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
        Debug.Log("GameOverUI: RestartLevel chamado!");
        Time.timeScale = 1f;

        // Usa o método do GameManager para restart da fase atual
        if (GameManager.Instance != null)
        {
            Debug.Log("GameOverUI: Usando GameManager.RestartCurrentLevel()");
            GameManager.Instance.RestartCurrentLevel();
        }
        else
        {
            Debug.LogWarning("GameOverUI: GameManager não encontrado, usando fallback");
            // Fallback caso o GameManager não exista
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }
    }

    public void GoToMainMenu()
    {
        Debug.Log("GameOverUI: GoToMainMenu chamado!");
        Time.timeScale = 1f;
        
        // Usa o SceneTransitionManager se disponível
        if (SceneTransitionManager.Instance != null)
        {
            Debug.Log("GameOverUI: Usando SceneTransitionManager para ir ao menu");
            SceneTransitionManager.Instance.TransitionToScene("MenuScene");
        }
        else
        {
            Debug.Log("GameOverUI: SceneTransitionManager não encontrado, usando carregamento direto");
            SceneManager.LoadScene("MenuScene"); // Carrega a cena do menu principal
        }
    }
} 