using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScreenManager : MonoBehaviour
{
    [SerializeField] private float splashDuration = 5f;
    [SerializeField] private string nextSceneName = "SampleScene";

    private void Start()
    {
        StartCoroutine(LoadNextSceneAfterDelay());
    }

    private IEnumerator LoadNextSceneAfterDelay()
    {
        yield return new WaitForSeconds(splashDuration);
        SceneManager.LoadScene(nextSceneName);
    }
}