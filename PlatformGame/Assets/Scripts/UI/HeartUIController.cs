using UnityEngine;
using UnityEngine.UI;

public class HeartUIController : MonoBehaviour
{
    public Image heartFillImage; // arraste o Image do coração aqui no Inspector

    public void UpdateUI()
    {
        if (GameManager.Instance == null) return;
        float fill = Mathf.Clamp01(GameManager.Instance.playerHealth / GameManager.Instance.maxHealth);
        heartFillImage.fillAmount = fill;
    }
} 