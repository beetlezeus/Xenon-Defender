using System.Collections;
using Unity.Services.CloudSave;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class HighscoreManager : MonoBehaviour
{
    public async Task SaveHighScore(string playerName, int score, int kills)
    {
        try
        {
            // Construct a unique key for the player
            string uniqueKey = $"HighScore_{playerName}";

            // Create a string to store score and kills in a simple format
            string value = $"{score}|{kills}";

            // Save the data to Cloud Save
            var data = new Dictionary<string, object>
        {
            { uniqueKey, value }
        };
            await CloudSaveService.Instance.Data.Player.SaveAsync(data);

            Debug.Log($"High score saved: {uniqueKey} -> {value}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error saving high score: {ex.Message}");
        }
    }


    public async Task<List<(string playerName, int score, int kills)>> LoadHighScores()
    {
        var highScores = new List<(string playerName, int score, int kills)>();

        try
        {
            // Load all data from Cloud Save
            var savedData = await CloudSaveService.Instance.Data.Player.LoadAllAsync();

            foreach (var entry in savedData)
            {
                if (entry.Key.StartsWith("HighScore_"))
                {
                    string playerName = entry.Key.Replace("HighScore_", ""); // Extract player name
                    string[] values = entry.Value.ToString().Split('|');    // Split "Score|Kills"

                    if (values.Length == 2 &&
                        int.TryParse(values[0], out int score) &&
                        int.TryParse(values[1], out int kills))
                    {
                        highScores.Add((playerName, score, kills));
                    }
                    else
                    {
                        Debug.LogWarning($"Invalid high score format for key {entry.Key}: {entry.Value}");
                    }
                }
            }

            // Sort high scores by score in descending order
            highScores.Sort((a, b) => b.score.CompareTo(a.score));

            // Trim to top 10
            if (highScores.Count > 10)
            {
                highScores = highScores.GetRange(0, 10);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error loading high scores: {ex.Message}");
        }

        return highScores;
    }

    public async Task CheckForNewHighScore(int score)
    {
        var highScores = await LoadHighScores();

        // Check if the new score qualifies for the top 10
        if (highScores.Count < 10 || score > highScores[highScores.Count - 1].score)
        {
            Debug.Log("New High Score!");
            // await SaveHighScore(playerName, score, kills); // Uncomment and implement SaveHighScore
            PersistentGameManager.Instance.newHighScore = true;
        }
        else
        {
            Debug.Log("Score did not qualify for the top 10.");
        }
    }
}