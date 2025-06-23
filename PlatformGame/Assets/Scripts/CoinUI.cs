using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;

    private void OnEnable()
    {
        CoinManager.Instance.OnCoinChanged += UpdateUI;
        UpdateUI(CoinManager.Instance.Coins); // Atualiza ao habilitar
    }

    private void OnDisable()
    {
        if (CoinManager.Instance != null)
            CoinManager.Instance.OnCoinChanged -= UpdateUI;
    }

    private void Start()
    {
        UpdateUI(CoinManager.Instance.Coins);
    }

    private void UpdateUI(int newTotal)
    {
        Debug.Log("UpdateUI chamado! Novo valor: " + newTotal);
        if (coinText != null)
            coinText.text = newTotal.ToString();
    }
} 