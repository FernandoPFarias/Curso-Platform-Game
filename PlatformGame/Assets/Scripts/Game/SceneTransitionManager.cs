using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;
    
    [Header("Transition Settings")]
    public bool disablePlayerInputDuringTransition = true;
    public bool pauseGameDuringTransition = true;
    
    private bool isTransitioning = false;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Remover ou corrigir DontDestroyOnLoad
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void TransitionToScene(string sceneName)
    {
        if (isTransitioning)
        {
            Debug.LogWarning("SceneTransitionManager: Transição já em andamento!");
            return;
        }
        
        StartCoroutine(TransitionCoroutine(sceneName));
    }
    
    public void TransitionToScene(int sceneIndex)
    {
        if (isTransitioning)
        {
            Debug.LogWarning("SceneTransitionManager: Transição já em andamento!");
            return;
        }
        
        StartCoroutine(TransitionCoroutine(sceneIndex));
    }
    
    private IEnumerator TransitionCoroutine(string sceneName)
    {
        isTransitioning = true;
        Debug.Log($"SceneTransitionManager: Iniciando transição para {sceneName}");
        
        // Desabilita input do player
        if (disablePlayerInputDuringTransition)
        {
            DisablePlayerInput();
        }
        
        // Pausa o jogo se necessário
        if (pauseGameDuringTransition)
        {
            Time.timeScale = 0f;
        }
        
        // Usa o LoadingScreenManager se disponível
        if (LoadingScreenManager.Instance != null)
        {
            Debug.Log("SceneTransitionManager: Usando LoadingScreenManager");
            yield return StartCoroutine(LoadingScreenTransition(sceneName));
        }
        else
        {
            Debug.LogWarning("SceneTransitionManager: LoadingScreenManager não encontrado, carregando cena diretamente");
            SceneManager.LoadScene(sceneName);
        }
        
        // Restaura o jogo
        if (pauseGameDuringTransition)
        {
            Time.timeScale = 1f;
        }
        
        // Reabilita input do player
        if (disablePlayerInputDuringTransition)
        {
            ReenablePlayerInput();
        }
        
        isTransitioning = false;
        Debug.Log("SceneTransitionManager: Transição concluída!");
    }
    
    private IEnumerator TransitionCoroutine(int sceneIndex)
    {
        isTransitioning = true;
        Debug.Log($"SceneTransitionManager: Iniciando transição para cena {sceneIndex}");
        
        // Desabilita input do player
        if (disablePlayerInputDuringTransition)
        {
            DisablePlayerInput();
        }
        
        // Pausa o jogo se necessário
        if (pauseGameDuringTransition)
        {
            Time.timeScale = 0f;
        }
        
        // Usa o LoadingScreenManager se disponível
        if (LoadingScreenManager.Instance != null)
        {
            Debug.Log("SceneTransitionManager: Usando LoadingScreenManager");
            yield return StartCoroutine(LoadingScreenTransition(sceneIndex));
        }
        else
        {
            Debug.LogWarning("SceneTransitionManager: LoadingScreenManager não encontrado, carregando cena diretamente");
            SceneManager.LoadScene(sceneIndex);
        }
        
        // Restaura o jogo
        if (pauseGameDuringTransition)
        {
            Time.timeScale = 1f;
        }
        
        // Reabilita input do player
        if (disablePlayerInputDuringTransition)
        {
            ReenablePlayerInput();
        }
        
        isTransitioning = false;
        Debug.Log("SceneTransitionManager: Transição concluída!");
    }
    
    private IEnumerator LoadingScreenTransition(string sceneName)
    {
        bool transitionComplete = false;
        
        LoadingScreenManager.Instance.LoadSceneWithLoadingScreen(sceneName, () => {
            transitionComplete = true;
        });
        
        while (!transitionComplete)
        {
            yield return null;
        }
    }
    
    private IEnumerator LoadingScreenTransition(int sceneIndex)
    {
        bool transitionComplete = false;
        
        LoadingScreenManager.Instance.LoadSceneWithLoadingScreen(sceneIndex, () => {
            transitionComplete = true;
        });
        
        while (!transitionComplete)
        {
            yield return null;
        }
    }
    
    private void DisablePlayerInput()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.enabled = false;
                Debug.Log("SceneTransitionManager: Input do player desabilitado");
            }
        }
    }
    
    private void ReenablePlayerInput()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.enabled = true;
                Debug.Log("SceneTransitionManager: Input do player reabilitado");
            }
        }
    }
    
    // Método para transição simples sem loading screen (fallback)
    public void LoadSceneImmediate(string sceneName)
    {
        Debug.Log($"SceneTransitionManager: Carregando cena imediatamente: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }
    
    public void LoadSceneImmediate(int sceneIndex)
    {
        Debug.Log($"SceneTransitionManager: Carregando cena imediatamente: {sceneIndex}");
        SceneManager.LoadScene(sceneIndex);
    }
} 