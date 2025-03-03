using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/* 

  This script handles the Pause Menu UI and its functionalities

 */
public class PauseMenu : MonoBehaviour
{
    // References to UI elements for the pause menu
    public GameObject pauseMenuCanvas; // Reference to the Pause Menu Canvas
    public GameObject pauseMenuPanel; // Reference to the Pause Menu Panel
    public Button resumeButton; // Reference to the Resume Button
    public Button quitButton; // Reference to the Quit Button

    public bool isPaused = false; // flag for whehter game is paused or not

    void Start()
    {
        // Ensure the pause menu UI is initially inactive
        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.SetActive(false);
        }
        else
        {
            Debug.LogError("PauseMenuCanvas is not assigned in the inspector!");
        }

        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("PauseMenuPanel is not assigned in the inspector!");
        }

        // Assign button listeners
        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(Resume);
        }
        else
        {
            Debug.LogError("ResumeButton is not assigned in the inspector!");
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitToMainMenu);
        }
        else
        {
            Debug.LogError("QuitButton is not assigned in the inspector!");
        }
    }

    void Update()
    {
        // check if the ESC key is pressed
        // only toggle pause menu if scene build is not 0, player is alive, and gameplay is sctive
        if ((Input.GetKeyDown(KeyCode.Escape)) &&
            (SceneManager.GetActiveScene().buildIndex != 0) &&
            (!PersistentGameManager.Instance.isDead) &&
            (!PersistentGameManager.Instance.levelCleared))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    // Resumes normal gameplay
    public void Resume()
    {
        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.SetActive(false);
        }

        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }

        Time.timeScale = 1f;
        isPaused = false;
    }

    // Pauses the gameplay and displays pause menu
    void Pause()
    {
        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.SetActive(true);
        }

        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
        }

        Time.timeScale = 0f;
        isPaused = true;
    }

    // ends game play and returns to the main menu (scene 0)
    public void QuitToMainMenu()
    {
        // Deactivate pause menu UI
        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.SetActive(false);
        }
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }

        Time.timeScale = 1f; // Reset time scale
        isPaused = false; // Reset pause state

        // Load the main menu scene
        SceneManager.LoadScene("0_IntroScene");
    }
}