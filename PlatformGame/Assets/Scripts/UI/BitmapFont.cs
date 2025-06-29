#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[System.Serializable]
public class SpriteChar
{
    public string character; // Ex: "A", "1", "?"
    public Sprite sprite;
}

[CreateAssetMenu(fileName = "BitmapFont", menuName = "UI/BitmapFont")]
public class BitmapFont : ScriptableObject
{
    public SpriteChar[] characters;
    public float characterSpacing = 0f;
    public float lineHeight = 32f;
    // Adicione outras configs se quiser (offset, cor padrão, etc)

#if UNITY_EDITOR
    [ContextMenu("Preencher caracteres padrão")]
    public void PreencherCaracteresPadrao()
    {
        string padrao = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ?!/";
        characters = new SpriteChar[padrao.Length];
        for (int i = 0; i < padrao.Length; i++)
        {
            characters[i] = new SpriteChar { character = padrao[i].ToString(), sprite = null };
        }
        EditorUtility.SetDirty(this);
    }
#endif
} 