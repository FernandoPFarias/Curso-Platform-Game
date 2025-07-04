using UnityEngine;
using UnityEngine.UI;

public class SpriteTextGlowUI : MonoBehaviour
{
    [SerializeField] private Color colorA = Color.white;
    [SerializeField] private Color colorB = new Color(1f, 0.95f, 0.6f);
    [SerializeField] private float speed = 2f;

    private Image[] letters;

    void Awake()
    {
        letters = GetComponentsInChildren<Image>();
    }

    void Update()
    {
        float t = (Mathf.Sin(Time.time * speed) + 1f) / 2f;
        Color lerped = Color.Lerp(colorA, colorB, t);
        foreach (var img in letters)
        {
            img.color = lerped;
        }
    }
} 