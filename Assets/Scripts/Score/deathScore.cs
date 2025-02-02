using UnityEngine;
using TMPro;

public class DeathScore : MonoBehaviour
{
    public TMP_Text currentScore;
    public TMP_Text highscoreText;
    public static int highscore;

    private int score;
    

    void Start()
    {
        // Load the high score from PlayerPrefs
        highscore = PlayerPrefs.GetInt("Highscore", 0);

        // Get the current score from the Score class
        score = Score.score;

        // Update high score if current score is greater
        if (score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetInt("Highscore", highscore);
            PlayerPrefs.Save(); // Ensure it saves immediately
        }

        currentScore.text = "SCORE: " + score.ToString();
        highscoreText.text = "HIGHSCORE: " + highscore.ToString();
    }
}