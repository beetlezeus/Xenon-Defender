using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class PersistentGameManager : MonoBehaviour
{

    public static PersistentGameManager Instance;
    public int playerLives = 3;
    public bool isCrashed = false;
    private int enemyHitScore = 0;
    private int enemyKillCount = 0;

    public TMP_Text playerLivesText;
    public TMP_Text scoreText;
    public TMP_Text killText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to scene loaded event
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe to prevent memory leaks
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetCrashState();
        AssignUITextObjects();
        UpdateLivesDisplay();
    }

    private void AssignUITextObjects()
    {
        if (playerLivesText == null)
        {
            playerLivesText = GameObject.Find("Lives Text").GetComponent<TMP_Text>();
        }
        if (scoreText == null)
        {
            scoreText = GameObject.Find("Score Text").GetComponent<TMP_Text>();
        }
        if (killText == null)
        {
            killText = GameObject.Find("Enemy Kill Text").GetComponent<TMP_Text>();
        }
    }

    public void DecrementLives()
    {
        if (!isCrashed)
        {
            playerLives--;
            isCrashed = true;

            if (playerLives <= 0)
            {
                Debug.Log("Game Over!");
                // Implement game-over logic, e.g., load a game-over screen
            }
        }
    }

    public void StartDeathSequence()
    {
        DecrementLives();
        ResetScores();
    }

    //void ResetPlayerScores()
    //{
    //    PlayerScoreUI playerScore = FindObjectOfType<PlayerScoreUI>();
    //    if (playerScore != null)
    //    {
    //        playerScore.ResetScores(); // Ensure scores reset on scene reload
    //    }
    //}

    public void ResetCrashState()
    {
        isCrashed = false;
    }

    void UpdateLivesDisplay()
    {

        playerLivesText.text = "Lives: " + playerLives.ToString();
    }

    void ResetScores()
    {
        enemyHitScore = 0;
        enemyKillCount = 0;
    }

    public void UpdateEnemyHitScore(int hitScore)
    {
        if (!isCrashed)
        {
            enemyHitScore += hitScore;
            scoreText.text = "SCORE: " + enemyHitScore.ToString();
        }
        
    }

    public void UpdateEnemyKillCount()
    {
        if (!isCrashed)
        {
            enemyKillCount++;
            killText.text = "KILLS: " + enemyKillCount.ToString();
        }
    }

}

