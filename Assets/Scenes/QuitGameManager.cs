using UnityEngine;

public class QuitGameManager : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("Quitting the game...");
        Application.Quit();

        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}