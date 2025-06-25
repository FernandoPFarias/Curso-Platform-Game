using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HeartBarUIController : MonoBehaviour
{
    public GameObject heartPrefab; // Prefab do coração (UI Image)
    private List<Image> hearts = new List<Image>();

    void Start()
    {
        // Instancia os corações logo ao iniciar o jogo
        if (PlayerController.Instance != null)
        {
            var health = PlayerController.Instance.GetComponent<PlayerHealth>();
            if (health != null)
            {
                int vidaMaxima = Mathf.RoundToInt(health.maxHealth);
                AtualizarCoroes(vidaMaxima);
            }
        }
    }

    void Update()
    {
        if (PlayerController.Instance == null) return;
        var health = PlayerController.Instance.GetComponent<PlayerHealth>();
        if (health == null) return;

        int vidaMaxima = Mathf.FloorToInt(health.maxHealth);
        int vidaAtual = Mathf.FloorToInt(health.CurrentHealth);
        AtualizarCoroes(vidaMaxima);

        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < vidaAtual)
                hearts[i].fillAmount = 1f;
            else
                hearts[i].fillAmount = 0f;
        }
    }

    void AtualizarCoroes(int vidaMaxima)
    {
        // Ajusta a quantidade de corações
        while (hearts.Count < vidaMaxima)
        {
            var heart = Instantiate(heartPrefab, transform).GetComponent<Image>();
            hearts.Add(heart);
        }
        while (hearts.Count > vidaMaxima)
        {
            Destroy(hearts[hearts.Count - 1].gameObject);
            hearts.RemoveAt(hearts.Count - 1);
        }
    }
}