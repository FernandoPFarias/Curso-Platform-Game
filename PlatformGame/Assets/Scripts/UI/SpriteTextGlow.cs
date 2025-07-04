using UnityEngine;

public class SpriteTextGlow : MonoBehaviour
{
    [SerializeField] private Color colorA = Color.white;
    [SerializeField] private Color colorB = new Color(1f, 0.95f, 0.6f);
    [SerializeField] private float speed = 2f;

    private SpriteRenderer[] letters;

    void Awake()
    {
        letters = GetComponentsInChildren<SpriteRenderer>();
    }

    void Update()
    {
        float t = (Mathf.Sin(Time.time * speed) + 1f) / 2f;
        Color lerped = Color.Lerp(colorA, colorB, t);
        foreach (var sr in letters)
        {
            sr.color = lerped;
        }
    }
} 