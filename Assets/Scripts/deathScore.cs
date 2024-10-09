using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class deathScore : MonoBehaviour
{
    public TMP_Text currentScore;
    public static int highscore;
    public TMP_Text highscoreText;

    void Start()
    {
        if (Score.score > highscore)
        {
            highscore = Score.score;
        }

        currentScore.text = "SCORE: " + Score.score.ToString();
        highscoreText.text = "HIGHSCORE: " + highscore.ToString();
    }
}
