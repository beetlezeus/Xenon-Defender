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
        highscoreManager = GetComponent<HighscoreManager>();
    }

    public async Task DisplayHighScores()
    {
        var highScores = await highscoreManager.LoadHighScores();

        foreach (var (playerName, score, kills) in highScores)
        {
            leaderboardText.text += $"{playerName}: {score} points, {kills} kills\n";
        }
    }
}
