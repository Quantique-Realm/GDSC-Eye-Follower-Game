using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject gameOverUI; // Reference to the Game Over UI Panel

    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep GameManager across scenes if needed
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;

            // Show the Game Over UI
            gameOverUI.SetActive(true);

            // Optionally, freeze the game
            Time.timeScale = 0f;
        }
    }

    // Optional: Function to Restart the Game
    public void RestartGame()
    {
        isGameOver = false;
        gameOverUI.SetActive(false);
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}