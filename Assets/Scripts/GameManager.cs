using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score = 0;
    private int highScore = 0;
    public Text scoreText;
    public Text highScoreText;

    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public GameObject startPanel;
    public GameObject gameLogicRoot; // Parent of game logic like spawner


    public bool isGameOver = false;
    public bool isPaused = false;
    private bool gameStarted = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
        startPanel.SetActive(true);
        gameLogicRoot.SetActive(false);


        // Load saved high score
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score: " + highScore;
    }

    private void Update()
    {
        // Start Game
        if (!gameStarted && Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }

        // Pause/Resume
        if (gameStarted && !isGameOver && Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGameOver) return;

            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }

        if (isGameOver && Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
            highScoreText.text = "High Score: " + highScore;
        }
    }

    private void StartGame()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.gameStartSound);
        gameStarted = true;
        startPanel.SetActive(false);
        gameLogicRoot.SetActive(true);
    }

    public void AddScore(int linesCleared)
    {
        int points = 0;
        switch (linesCleared)
        {
            case 1: points = 100; break;
            case 2: points = 300; break;
            case 3: points = 500; break;
            case 4: points = 1000; break;
        }
        score += points;
        scoreText.text = "Score: " + score;
    }

    public float GetFallTime()
    {
        // Start at 0.8s, decrease as score increases (min 0.1s)
        return Mathf.Max(0.1f, 0.8f - (score / 1000f) * 0.1f);
    }

    public void PauseGame()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.gameStartSound);
        isPaused = true;
        pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.gameStartSound);
        isPaused = false;
        pausePanel.SetActive(false);
    }

    public void GameOver()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.gameOverSound);
        AudioManager.Instance.musicSource.Stop();
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
        AudioManager.Instance.musicSource.Play();
    }
}
