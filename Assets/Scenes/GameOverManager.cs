using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private GameObject retryButton;

    private void Start()
    {
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("GameOverText is not assigned!");
        }

        if (finalScoreText != null)
        {
            finalScoreText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("FinalScoreText is not assigned!");
        }

        if (retryButton != null)
        {
            retryButton.SetActive(false);
        }
        else
        {
            Debug.LogError("RetryButton is not assigned!");
        }
    }

    public void ShowGameOver(int finalScore)
    {
        Debug.Log("Displaying Game Over screen...");

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = "GAME OVER";
        }

        if (finalScoreText != null)
        {
            finalScoreText.gameObject.SetActive(true);
            finalScoreText.text = $"Final Score: {finalScore}";
        }

        if (retryButton != null)
        {
            retryButton.SetActive(true);
        }
    }

    public void RetryGame()
    {
        Debug.Log("Restarting the game...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}