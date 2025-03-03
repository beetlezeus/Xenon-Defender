using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.Playables;

/*
 This class manages essential game state across scene loads, including player lives, crashes, scoring, UI updates, and transitioning between levels.

It follows the Singleton pattern to persist between scenes.
 */

public class PersistentGameManager : MonoBehaviour
{
    
    public static PersistentGameManager Instance; // Singleton reference for global access
    // Player states and scores
    private int playerLives = 3;
    public bool isCrashed = false; // flag if the player has recently crashed
    private int enemyHitScore = 0; // score from hitting enemies
    private int enemyKillCount = 0; // num enemies killed by the player
    private int highestScore = 0; // Highest score encountered this session
    private int highestKills = 0; // Highest kill count encountered this session
    public bool isDead = false;   // flag if the player has run out of lives
    public bool levelCleared = false;  // flag if the level has been successfully cleared
    public bool newHighScore = false; // Flag to indicate a newly achieved high score

    // Reference to the high score management class
    HighscoreManager highScoreManager;

    // UI references for in-game panels and text
    [SerializeField] GameObject gameUIPanel;
    [SerializeField] TMP_Text playerLivesText;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text killText;

    // Transition UI references
    [SerializeField] public GameObject transitionCanvas;
    [SerializeField] GameObject missionSuccessPanel;
    [SerializeField] GameObject missionFailedPanel;
    [SerializeField] TMP_Text missionSuccessKillsText;
    [SerializeField] TMP_Text missionSuccessScoreText;
    [SerializeField] TMP_Text missionSuccessLivesText;
    [SerializeField] TMP_Text missionFailedKillsText;
    [SerializeField] TMP_Text missionFailedScoreText;
    [SerializeField] TMP_Text missionFailedLivesText;
    [SerializeField] Button missionSuccessProceedButton;
    [SerializeField] Button missionSuccessReturnButton;
    [SerializeField] Button missionFailedProceedButton;
    [SerializeField] Button missionFailedReturnButton;
    [SerializeField] GameObject newHighScorePanel;
    [SerializeField] TMP_Text newHighScoreScore;
    [SerializeField] TMP_Text newHighScoreKills;
    [SerializeField] TMP_InputField playerInitialsInputField;
    [SerializeField] Button submitButton;


