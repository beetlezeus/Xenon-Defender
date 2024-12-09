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

    public TMP_Text playerLivesText;

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
        ResetPlayerScores();

        if(playerLivesText == null)
        {
            playerLivesText = GameObject.Find("Lives Text").GetComponent<TMP_Text>();
        }

        UpdateLivesDisplay();
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

    void ResetPlayerScores()
    {
        PlayerScoreUI playerScore = FindObjectOfType<PlayerScoreUI>();
        if (playerScore != null)
        {
            playerScore.ResetScores(); // Ensure scores reset on scene reload
        }
    }

    public void ResetCrashState()
    {
        isCrashed = false;
    }

    void UpdateLivesDisplay()
    {

        playerLivesText.text = "Lives: " + playerLives.ToString();
    }

}

