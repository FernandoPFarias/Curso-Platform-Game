using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Controla o efeito de fade in/out na tela para transições suaves
public class FadeController : MonoBehaviour
{
    public Image fadeImage; // Imagem usada para o fade
    [Header("Fade Timings (seconds)")]
    public float fadeOutDuration = 0.2f;
    public float blackDuration = 0.1f;
    public float fadeInDuration = 0.2f;

    void Awake()
    {
        // Se fadeImage não foi configurado, tenta criar ou encontrar um
        if (fadeImage == null)
        {
            SetupFadeImage();
        }
        
        if (fadeImage != null)
            fadeImage.gameObject.SetActive(false); // Começa desativado
    }
    
    // Configura automaticamente o fadeImage se não estiver configurado
    private void SetupFadeImage()
    {
        // Primeiro, procura por uma imagem de fade existente na cena
        var existingFadeImage = FindObjectOfType<Image>();
        if (existingFadeImage != null && existingFadeImage.gameObject.name.Contains("Fade"))
        {
            fadeImage = existingFadeImage;
            Debug.Log("FadeController: fadeImage encontrado automaticamente na cena");
            return;
        }
        
        // Se não encontrar, procura por um Canvas
        var canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            // Cria uma nova imagem de fade
            GameObject fadeObj = new GameObject("FadeImage");
            fadeObj.transform.SetParent(canvas.transform, false);
            
            // Configura o RectTransform para cobrir toda a tela
            var rectTransform = fadeObj.AddComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            
            // Adiciona a Image
            fadeImage = fadeObj.AddComponent<Image>();
            fadeImage.color = Color.black;
            fadeImage.raycastTarget = false;
            
            // Garante que fique por cima de tudo
            var canvasGroup = fadeObj.AddComponent<CanvasGroup>();
            canvasGroup.blocksRaycasts = false;
            
            Debug.Log("FadeController: fadeImage criado automaticamente");
        }
        else
        {
            Debug.LogWarning("FadeController: Nenhum Canvas encontrado para criar fadeImage");
        }
    }

    // Inicia o efeito de fade
    public void Fade(System.Action onBlack = null, System.Action onComplete = null)
    {
        // Garante que o fadeImage está configurado
        if (fadeImage == null)
        {
            SetupFadeImage();
        }
        
        StartCoroutine(FadeRoutine(onBlack, onComplete));
    }

    private IEnumerator FadeRoutine(System.Action onBlack, System.Action onComplete)
    {
        // Verifica se o fadeImage ainda existe
        if (fadeImage == null)
        {
            Debug.LogWarning("FadeController: fadeImage é null, executando callback onBlack diretamente");
            if (onBlack != null) 
            {
                Debug.Log("FadeController: Executando callback onBlack (sem fade)");
                onBlack();
            }
            if (onComplete != null) 
            {
                Debug.Log("FadeController: Executando callback onComplete (sem fade)");
                onComplete();
            }
            yield break;
        }

        fadeImage.gameObject.SetActive(true);
        
        // Fade Out (visível -> preto)
        yield return StartCoroutine(FadeAlpha(0, 1, fadeOutDuration));
        
        // Verifica novamente se ainda existe antes de continuar
        if (fadeImage == null)
        {
            Debug.LogWarning("FadeController: fadeImage foi destruído durante fade out");
            if (onComplete != null) onComplete();
            yield break;
        }
        
        // Tela preta
        if (onBlack != null) 
        {
            Debug.Log("FadeController: Executando callback onBlack");
            onBlack();
        }
        yield return new WaitForSeconds(blackDuration);
        
        // Verifica se ainda existe antes do fade in
        if (fadeImage == null)
        {
            Debug.LogWarning("FadeController: fadeImage foi destruído durante black duration");
            if (onComplete != null) onComplete();
            yield break;
        }
        
        // Fade In (preto -> visível)
        yield return StartCoroutine(FadeAlpha(1, 0, fadeInDuration));
        
        // Verifica uma última vez antes de finalizar
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(false);
        }
        
        if (onComplete != null) 
        {
            Debug.Log("FadeController: Executando callback onComplete");
            onComplete();
        }
    }

    private IEnumerator FadeAlpha(float from, float to, float duration)
    {
        // Verifica se o fadeImage existe
        if (fadeImage == null)
        {
            Debug.LogWarning("FadeController: fadeImage é null em FadeAlpha");
            yield break;
        }

        float elapsed = 0f;
        Color c = fadeImage.color;
        c.a = from;
        fadeImage.color = c;
        fadeImage.raycastTarget = true;
        
        while (elapsed < duration)
        {
            // Verifica se o fadeImage ainda existe a cada frame
            if (fadeImage == null)
            {
                Debug.LogWarning("FadeController: fadeImage foi destruído durante FadeAlpha");
                yield break;
            }
            
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, elapsed / duration);
            c.a = alpha;
            fadeImage.color = c;
            yield return null;
        }
        
        // Verifica uma última vez antes de finalizar
        if (fadeImage != null)
        {
            c.a = to;
            fadeImage.color = c;
            fadeImage.raycastTarget = to > 0.5f;
        }
    }
} 