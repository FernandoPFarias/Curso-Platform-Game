using UnityEngine;
using System;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }
    public int Coins { get; private set; }

    public event Action<int> OnCoinChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        SetCoins(0);
    }

    public void AddCoin(int amount = 1)
    {
        Coins += amount;
        Debug.Log("Total de moedas: " + Coins);
        OnCoinChanged?.Invoke(Coins);
    }

    public void ResetCoins()
    {
        Coins = 0;
        OnCoinChanged?.Invoke(Coins);
    }

    public void SetCoins(int value)
    {
        Coins = value;
        OnCoinChanged?.Invoke(Coins);
    }
} 