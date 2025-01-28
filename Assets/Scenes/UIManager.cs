using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI scoreText;

    private int lives = 3;
    private int score = 0;

    private void Start()
    {
        UpdateLivesUI();
        UpdateScoreUI();
    }

    public void UpdateLives(int newLives)
    {
        lives = newLives;
        UpdateLivesUI();

    }

    public void UpdateScore(int newScore)
    {
        score = newScore;
        UpdateScoreUI();
    }

    public int GetScore()
    {
        return score;
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
        {
            livesText.text = $"Lives: {lives}";
        }
        else
        {
            Debug.LogError("Lives text is not assigned in the UIManager!");
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
        else
        {
            Debug.LogError("Score text is not assigned in the UIManager!");
        }
    }
}