using UnityEngine;

public class StartGameManager : MonoBehaviour
{
    [SerializeField] private GameObject startButton;

    private void Start()
    {
        Time.timeScale = 0;

        if (startButton != null)
        {
            startButton.SetActive(true);
        }
        else
        {
            Debug.LogError("StartButton is not assigned!");
        }
    }

    public void StartGame()
    {
        Debug.Log("Starting game...");
        Time.timeScale = 1;
        if (startButton != null)
        {
            startButton.SetActive(false);
        }
    }
}