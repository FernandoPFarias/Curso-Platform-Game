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
        fadeImage.gameObject.SetActive(true);
        // Fade Out (visível -> preto)
        yield return StartCoroutine(FadeAlpha(0, 1, fadeOutDuration));
        // Tela preta
        if (onBlack != null) onBlack();
        yield return new WaitForSeconds(blackDuration);
        // Fade In (preto -> visível)
        yield return StartCoroutine(FadeAlpha(1, 0, fadeInDuration));
        fadeImage.gameObject.SetActive(false);
        if (onComplete != null) onComplete();
    }

    private IEnumerator FadeAlpha(float from, float to, float duration)
    {
        float elapsed = 0f;
        Color c = fadeImage.color;
        c.a = from;
        fadeImage.color = c;
        fadeImage.raycastTarget = true;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, elapsed / duration);
            c.a = alpha;
            fadeImage.color = c;
            yield return null;
        }
        c.a = to;
        fadeImage.color = c;
        fadeImage.raycastTarget = to > 0.5f;
    }
} 