using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeController : MonoBehaviour
{
    public Image fadeImage; // Arraste o FadePanel aqui
    [Header("Fade Timings (seconds)")]
    public float fadeOutDuration = 0.2f;
    public float blackDuration = 0.1f;
    public float fadeInDuration = 0.2f;

    void Awake()
    {
        if (fadeImage != null)
            fadeImage.gameObject.SetActive(false); // Começa desativado
    }

    public void Fade(System.Action onBlack = null, System.Action onComplete = null)
    {
        StartCoroutine(FadeRoutine(onBlack, onComplete));
    }

    private IEnumerator FadeRoutine(System.Action onBlack, System.Action onComplete)
    {
        // Verifica se o fadeImage ainda existe
        if (fadeImage == null)
        {
            Debug.LogWarning("FadeController: fadeImage é null, abortando fade");
            if (onComplete != null) onComplete();
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
        if (onBlack != null) onBlack();
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
        
        if (onComplete != null) onComplete();
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