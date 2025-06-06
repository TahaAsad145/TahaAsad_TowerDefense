using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class BaseHealth : MonoBehaviour
{
    public static BaseHealth Instance;

    public int startingLives = 15;
    private int currentLives;

    public TextMeshProUGUI lives;
    public GameObject gameOverUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentLives = startingLives;
        UpdateUi();
    }
    public void LoseLife() 
    {
        currentLives--;
        UpdateUi();
        if(currentLives <= 0)
        {
            GameOver();
        }
    }

    public void UpdateUi()
    {
        if(lives != null)
        {
            lives.text = "Lives: " + currentLives;
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        if (gameOverUI != null) gameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