      //Awake is called before Start(). used to enforce the Singleton pattern.
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this GameObject alive across scenes
        }
        else
        {
            Destroy(gameObject); // Enforce single instance
        }
    }

     //Subscribes to the SceneManager's sceneLoaded event. ensures OnSceneLoaded is called when any scene finishes loading.
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to scene loaded event
    }

    // unsubscribes from the SceneLoaded event to prevent potential memory leaks when the object is disabled
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe to prevent memory leaks
    }

    //Called automatically when a scene completes loading to ensure proper scene setup    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex > 0)
        {
            gameUIPanel.SetActive(true); // Toggle the game UI panel on if gameplay scene.
        }
        else
        {
            gameUIPanel.SetActive(false);
        }
        ResetCrashState(); // Reset crash state
        AssignUITextObjects(); // assign UI references if necessary
        ResetScores(); // resets scores
        ClearGameScoreText(); // clear score text on UI
        UpdateLivesDisplay(); //update the player's lives display.

        highScoreManager = GetComponent<HighscoreManager>(); //Initializes the highScoreManager component reference
    }

    // Assigns references to the TMP_Text objects for PlayerLivesText, ScoreText, killText.
    // ensures that UI elements are assigned properly 
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

    // decrements player lives
    public void DecrementLives()
    {
        // if player not already crashed decrement lives, set isCrashed to true and update the Lives display function
        if (!isCrashed)
        {
            playerLives--;
            isCrashed = true;
            UpdateLivesDisplay();

            // if no lives remaining set isDead to true, pause the game and call ShowTransitionScreen function
            if (playerLives <= 0)
            {
                isDead = true;
                Time.timeScale = 0f;
                _ = ShowTransitionScreen(isDead);
            }
        }
    }

    // Called when player crashes to start the death sequence
    public void StartDeathSequence()
    {
        DecrementLives();  // decrement lives
        SetHighScoreAndKills(); // update the high score and high kills tracker
        ResetScores(); // reset the current scores to zero

        // if isDead is true end early
        if (isDead)
        {
            return;
        }
        
        RestartLevelWithDelay(); // if not dead restart the level
    }

    // Resets the crash state so the player can be hit again.
    public void ResetCrashState()
    {
        isCrashed = false;
    }

    // Resets death related states so the game can continue
    public void ResetDeathState()
    {
        Time.timeScale = 1.0f; // restore time scale back to 1
        isDead = false; // reset the isDead flag to false
        ResetScores(); // reset scores back to 0
        ClearGameScoreText(); // clear the scores text UI
        playerLives = 3;  // restore playerLives back to 3 to restart gameplay
    }

    // Clears the levelCleared flag to begin a new level normally.
    public void ResetLevelClearFlag()
    {
        levelCleared = false;
    }

    // Updates the UI text element that shows the current player lives
    void UpdateLivesDisplay()
    {

        playerLivesText.text = "Lives: " + playerLives.ToString();
    }

    // Resets the hit score and enemy kill score when needed to update the UI upon crashing / restarting the scene
    public void ResetScores()
    {
        enemyHitScore = 0;
        enemyKillCount = 0;
    }

    // Updated the UI element that shows enemy hit score
    public void UpdateEnemyHitScore(int hitScore)
    {
        // if player hasn't already crashed increment the score by passed value, update the UI text with latest score
        if (!isCrashed)
        {
            enemyHitScore += hitScore;
            scoreText.text = "SCORE: " + enemyHitScore.ToString();
        }
        
    }

    // clears the in-game score and kill text UI
    public void ClearGameScoreText()
    {
        scoreText.text = "SCORE: ";
        killText.text = "KILLS: ";

    }

    // clears the mission high score fields on the mission failed and mission success UI panels
    // ensures panels always start with clear text before updating for display
    public void ClearHighScoreText()
    {
        missionFailedScoreText.text = "";
        missionFailedKillsText.text = "";
        missionSuccessKillsText.text = "";
        missionSuccessScoreText.text = "";

    }

    // Updates the UI element that shows enemy hit score
    public void UpdateEnemyKillCount()
    {
        // if player hasn't already crashed increment the kills, update the UI text with latest count
        if (!isCrashed)
        {
            enemyKillCount++;
            killText.text = "KILLS: " + enemyKillCount.ToString();
        }
    }

    // Asynchronous method invoked when showing end-of-level or death transitions.
    public async Task ShowTransitionScreen(bool playerDead)
    {
        SetHighScoreAndKills(); // ensure latest score/kill data is set
        await highScoreManager.CheckForNewHighScore(highestScore); // check for a new high score (await the result)
        transitionCanvas.SetActive(true); // Display the transition canvas

        // if player got new highscore display the New High Score UI panel, else display the mision status panel based on playerDead
        if (newHighScore)
        {
            ShowNewHighScorePanel();
        }
        else
        {
            ShowMissionStatusPanel(playerDead);
        }
    }

    // Displays the panel for entering initials when a new high score is achieved
    private void ShowNewHighScorePanel()
    {
        playerInitialsInputField.text = ""; // Reset the initials input field
        SetHighScoreAndKills(); // update high-score values to ensure latest high scores are displayed
        Time.timeScale = 0f;  // pause game
        missionFailedPanel.SetActive(false);  // disable the mission failed panel
        missionSuccessPanel.SetActive(false); // disable the mission success panel
        newHighScorePanel.SetActive(true);    // enable the New High Score panel
        newHighScoreKills.text = "Highest Kills: " + highestKills.ToString();  // display highset kills
        newHighScoreScore.text = "Highest Score: " + highestScore.ToString();  // display highest score
        submitButton.onClick.RemoveAllListeners(); // ensure buttons are only assigned once to avoid duplicate listeners
        submitButton.onClick.AddListener(OnNewHighScoreSubmitWrapper); // Add Listeners
    }

    // Wrapper method to call the asynchronous OnNewHighScoreSubmit() without blocking Unity’s main thread
    // Called when the submitButton is clicked.
    private void OnNewHighScoreSubmitWrapper()
    {
        _ = OnNewHighScoreSubmit();
    }

    // Asynchronous method that verifies the player's initials
    private async Task OnNewHighScoreSubmit()
    {
        string playerInitials = playerInitialsInputField.text;

        // ensure player initials have proper value. If no initials are provided, do nothing
        // else store the new highscore with entered initials
        if (playerInitials.Length == 0)
        {
            return;
        }
        else 
        {
            await highScoreManager.SaveHighScore(playerInitials, highestScore, highestKills);
            newHighScore = false;
            newHighScorePanel.SetActive(false);
            ShowMissionStatusPanel(isDead);  // after submitting score, display the mission status panel
        }
    }

    // Controls which mission status panel to display
    private void ShowMissionStatusPanel(bool playerDead)
    {
        // if player has died (playerDead is true) display the mission failed UI panel
        // else display the mission success panel
        if (playerDead)
        {
            ShowMissionFailedPanel();
        }
        else
        {
            ShowMissionSuccessPanel();
        }

    }

    // Displays the mission success panel with relevant data
    private void ShowMissionSuccessPanel()
    {
        SetHighScoreAndKills(); // update high score and kills
        Time.timeScale = 0f; // pause the game

        // ensure only correct panel is active
        missionFailedPanel.SetActive(false); 
        missionSuccessPanel.SetActive(true);

        // display remaining lives, highest score, highest kills
        missionSuccessLivesText.text = "Lives Remaining: " + playerLives.ToString(); 
        missionSuccessKillsText.text = "Kills: " + enemyKillCount.ToString();  
        missionSuccessScoreText.text = "Score: " + enemyHitScore.ToString();

        // Ensure buttons are only assigned once to avoid duplicate listeners
        missionSuccessReturnButton.onClick.RemoveAllListeners();
        missionSuccessProceedButton.onClick.RemoveAllListeners();

        // Add Listeners
        missionSuccessProceedButton.onClick.AddListener(RestartLevel);
        missionSuccessReturnButton.onClick.AddListener(ReturnToMain);
    }

    //Displays the mission failed panel with relevant data
    private void ShowMissionFailedPanel()
    {
        SetHighScoreAndKills(); // update high score and kills
        // ensure only correct panel is active
        missionSuccessPanel.SetActive(false);
        missionFailedPanel.SetActive(true);

        // display remaining lives, highest score, highest kills
        missionFailedLivesText.text = "Lives Remaining: " + playerLives.ToString();
        missionFailedKillsText.text = "Highest Kills: " + highestKills.ToString();
        missionFailedScoreText.text = "Highest Score: " + highestScore.ToString();

        // Ensure buttons are only assigned once to avoid duplicate listeners
        missionFailedReturnButton.onClick.RemoveAllListeners();
        missionFailedProceedButton.onClick.RemoveAllListeners();

        // Add Listeners 
        missionFailedReturnButton.onClick.AddListener(ReturnToMain);
        missionFailedProceedButton.onClick.AddListener(RestartLevel);
    }

    // Resets the tracked highest score and kills in the current gameplay session
    public void ResetHighScores()
    {
        highestKills = 0;
        highestScore = 0;
    }

    // Restarts the current level by reloading the active scene
    public void RestartLevel()
    {
        // if player has died, reset death states
        if (isDead)
        {
            Time.timeScale = 1.0f;
            isDead = false;
            playerLives = 3;
            ResetHighScores();
            ClearHighScoreText();
        }

        // if level was cleared, reset states
        // THIS LOGIC IS HERE AS PLACEHOLDER. THIS SHOULD BE REMOVED & REFACORED INTO THE LOAD NEXT STAGE FUNCTION. PLAYER LIVES SHOULD NOT RESET TO 3
        if (levelCleared)
        {
            Time.timeScale = 1.0f;
            levelCleared = false;
            playerLives = 3;
            ResetHighScores();
            ClearHighScoreText();
        }
        // disable the transition canvas and reload the scene
        transitionCanvas.SetActive(false);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    // Restarts the level with a delay of 1 second before calling Restartlevel
    public void RestartLevelWithDelay()
    {
        Invoke(nameof(RestartLevel), 1);
        transitionCanvas.SetActive(false);
    }

    // Loads the next stage by incrementing the current scene’s build index.
    public void LoadNextStage()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
        transitionCanvas.SetActive(false);
    }

    // Returns to the main menu (scene index 0) and resets the scene states
    public void ReturnToMain()
    {
        ResetHighScores();
        transitionCanvas.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    // Returns to Main with a delay of 1 second before calling Return to main
    public void ReturnToMainWithDelay()
    {
        Invoke(nameof(ReturnToMain), 1);
        Instance.transitionCanvas.SetActive(false);
    }

    // Ensures that the player's current run's score and kills are the highest encountered in the gameplay session.
    void SetHighScoreAndKills()
    {
        if(enemyHitScore > highestScore)
        {
            highestScore = enemyHitScore;
        }

        if (enemyKillCount > highestKills)
        {
            highestKills = enemyKillCount;
        }

    }

    // Unsubscribes from the SceneLoaded event.
    // ensures no event references left behind when object is destroyed
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

