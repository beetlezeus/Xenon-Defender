using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;

public class PersistentGameManager : MonoBehaviour
{

    public static PersistentGameManager Instance;
    private int playerLives = 3;
    public bool isCrashed = false;
    private int enemyHitScore = 0;
    private int enemyKillCount = 0;
    public bool isDead = false;

    [SerializeField] GameObject gameUIPanel;
    [SerializeField] TMP_Text playerLivesText;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text killText;

    [SerializeField] public GameObject transitionCanvas;
    [SerializeField] GameObject missionSuccessPanel;
    [SerializeField] GameObject missionFailedPanel;
    [SerializeField] TMP_Text missionSuccessKillsText;
    [SerializeField] TMP_Text missionSuccessLivesText;
    [SerializeField] TMP_Text missionFailedKillsText;
    [SerializeField] TMP_Text missionFailedLivesText;
    [SerializeField] Button missionSuccessProceedButton;
    [SerializeField] Button missionSuccessReturnButton;
    [SerializeField] Button missionFailedProceedButton;
    [SerializeField] Button missionFailedReturnButton;

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
        if(scene.buildIndex > 0)
        {
            gameUIPanel.SetActive(true);
        }
        else
        {
            gameUIPanel.SetActive(false);
        }
        ResetCrashState();
        AssignUITextObjects();
        ResetScores();
        ClearGameScoreText();
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
            UpdateLivesDisplay();

            if (playerLives <= 0)
            {
                isDead = true;
                Time.timeScale = 0f;
                ShowTransitionScreen(isDead);
            }
        }
    }

    public void StartDeathSequence()
    {
        DecrementLives();
        if (isDead)
        {
            return;
        }
        ResetScores();
        RestartLevelWithDelay();
    }

    public void ResetCrashState()
    {
        isCrashed = false;
    }

    public void ResetDeathState()
    {
        Time.timeScale = 1.0f;
        isDead = false;
        ResetScores();
        ClearGameScoreText();
        playerLives = 3;
    }

    void UpdateLivesDisplay()
    {

        playerLivesText.text = "Lives: " + playerLives.ToString();
    }

    public void ResetScores()
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

    public void ClearGameScoreText()
    {
        scoreText.text = "SCORE: ";
        killText.text = "KILLS: ";

    }

    public void UpdateEnemyKillCount()
    {
        if (!isCrashed)
        {
            enemyKillCount++;
            killText.text = "KILLS: " + enemyKillCount.ToString();
        }
    }

    public void ShowTransitionScreen(bool playerDead)
    {
        transitionCanvas.SetActive(true);
        if (playerDead)
        {
            missionSuccessPanel.SetActive(false);
            missionFailedPanel.SetActive(true);
            missionFailedLivesText.text = "Lives: " + playerLives.ToString();
            missionFailedKillsText.text = "Kills: " + enemyKillCount.ToString();
            // Ensure buttons are only assigned once to avoid duplicate listeners
            missionFailedReturnButton.onClick.RemoveAllListeners();
            missionFailedProceedButton.onClick.RemoveAllListeners();
            // Add Listeners 
            missionFailedReturnButton.onClick.AddListener(ReturnToMain);
            missionFailedProceedButton.onClick.AddListener(RestartLevel);
        }
        else
        {
            missionFailedPanel.SetActive(false);
            missionSuccessPanel.SetActive(true);
            missionSuccessLivesText.text = "Lives: " + playerLives.ToString();
            missionSuccessKillsText.text = "Kills: " + enemyKillCount.ToString();
            // Ensure buttons are only assigned once to avoid duplicate listeners
            missionSuccessReturnButton.onClick.RemoveAllListeners();
            missionSuccessProceedButton.onClick.RemoveAllListeners();
            // Add Listeners
            missionSuccessProceedButton.onClick.AddListener(RestartLevel);
            missionSuccessReturnButton.onClick.AddListener(ReturnToMain);
        }
    }

    public void RestartLevel()
    {
        if (isDead)
        {
            Time.timeScale = 1.0f;
            isDead = false;
            playerLives = 3;
        }
        transitionCanvas.SetActive(false);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void RestartLevelWithDelay()
    {
        Invoke("RestartLevel", 1);
        transitionCanvas.SetActive(false);
    }

    public void LoadNextStage()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
        transitionCanvas.SetActive(false);
    }

    public void ReturnToMain()
    {
        SceneManager.LoadScene(0);
        transitionCanvas.SetActive(false);
    }

    public void ReturnToMainWithDelay()
    {
        Invoke("ReturnToMain", 1);
        Instance.transitionCanvas.SetActive(false);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

