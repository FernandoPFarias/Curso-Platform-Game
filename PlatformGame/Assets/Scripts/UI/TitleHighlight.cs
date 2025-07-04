using UnityEngine;

public class TitleHighlight : MonoBehaviour
{
    [SerializeField] private RectTransform highlight;
    [SerializeField] private float speed = 100f;
    [SerializeField] private float startX = -200f;
    [SerializeField] private float endX = 200f;

    void OnEnable()
    {
        if (highlight != null)
            highlight.anchoredPosition = new Vector2(startX, highlight.anchoredPosition.y);
    }

    void Update()
    {
        if (highlight != null)
        {
            float x = highlight.anchoredPosition.x + speed * Time.deltaTime;
            if (x > endX) x = startX;
            highlight.anchoredPosition = new Vector2(x, highlight.anchoredPosition.y);
        }
    }
} 