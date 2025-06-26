using UnityEngine;
using UnityEngine.UI;

public class HeartBarUIController : MonoBehaviour
{
    [Header("Referências das Imagens")]
    public Image heartFillerImage; // Imagem do coração que enche/seca (Image Type: Filled)
    public Image heartFrameImage;  // Imagem do contorno do coração

    [Header("Configurações da Animação")]
    public float lowHealthThreshold = 0.3f; // Vida considerada baixa
    public float pulseSpeed = 8f;           // Velocidade do pulsar
    public float pulseAmount = 0.1f;        // Intensidade do pulsar
    public float blinkSpeed = 2f;           // Velocidade do piscar

    void Update()
    {
        if (PlayerController.Instance == null) return;
        var health = PlayerController.Instance.GetComponent<PlayerHealth>();
        if (health == null) return;

        float fill = Mathf.Clamp01(health.CurrentHealth / health.maxHealth);
        if (heartFillerImage != null)
            heartFillerImage.fillAmount = fill;

        // Animação: pulsar e piscar vermelho quando a vida estiver baixa
        if (fill < lowHealthThreshold)
        {
            float scale = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
            Color blinkColor = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time * blinkSpeed, 1));

            if (heartFillerImage != null)
            {
                heartFillerImage.rectTransform.localScale = new Vector3(scale, scale, 1f);
                heartFillerImage.color = blinkColor;
            }
            if (heartFrameImage != null)
            {
                heartFrameImage.rectTransform.localScale = new Vector3(scale, scale, 1f);
                heartFrameImage.color = blinkColor;
            }
        }
        else
        {
            if (heartFillerImage != null)
            {
                heartFillerImage.rectTransform.localScale = Vector3.one;
                heartFillerImage.color = Color.white;
            }
            if (heartFrameImage != null)
            {
                heartFrameImage.rectTransform.localScale = Vector3.one;
                heartFrameImage.color = Color.white;
            }
        }
    }
}