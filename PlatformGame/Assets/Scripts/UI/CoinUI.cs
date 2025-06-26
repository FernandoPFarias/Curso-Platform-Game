using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;

    private void OnEnable()
    {
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.OnCoinChanged += UpdateUI;
            UpdateUI(CoinManager.Instance.Coins); // Atualiza ao habilitar
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
        Debug.Log("UpdateUI chamado! Novo valor: " + newTotal + " | coinText null? " + (coinText == null));
        if (coinText != null)
            coinText.text = "x " + newTotal.ToString();
    }

    void Update()
    {
        if (CoinManager.Instance != null && coinText != null)
            coinText.text = "x " + CoinManager.Instance.Coins.ToString();
    }
} 