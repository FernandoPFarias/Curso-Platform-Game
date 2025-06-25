using UnityEngine;
using UnityEngine.UI;

public class HeartUIController : MonoBehaviour
{
    public Image heartFillImage; // arraste o Image do coração aqui no Inspector
    public int vidaMaxima = 3;   // pode ser lido do GameManager se preferir

    void Update()
    {
        int vidaAtual = GameManager.Instance.vida; // ou PlayerController.Instance.vida
        float fill = Mathf.Clamp01((float)vidaAtual / vidaMaxima);
        heartFillImage.fillAmount = fill;
    }
} 