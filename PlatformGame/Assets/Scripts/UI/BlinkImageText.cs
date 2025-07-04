using UnityEngine;
using UnityEngine.UI;

public class BlinkImageText : MonoBehaviour
{
    [SerializeField] private Image[] images;
    [SerializeField] private float blinkSpeed = 2f;
    [SerializeField] private float minAlpha = 0.3f;
    [SerializeField] private float maxAlpha = 1f;

    void Awake()
    {
        if (images == null || images.Length == 0)
            images = GetComponentsInChildren<Image>();
    }

    void Update()
    {
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, (Mathf.Sin(Time.time * blinkSpeed) + 1f) / 2f);
        foreach (var img in images)
        {
            Color c = img.color;
            c.a = alpha;
            img.color = c;
        }
    }
} 