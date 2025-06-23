using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{   public static GameController instance;

    public int score;
    public TextMeshProUGUI scoreText;



    public void Awake()
    {
        instance = this;

    }

    // Update is called once per frame
    public void GetCoin()
    {
        score++;
        scoreText.text = score.ToString();

    }
}
