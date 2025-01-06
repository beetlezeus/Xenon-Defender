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

        if(leaderboardText == null)
        {

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
        var highScores = await highscoreManager.LoadHighScores();

        // Check if there are no scores
        if (highScores.Count == 0)
        {
            leaderboardText.text = "No high scores yet!";
            return;
        }

        //leaderboardText.text = " ** Top 10 Scores \n";
        leaderboardText.text += "\n";

        foreach (var (playerName, score, kills) in highScores)
        {
            leaderboardText.text += $"{playerName}: {score} points, {kills} kills\n";
        }
    }

    // Wrapper method
    public void DisplayHighScoresWrapper()
    {
        // Call the async method but without breaking Unity's requirements
        _ = DisplayHighScores();
    }
}
