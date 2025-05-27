using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score = 0;
    private int highScore = 0;
    public Text scoreText;
    public Text highScoreText;

    public GameObject gameOverPanel;
    public GameObject pausePanel;

    public bool isGameOver = false;
    public bool isPaused = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);

        // Load saved high score
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score: " + highScore;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGameOver) return;

            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void AddScore(int linesCleared)
    {
        int points = 0;
        switch (linesCleared)
        {
            case 1: points = 100; break;
            case 2: points = 300; break;
            case 3: points = 500; break;
            case 4: points = 800; break;
        }
        score += points;
        scoreText.text = "Score: " + score;
    }

    public void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
    }

    public void GameOver()
    {
        isGameOver = true;

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (highScoreText != null)
            highScoreText.text = "High Score: " + highScore;

        Debug.Log("Game Over triggered");
    }


    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
