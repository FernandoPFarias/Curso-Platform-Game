using UnityEngine;
using TMPro;

public class TitleGlow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Color colorA = Color.white;
    [SerializeField] private Color colorB = new Color(1f, 0.95f, 0.6f); // Amarelo claro
    [SerializeField] private float speed = 2f;

    void Update()
    {
        if (titleText != null)
        {
            float t = (Mathf.Sin(Time.time * speed) + 1f) / 2f;
            titleText.color = Color.Lerp(colorA, colorB, t);
        }
    }
} 