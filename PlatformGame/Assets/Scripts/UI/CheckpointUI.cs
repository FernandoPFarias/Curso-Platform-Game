using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CheckpointUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI checkpointText; // Texto "Checkpoint Salvo!"
    public Image iconImage; // Ícone de checkpoint
    public CanvasGroup canvasGroup; // Para fade in/out
    
    [Header("Animation Settings")]
    public float fadeInDuration = 0.5f;
    public float displayDuration = 2f;
    public float fadeOutDuration = 0.5f;
    
    private void Start()
    {
        // Configura estado inicial
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
        
        gameObject.SetActive(false);
    }
    
    public void ShowCheckpointSaved()
    {
        gameObject.SetActive(true);
        StartCoroutine(AnimateCheckpointUI());
    }
    
    private System.Collections.IEnumerator AnimateCheckpointUI()
    {
        // Fade In
        float elapsed = 0f;
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsed / fadeInDuration);
            if (canvasGroup != null)
                canvasGroup.alpha = alpha;
            yield return null;
        }
        
        if (canvasGroup != null)
            canvasGroup.alpha = 1f;
        
        // Aguarda tempo de exibição
        yield return new WaitForSeconds(displayDuration);
        
        // Fade Out
        elapsed = 0f;
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeOutDuration);
            if (canvasGroup != null)
                canvasGroup.alpha = alpha;
            yield return null;
        }
        
        if (canvasGroup != null)
            canvasGroup.alpha = 0f;
        
        gameObject.SetActive(false);
    }
} 