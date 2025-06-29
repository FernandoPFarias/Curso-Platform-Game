using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class SpriteText : MonoBehaviour
{
    public BitmapFont font;
    [Header("Texto exibido (apenas para edição no Editor)")]
    public string displayText = "PLAY";

    [Header("Espaçamento extra após caracteres específicos")]
    public string extraSpaceAfter = "x"; // caracteres que terão espaço extra depois
    public float extraSpacing = 10f;     // valor do espaço extra em pixels

    [Header("Offset manual do texto")]
    public float offsetX = 0f;
    public float offsetY = 0f;

#if UNITY_EDITOR
    [ContextMenu("Gerar Texto no Editor")]
    public void GenerateTextInEditor()
    {
        SetText(displayText);
    }
#endif

    public void SetText(string text)
    {
        // Limpa caracteres antigos
        foreach (Transform child in transform)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                DestroyImmediate(child.gameObject);
            else
#endif
                Destroy(child.gameObject);
        }

        float xOffset = 0f;
        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];
            var spriteChar = System.Array.Find(font.characters, x => x.character == c.ToString().ToUpper());
            if (spriteChar == null || spriteChar.sprite == null) continue;

            GameObject go = new GameObject("char_" + c, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            go.transform.SetParent(transform, false);

            var img = go.GetComponent<Image>();
            img.sprite = spriteChar.sprite;
            img.raycastTarget = false;

            var rt = go.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(spriteChar.sprite.rect.width, spriteChar.sprite.rect.height);
            rt.anchoredPosition = new Vector2(xOffset + offsetX, offsetY);

            xOffset += rt.sizeDelta.x + font.characterSpacing;

            // Adiciona espaço extra se o caractere for um dos definidos
            if (extraSpaceAfter.Contains(c.ToString()))
                xOffset += extraSpacing;
        }
    }
} 