using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;
using System.Collections;

public class LoadingScreenManager : MonoBehaviour
{
    public static LoadingScreenManager Instance;
    
    [Header("Loading Screen UI")]
    public Image loadingImage;
    public Canvas loadingCanvas;
    public Sprite[] loadingSprites; // Array de sprites para animação (opcional)
    
    [Header("Loading Settings")]
    public float fadeOutDuration = 0.3f; // Reduzido de 0.5f para 0.3f
    public float fadeInDuration = 0.3f; // Reduzido de 0.5f para 0.3f
    public float loadingImageDuration = 0.5f; // Reduzido de 1.0f para 0.5f
    public float sceneSetupDelay = 0.1f; // Reduzido de 0.2f para 0.1f
    public bool useLoadingAnimation = false;
    public float animationSpeed = 0.5f;
    
    [Header("Advanced Settings")]
    public bool useMinimumLoadingTime = false; // Mudado para false por padrão
    public float minimumLoadingTime = 0.5f; // Reduzido ainda mais
    public bool ultraFastTransition = true; // Nova opção para transições ultra-rápidas
    
    private bool isLoading = false;
    private Coroutine loadingCoroutine;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Garante que o Canvas e a Image sejam configurados corretamente
            if (loadingCanvas != null)
            {
                DontDestroyOnLoad(loadingCanvas.gameObject);
            }
        }
        else
        {
            // Se já existe uma instância, destrói esta
            if (loadingCanvas != null)
            {
                DestroyImmediate(loadingCanvas.gameObject);
            }
            DestroyImmediate(gameObject);
        }
    }
    
    void Start()
    {
        // Verifica se as referências estão configuradas
        if (loadingImage == null)
        {
            Debug.LogError("LoadingScreenManager: loadingImage não está configurado no Inspector!");
        }
        
        if (loadingCanvas == null)
        {
            Debug.LogError("LoadingScreenManager: loadingCanvas não está configurado no Inspector!");
        }
        
        // Inicia com loading invisível
        ResetLoadingImage();
    }
    
    private void ResetLoadingImage()
    {
        if (loadingImage != null)
        {
            Color c = loadingImage.color;
            c.a = 0f;
            loadingImage.color = c;
            loadingImage.gameObject.SetActive(false);
        }
        
        if (loadingCanvas != null)
        {
            loadingCanvas.gameObject.SetActive(false);
        }
    }
    
    public void LoadSceneWithLoadingScreen(string sceneName, System.Action onComplete = null)
    {
        if (isLoading) 
        {
            Debug.LogWarning("LoadingScreenManager: Já está carregando uma cena!");
            return;
        }
        
        // Limpa qualquer estado anterior
        CleanupLoadingScreen();
        
        StartCoroutine(LoadSceneCoroutine(sceneName, onComplete));
    }
    
    public void LoadSceneWithLoadingScreen(int sceneIndex, System.Action onComplete = null)
    {
        if (isLoading) 
        {
            Debug.LogWarning("LoadingScreenManager: Já está carregando uma cena!");
            return;
        }
        
        // Limpa qualquer estado anterior
        CleanupLoadingScreen();
        
        StartCoroutine(LoadSceneCoroutine(sceneIndex, onComplete));
    }
    
    private IEnumerator LoadSceneCoroutine(string sceneName, System.Action onComplete)
    {
        isLoading = true;
        float startTime = Time.time;
        Debug.Log($"LoadingScreenManager: Iniciando carregamento da cena {sceneName}");
        
        // 1. Mostra imagem de loading IMEDIATAMENTE
        ShowLoadingImage();
        
        // 2. Fade Out rápido (tela fica escura) - DURANTE este tempo carregamos a cena
        yield return StartCoroutine(FadeOutAndLoadScene(sceneName));
        
        // 3. Garante tempo mínimo de loading se habilitado (e não for ultra-rápido)
        if (useMinimumLoadingTime && !ultraFastTransition)
        {
            float elapsedTime = Time.time - startTime;
            float remainingTime = minimumLoadingTime - elapsedTime;
            
            if (remainingTime > 0)
            {
                Debug.Log($"LoadingScreenManager: Aguardando tempo mínimo restante: {remainingTime:F2}s");
                yield return new WaitForSeconds(remainingTime);
            }
        }
        
        // 4. Fade In (tela volta transparente)
        yield return StartCoroutine(FadeInCoroutine());
        
        // 5. Esconde imagem de loading
        HideLoadingImage();
        
        isLoading = false;
        Debug.Log("LoadingScreenManager: Carregamento concluído!");
        
        if (onComplete != null) onComplete();
    }
    
    private IEnumerator LoadSceneCoroutine(int sceneIndex, System.Action onComplete)
    {
        isLoading = true;
        float startTime = Time.time;
        Debug.Log($"LoadingScreenManager: Iniciando carregamento da cena {sceneIndex}");
        
        // 1. Mostra imagem de loading IMEDIATAMENTE
        ShowLoadingImage();
        
        // 2. Fade Out rápido (tela fica escura) - DURANTE este tempo carregamos a cena
        yield return StartCoroutine(FadeOutAndLoadScene(sceneIndex));
        
        // 3. Garante tempo mínimo de loading se habilitado (e não for ultra-rápido)
        if (useMinimumLoadingTime && !ultraFastTransition)
        {
            float elapsedTime = Time.time - startTime;
            float remainingTime = minimumLoadingTime - elapsedTime;
            
            if (remainingTime > 0)
            {
                Debug.Log($"LoadingScreenManager: Aguardando tempo mínimo restante: {remainingTime:F2}s");
                yield return new WaitForSeconds(remainingTime);
            }
        }
        
        // 4. Fade In (tela volta transparente)
        yield return StartCoroutine(FadeInCoroutine());
        
        // 5. Esconde imagem de loading
        HideLoadingImage();
        
        isLoading = false;
        Debug.Log("LoadingScreenManager: Carregamento concluído!");
        
        if (onComplete != null) onComplete();
    }
    
    private IEnumerator FadeOutCoroutine()
    {
        Debug.Log("LoadingScreenManager: Fade out iniciado...");
        
        if (loadingImage == null) yield break;
        
        loadingImage.gameObject.SetActive(true);
        
        float elapsed = 0f;
        Color c = loadingImage.color;
        c.a = 0f;
        loadingImage.color = c;
        
        while (elapsed < fadeOutDuration)
        {
            if (loadingImage == null) break;
            
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsed / fadeOutDuration);
            c.a = alpha;
            loadingImage.color = c;
            yield return null;
        }
        
        if (loadingImage != null)
        {
            c.a = 1f;
            loadingImage.color = c;
        }
    }
    
    private IEnumerator FadeInCoroutine()
    {
        Debug.Log("LoadingScreenManager: Fade in iniciado...");
        
        if (loadingImage == null) yield break;
        
        float elapsed = 0f;
        Color c = loadingImage.color;
        c.a = 1f;
        loadingImage.color = c;
        
        while (elapsed < fadeInDuration)
        {
            if (loadingImage == null) break;
            
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeInDuration);
            c.a = alpha;
            loadingImage.color = c;
            yield return null;
        }
        
        if (loadingImage != null)
        {
            c.a = 0f;
            loadingImage.color = c;
            loadingImage.gameObject.SetActive(false);
        }
    }
    
    private void ShowLoadingImage()
    {
        Debug.Log("LoadingScreenManager: Mostrando imagem de loading...");
        
        if (loadingImage != null)
        {
            // Garante que o Canvas está ativo
            if (loadingCanvas != null)
            {
                loadingCanvas.gameObject.SetActive(true);
                Debug.Log("LoadingScreenManager: Canvas de loading ativado");
            }
            
            // Ativa a imagem e garante que ela seja visível
            loadingImage.gameObject.SetActive(true);
            
            // Garante que a imagem tenha alpha total
            Color c = loadingImage.color;
            c.a = 1f;
            loadingImage.color = c;
            
            Debug.Log("LoadingScreenManager: Imagem de loading ativada e visível");
            
            if (useLoadingAnimation && loadingSprites != null && loadingSprites.Length > 0)
            {
                StartCoroutine(LoadingAnimationCoroutine());
            }
        }
        else
        {
            Debug.LogError("LoadingScreenManager: loadingImage é null!");
        }
    }
    
    private void HideLoadingImage()
    {
        Debug.Log("LoadingScreenManager: Escondendo imagem de loading...");
        
        if (loadingImage != null)
        {
            loadingImage.gameObject.SetActive(false);
        }
        
        if (loadingCanvas != null)
        {
            loadingCanvas.gameObject.SetActive(false);
        }
    }
    
    private void CleanupLoadingScreen()
    {
        Debug.Log("LoadingScreenManager: Limpando loading screen...");
        
        // Para todas as coroutines de loading
        if (loadingCoroutine != null)
        {
            StopCoroutine(loadingCoroutine);
            loadingCoroutine = null;
        }
        
        // Reseta a imagem de loading
        ResetLoadingImage();
        
        // Reseta o estado
        isLoading = false;
    }
    
    private IEnumerator LoadingAnimationCoroutine()
    {
        int currentSpriteIndex = 0;
        
        while (isLoading && loadingImage != null && loadingSprites != null)
        {
            if (loadingSprites.Length > 0)
            {
                loadingImage.sprite = loadingSprites[currentSpriteIndex];
                currentSpriteIndex = (currentSpriteIndex + 1) % loadingSprites.Length;
            }
            
            yield return new WaitForSeconds(animationSpeed);
        }
    }
    
    // Método para verificar se está configurado corretamente
    public bool IsProperlyConfigured()
    {
        bool isConfigured = true;
        
        if (loadingImage == null)
        {
            Debug.LogError("LoadingScreenManager: loadingImage não está configurado!");
            isConfigured = false;
        }
        
        if (loadingCanvas == null)
        {
            Debug.LogError("LoadingScreenManager: loadingCanvas não está configurado!");
            isConfigured = false;
        }
        
        if (isConfigured)
        {
            Debug.Log("LoadingScreenManager: Configurado corretamente!");
        }
        
        return isConfigured;
    }
    
    // Método para configurar manualmente via Inspector
    [ContextMenu("Check Configuration")]
    public void CheckConfiguration()
    {
        IsProperlyConfigured();
    }
    
    private void ForceCameraFinalCheck()
    {
        Debug.Log("LoadingScreenManager: Verificação final da câmera...");
        
        var virtualCamera = FindObjectOfType<CinemachineCamera>();
        var player = GameObject.FindGameObjectWithTag("Player");
        
        if (virtualCamera != null && player != null)
        {
            // Garante que a câmera está seguindo o player
            if (virtualCamera.Follow != player.transform)
            {
                Debug.Log("LoadingScreenManager: Corrigindo follow target da câmera");
                virtualCamera.Follow = player.transform;
                virtualCamera.LookAt = player.transform;
            }
            
            // Garante que a câmera principal está na posição correta
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                Vector3 targetPosition = player.transform.position;
                targetPosition.z = mainCamera.transform.position.z;
                
                if (Vector3.Distance(mainCamera.transform.position, targetPosition) > 0.1f)
                {
                    Debug.Log("LoadingScreenManager: Corrigindo posição da câmera principal");
                    mainCamera.transform.position = targetPosition;
                }
            }
        }
    }
    
    private void SetupCameraInNewScene()
    {
        Debug.Log("LoadingScreenManager: Configurando câmera na nova cena...");
        
        // Primeiro, procura por um CameraController na cena
        var cameraController = FindObjectOfType<CameraController>();
        if (cameraController != null)
        {
            Debug.Log("LoadingScreenManager: CameraController encontrado, usando ele para configurar a câmera");
            cameraController.SetupCamera();
            return;
        }
        
        // Se não encontrar CameraController, usa o método antigo
        Debug.Log("LoadingScreenManager: CameraController não encontrado, usando método manual");
        
        // Procura pela câmera virtual na nova cena
        var virtualCamera = FindObjectOfType<CinemachineCamera>();
        if (virtualCamera != null)
        {
            Debug.Log("LoadingScreenManager: CinemachineCamera encontrada");
            
            // Procura pelo player
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Debug.Log("LoadingScreenManager: Player encontrado, configurando follow target");
                
                // Desabilita temporariamente a câmera virtual para evitar conflitos
                virtualCamera.enabled = false;
                
                // Configura a câmera para seguir o player
                virtualCamera.Follow = player.transform;
                virtualCamera.LookAt = player.transform;
                
                // Move a câmera principal para a posição do player
                Camera mainCamera = Camera.main;
                if (mainCamera != null)
                {
                    Vector3 targetPosition = player.transform.position;
                    targetPosition.z = mainCamera.transform.position.z; // Mantém o Z da câmera
                    mainCamera.transform.position = targetPosition;
                    
                    Debug.Log($"LoadingScreenManager: Câmera principal movida para {targetPosition}");
                }
                
                // Reabilita a câmera virtual após um frame
                StartCoroutine(ReenableCameraAfterFrame(virtualCamera));
            }
            else
            {
                Debug.LogWarning("LoadingScreenManager: Player não encontrado para configurar câmera");
            }
        }
        else
        {
            Debug.LogWarning("LoadingScreenManager: CinemachineCamera não encontrada na nova cena");
        }
    }
    
    private IEnumerator ReenableCameraAfterFrame(CinemachineCamera virtualCamera)
    {
        // Espera um frame para garantir que tudo foi configurado
        yield return null;
        
        if (virtualCamera != null)
        {
            virtualCamera.enabled = true;
            Debug.Log("LoadingScreenManager: Câmera virtual reabilitada!");
        }
    }
    
    private void SetupPlayerInNewScene()
    {
        Debug.Log("LoadingScreenManager: Configurando player na nova cena...");
        
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // Reabilita o input do player
                playerController.enabled = true;
                Debug.Log("LoadingScreenManager: Input do player reabilitado");
                
                // Comentado temporariamente para evitar erros de compilação
                // if (playerController.GetType().GetMethod("ForceGroundCheck") != null)
                // {
                //     playerController.ForceGroundCheck();
                // }
                
                // if (playerController.GetType().GetMethod("OnCameraReconnected") != null)
                // {
                //     playerController.OnCameraReconnected();
                // }
            }
        }
        else
        {
            Debug.LogWarning("LoadingScreenManager: Player não encontrado na nova cena");
        }
    }

    private IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        Debug.Log("LoadingScreenManager: Fade out iniciado com carregamento da cena...");
        
        // Verifica se a imagem ainda existe
        if (loadingImage == null)
        {
            Debug.LogError("LoadingScreenManager: loadingImage é null durante fade out!");
            yield break;
        }
        
        // Garante que a imagem está ativa e configurada
        loadingImage.gameObject.SetActive(true);
        
        // Inicia o carregamento da cena IMEDIATAMENTE
        Debug.Log("LoadingScreenManager: Iniciando carregamento assíncrono da cena...");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;
        
        float elapsed = 0f;
        Color c = loadingImage.color;
        c.a = 0f;
        loadingImage.color = c;
        
        // Fade out mais rápido enquanto carrega a cena
        while (elapsed < fadeOutDuration)
        {
            // Verifica se a imagem ainda existe a cada frame
            if (loadingImage == null)
            {
                Debug.LogWarning("LoadingScreenManager: loadingImage foi destruída durante fade out!");
                break;
            }
            
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsed / fadeOutDuration);
            c.a = alpha;
            loadingImage.color = c;
            
            // Mostra progresso do carregamento
            if (asyncLoad.progress < 0.9f)
            {
                Debug.Log($"LoadingScreenManager: Progresso do carregamento: {asyncLoad.progress * 100:F1}%");
            }
            
            yield return null;
        }
        
        // Garante que o fade out terminou
        if (loadingImage != null)
        {
            c.a = 1f;
            loadingImage.color = c;
        }
        
        // Aguarda o carregamento da cena terminar (se ainda não terminou)
        while (asyncLoad.progress < 0.9f)
        {
            Debug.Log($"LoadingScreenManager: Aguardando carregamento: {asyncLoad.progress * 100:F1}%");
            yield return null;
        }
        
        // Ativa a cena
        Debug.Log("LoadingScreenManager: Ativando nova cena...");
        asyncLoad.allowSceneActivation = true;
        
        // Aguarda a cena ser ativada
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
        // Configura a nova cena imediatamente
        Debug.Log("LoadingScreenManager: Configurando nova cena...");
        SetupCameraInNewScene();
        SetupPlayerInNewScene();
        
        // Aguarda um pouco para garantir que tudo foi configurado
        yield return new WaitForSeconds(sceneSetupDelay);
        
        // Força verificação final da câmera
        ForceCameraFinalCheck();
        
        Debug.Log("LoadingScreenManager: Cena carregada e configurada durante fade out!");
    }
    
    private IEnumerator FadeOutAndLoadScene(int sceneIndex)
    {
        Debug.Log("LoadingScreenManager: Fade out iniciado com carregamento da cena...");
        
        // Verifica se a imagem ainda existe
        if (loadingImage == null)
        {
            Debug.LogError("LoadingScreenManager: loadingImage é null durante fade out!");
            yield break;
        }
        
        // Garante que a imagem está ativa e configurada
        loadingImage.gameObject.SetActive(true);
        
        // Inicia o carregamento da cena IMEDIATAMENTE
        Debug.Log("LoadingScreenManager: Iniciando carregamento assíncrono da cena...");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        asyncLoad.allowSceneActivation = false;
        
        float elapsed = 0f;
        Color c = loadingImage.color;
        c.a = 0f;
        loadingImage.color = c;
        
        // Fade out mais rápido enquanto carrega a cena
        while (elapsed < fadeOutDuration)
        {
            // Verifica se a imagem ainda existe a cada frame
            if (loadingImage == null)
            {
                Debug.LogWarning("LoadingScreenManager: loadingImage foi destruída durante fade out!");
                break;
            }
            
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsed / fadeOutDuration);
            c.a = alpha;
            loadingImage.color = c;
            
            // Mostra progresso do carregamento
            if (asyncLoad.progress < 0.9f)
            {
                Debug.Log($"LoadingScreenManager: Progresso do carregamento: {asyncLoad.progress * 100:F1}%");
            }
            
            yield return null;
        }
        
        // Garante que o fade out terminou
        if (loadingImage != null)
        {
            c.a = 1f;
            loadingImage.color = c;
        }
        
        // Aguarda o carregamento da cena terminar (se ainda não terminou)
        while (asyncLoad.progress < 0.9f)
        {
            Debug.Log($"LoadingScreenManager: Aguardando carregamento: {asyncLoad.progress * 100:F1}%");
            yield return null;
        }
        
        // Ativa a cena
        Debug.Log("LoadingScreenManager: Ativando nova cena...");
        asyncLoad.allowSceneActivation = true;
        
        // Aguarda a cena ser ativada
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
        // Configura a nova cena imediatamente
        Debug.Log("LoadingScreenManager: Configurando nova cena...");
        SetupCameraInNewScene();
        SetupPlayerInNewScene();
        
        // Aguarda um pouco para garantir que tudo foi configurado
        yield return new WaitForSeconds(sceneSetupDelay);
        
        // Força verificação final da câmera
        ForceCameraFinalCheck();
        
        Debug.Log("LoadingScreenManager: Cena carregada e configurada durante fade out!");
    }
} 