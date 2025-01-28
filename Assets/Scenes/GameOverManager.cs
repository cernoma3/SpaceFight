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
        // Skry� texty a tla�idlo pri �tarte hry
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

        // Zobrazenie GAME OVER textu
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = "GAME OVER";
        }

        // Zobrazenie sk�re
        if (finalScoreText != null)
        {
            finalScoreText.gameObject.SetActive(true);
            finalScoreText.text = $"Final Score: {finalScore}";
        }

        // Zobrazenie tla�idla �Try again?�
        if (retryButton != null)
        {
            retryButton.SetActive(true);
        }
    }

    // Funkcia pre tla�idlo �Try again?�
    public void RetryGame()
    {
        Debug.Log("Restarting the game...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Na��ta aktu�lnu sc�nu od za�iatku
    }
}