using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject levelWinUI;
    public GameObject gameOverUI;

    public int totalLevels = 3;
    public int currentLevel = 1;

    public static LevelManager instance;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    public void LevelWon()
    {
        Time.timeScale = 0f;
        if (levelWinUI != null) levelWinUI.SetActive(true);
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        if (gameOverUI != null) gameOverUI.SetActive(true);
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextScene < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextScene);
        else
            Debug.Log("No more levels!");
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
