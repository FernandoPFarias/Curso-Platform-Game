using UnityEngine;
using TMPro;

public class LivesUI : MonoBehaviour
{
    public TextMeshProUGUI livesText; // Arraste o TextMeshProUGUI no Inspector

    void Update()
    {
        if (GameManager.Instance != null)
            livesText.text = "x " + GameManager.Instance.playerLives.ToString();
    }
} 