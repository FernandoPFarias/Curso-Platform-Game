using UnityEngine;
// using TMPro;

public class LivesUI : MonoBehaviour
{
    public SpriteText spriteText;

    void Update()
    {
        if (GameManager.Instance != null && spriteText != null)
            spriteText.SetText(GameManager.Instance.playerLives.ToString());
    }
} 