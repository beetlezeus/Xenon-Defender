using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

/*
 This script handles displaying the Laderboard UI on the dedicated Leaderboard screen
 It leverages the LoadHighScores method in the HighScoreManager class to fetch the top 10 high scores from the Leaderboard
 */
public class LeaderboardUIManager : MonoBehaviour
{
    [SerializeField] TMP_Text leaderboardText; // UI element for displaying the leaderboard entries
    [SerializeField] HighscoreManager highscoreManager; // reference to the highScoreManager script for fething the entries

    private void Start()
    {
        // if highScoreManager is not assigned, assign it to the HighScoreManager component in Game Manager game object
        if (highscoreManager == null)
        {
            highscoreManager = GameObject.Find("Game Manager").GetComponent<HighscoreManager>();
        }
    }

    // Displays the loaded leaderboard entries
    public async Task DisplayHighScores()
    {
        // Ensure leaderboardText is assigned
        if (leaderboardText == null)
        {
            Debug.LogError("Leaderboard text object is not assigned.");
            return;
        }

        // reset the leaderboard text and add a buffer line
        leaderboardText.text = "";
        leaderboardText.text += "\n";

        try
        {
            // variable to store result of calling LoadHighScores from highScoreManager and await results
            var highScores = await highscoreManager.LoadHighScores();

            // if the highScores list is empty, display a message to the player and return
            if (highScores.Count == 0)
            {
                leaderboardText.text += "\n";
                leaderboardText.text += "No high scores yet!";
                return;
            }
            // if list is not empty iterate over entries and display the player initials, score and kills
            foreach (var (playerName, score, kills) in highScores)
            {
                leaderboardText.text += $"{playerName}: {score} points, {kills} kills\n";
            }
        }
        catch(System.Exception ex)
        {
            Debug.LogError($"Error loading Leaderboard: {ex.Message}");
        }
    }

    // Wrapper method for calling DisplayHighScores without blocking Unity's main thread
    public void DisplayHighScoresWrapper()
    {
        _ = DisplayHighScores();
    }
}
