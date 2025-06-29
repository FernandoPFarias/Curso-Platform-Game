using UnityEngine;
// using UnityEngine.UI;
// using TMPro;

public class CoinUI : MonoBehaviour
{
    public SpriteText spriteText;

    private void OnEnable()
    {
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.OnCoinChanged += UpdateUI;
            UpdateUI(CoinManager.Instance.Coins);
        }
    }

    private void OnDisable()
    {
        if (CoinManager.Instance != null)
            CoinManager.Instance.OnCoinChanged -= UpdateUI;
    }

    private void Start()
    {
        if (CoinManager.Instance != null)
            UpdateUI(CoinManager.Instance.Coins);
    }

    private void UpdateUI(int newTotal)
    {
        if (spriteText != null)
            spriteText.SetText(newTotal.ToString());
    }

    void Update()
    {
        if (CoinManager.Instance != null && spriteText != null)
            spriteText.SetText(CoinManager.Instance.Coins.ToString());
    }
} 