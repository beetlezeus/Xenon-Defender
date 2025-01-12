using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class LeaderboardUIManager : MonoBehaviour
{
    [SerializeField] TMP_Text leaderboardText; // Assign in the Inspector
    [SerializeField] HighscoreManager highscoreManager;

    private void Start()
    {
        if (highscoreManager == null)
        {
            highscoreManager = GameObject.Find("Game Manager").GetComponent<HighscoreManager>();
        }
    }

    public async Task DisplayHighScores()
    {
        // Ensure leaderboardText is assigned
        if (leaderboardText == null)
        {
            Debug.LogError("Leaderboard text object is not assigned.");
            return;
        }

        leaderboardText.text = "";
        leaderboardText.text += "\n";
        try
        {
            var highScores = await highscoreManager.LoadHighScores();

            // Check if there are no scores
            if (highScores.Count == 0)
            {
                leaderboardText.text += "\n";
                leaderboardText.text += "No high scores yet!";
                return;
            }

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

    // Wrapper method
    public void DisplayHighScoresWrapper()
    {
        // Call the async method but without breaking Unity's requirements
        _ = DisplayHighScores();
    }
}
