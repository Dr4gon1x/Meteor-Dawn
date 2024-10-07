using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text scoreText; // Reference to the UI Text component
    private float elapsedTime;
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0f;
        score = 0;
        UpdateScoreText();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        score = Mathf.FloorToInt(elapsedTime*100);
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
}
