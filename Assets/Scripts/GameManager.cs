using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance { get; private set; }

    public float initialGameSpeed = 8f;
    public float gameSpeedIncrease = 0.3f;
    public float gameSpeed { get; private set; }
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    public AudioSource deathSound;
    public AudioSource gameMusic;

    public Button retryButton;

    private Player player;
    private Spawner spawner;

    private float score;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else {
            DestroyImmediate(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        spawner = FindObjectOfType<Spawner>();
        NewGame();
    }

    public void NewGame()
    {
        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();
        
        foreach (var obstacle in obstacles) {
            Destroy(obstacle.gameObject);
        }

        gameSpeed = initialGameSpeed;
        score = 0f;
        enabled = true;

        player.gameObject.SetActive(true);
        spawner.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        gameMusic.Play();

        UpdateHighScore();
    }

    private void Update()
    {
        gameSpeed += gameSpeedIncrease * Time.deltaTime;
        score += gameSpeed * Time.deltaTime;
        scoreText.text = Mathf.FloorToInt(score).ToString("D5");
        if (score % 100 == 0)
        {
            FlashingScore();
        }
    }

    private void UpdateHighScore()
    {
        float highScore = PlayerPrefs.GetFloat("highscore", 0);

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetFloat("highscore", highScore);
        }

        highScoreText.text = Mathf.FloorToInt(highScore).ToString("D5");

    }

    private void FlashingScore()
    {
        scoreText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);
    }

    public void GameOver()
    {
        gameMusic.Stop();
        deathSound.Play();
        gameSpeed = 0f;
        enabled = false;

        player.gameObject.SetActive(false);
        spawner.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);

        UpdateHighScore();

    }
}
