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
    private PlayableDirector masterTimeline;

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

    void Start()
    {
        AssignMasterTimeline();
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
        UpdateLivesDisplay();
    }

    private void AssignMasterTimeline()
    {
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            masterTimeline = GameObject.Find("Master Timeline").GetComponent<PlayableDirector>();

            if (masterTimeline == null)
            {
                Debug.LogError("Master Timeline PlayableDirector not found in the scene.");
            }
            else
            {
                Debug.Log("Master Timeline successfully assigned.");
            }
        }
    }

    private void StopMasterTimeline()
    {
        if (masterTimeline != null)
        {
            masterTimeline.Stop(); // Stop the timeline completely
            masterTimeline.time = 0; // Reset timeline position to the start
            masterTimeline.Evaluate();
            Debug.Log("Master Timeline stopped.");
        }
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
            ResetScores();
            UpdateLivesDisplay();
            UpdateEnemyHitScore(0);
            UpdateEnemyKillCount();

            if (playerLives <= 0)
            {
                isDead = true;
                StopMasterTimeline();
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

    public void ShowTransitionScreen(bool playerDead)
    {
        transitionCanvas.SetActive(true);
        if (playerDead)
        {
            missionFailedPanel.SetActive(true);
            missionFailedLivesText.text = "Lives: " + playerLives.ToString();
            missionFailedKillsText.text = "Kills: " + enemyKillCount.ToString();
            // Ensure buttons are only assigned once to avoid duplicate listeners
            missionFailedReturnButton.onClick.RemoveAllListeners();
            missionFailedProceedButton.onClick.RemoveAllListeners();
            // Add Listeners 
            missionFailedReturnButton.onClick.AddListener(ReturnToMainWithDelay);
            missionFailedProceedButton.onClick.AddListener(RestartLevelWithDelay);
        }
        else
        {
            missionSuccessPanel.SetActive(true);
            missionSuccessLivesText.text = "Lives: " + playerLives.ToString();
            missionSuccessKillsText.text = "Kills: " + enemyKillCount.ToString();
            // Ensure buttons are only assigned once to avoid duplicate listeners
            missionSuccessReturnButton.onClick.RemoveAllListeners();
            missionSuccessProceedButton.onClick.RemoveAllListeners();
            // Add Listeners
            missionFailedProceedButton.onClick.AddListener(RestartLevelWithDelay);
            missionSuccessReturnButton.onClick.AddListener(ReturnToMainWithDelay);
        }
    }

    public void RestartLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void RestartLevelWithDelay()
    {
        Invoke("RestartLevel", 1);
    }

    public void LoadNextStage()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void ReturnToMain()
    {
        SceneManager.LoadScene(0);
    }

    public void ReturnToMainWithDelay()
    {
        Invoke("ReturnToMain", 1);
        PersistentGameManager.Instance.transitionCanvas.SetActive(false);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }



}

