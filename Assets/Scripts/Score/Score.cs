using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public TextMeshPro scoreText; // Reference to the TextMeshProUGUI component
    private float elapsedTime;
    public static int score;

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
        score = Mathf.FloorToInt(elapsedTime * 120);
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
}
