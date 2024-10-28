using UnityEngine;
using TMPro;

public class DeathScore : MonoBehaviour
{
    public TMP_Text currentScore;
    public TMP_Text highscoreText;
    public static int highscore;

    void Start()
    {
        // Load the high score from PlayerPrefs
        highscore = PlayerPrefs.GetInt("Highscore", 0);

        // Update high score if current score is greater
        if (Score.score > highscore)
        {
            highscore = Score.score;
            PlayerPrefs.SetInt("Highscore", highscore);
            PlayerPrefs.Save(); // Ensure it saves immediately
        }

        currentScore.text = "SCORE: " + Score.score.ToString();
        highscoreText.text = "HIGHSCORE: " + highscore.ToString();
    }
}