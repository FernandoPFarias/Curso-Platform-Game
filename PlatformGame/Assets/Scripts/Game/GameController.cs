using UnityEngine;
// using UnityEngine.UI;
// using TMPro;

public class GameController : MonoBehaviour
{   public static GameController instance;

    public int score;
    public SpriteText spriteText;

    public void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    public void GetCoin()
    {
        score++;
        if (spriteText != null)
            spriteText.SetText(score.ToString());
    }
}
