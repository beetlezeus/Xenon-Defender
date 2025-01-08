using System.Collections;
using Unity.Services.CloudSave;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;
using Unity.Services.CloudSave.Internal.Http;
using UnityEngine;

[System.Serializable]
public class HighScoreEntry
{
    public int Score;
    public int Kills;
}

public class HighscoreManager : MonoBehaviour
{
    public async Task SaveHighScore(string playerName, int score, int kills)
    {
        //try
        //{
        //    // Construct a unique key for the player
        //    string uniqueKey = $"HighScore_{playerName}";

        //    // Create a high score entry object
        //    var highScoreEntry = new HighScoreEntry
        //    {
        //        Score = score,
        //        Kills = kills
        //    };

        //    // Convert the high score entry object to JSON
        //    string jsonValue = JsonUtility.ToJson(highScoreEntry);

        //    // Save the data to Cloud Save
        //    var data = new Dictionary<string, object>
        //{
        //    { uniqueKey, jsonValue }
        //};

        //    await CloudSaveService.Instance.Data.Player.SaveAsync(data);

        //    Debug.Log($"High score saved: {uniqueKey} -> {jsonValue}");
        //}
        //catch (System.Exception ex)
        //{
        //    Debug.LogError($"Error saving high score: {ex.Message}");
        //}

        try
        {
            string uniqueKey = $"HighScore_{playerName}";

            // Create a high score entry and serialize it as JSON
            var highScoreEntry = new HighScoreEntry { Score = score, Kills = kills };
            string jsonValue = JsonUtility.ToJson(highScoreEntry);

            // Verify JSON serialization
            Debug.Log($"Serialized JSON: {jsonValue}");

            // Save the JSON string to Cloud Save
            var data = new Dictionary<string, object> { { uniqueKey, jsonValue } };
            await CloudSaveService.Instance.Data.Player.SaveAsync(data);

            Debug.Log($"High score saved: {uniqueKey} -> {jsonValue}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error saving high score: {ex.Message}");
        }
    }


    //public async Task<List<(string playerName, int score, int kills)>> LoadHighScores()
    //{
    //    var highScores = new List<(string playerName, int score, int kills)>();

    //    try
    //    {
    //        // Load all data from Cloud Save
    //        var savedData = await CloudSaveService.Instance.Data.Player.LoadAllAsync();

    //        foreach (var entry in savedData)
    //        {
    //            if (entry.Key.StartsWith("HighScore_"))
    //            {
    //                string playerName = entry.Key.Replace("HighScore_", ""); // Extract player name
    //                Debug.Log($"value for entry key (playername): {playerName}");

    //                // Safely access and convert the Value property to a string
    //                string rawValue = entry.Value.Value.ToString();

    //                    if (!string.IsNullOrEmpty(rawValue))
    //                    {
    //                        try
    //                        {
    //                            // Debugging rawValue
    //                            Debug.Log($"Raw value for key {entry.Key}: {rawValue}");

    //                            // Deserialize rawValue into a HighScoreEntry object
    //                            HighScoreEntry highScoreEntry = JsonUtility.FromJson<HighScoreEntry>(rawValue);

    //                            // Add to the high scores list
    //                            highScores.Add((playerName, highScoreEntry.Score, highScoreEntry.Kills));
    //                        }
    //                        catch (System.Exception ex)
    //                        {
    //                            Debug.LogWarning($"Invalid high score format for key {entry.Key}: {ex.Message}");
    //                        }
    //                    }
    //                    else
    //                    {
    //                        Debug.LogWarning($"High score entry for key {entry.Key} does not contain a valid string value.");
    //                    }
    //                }
    //                else
    //                {
    //                    Debug.LogWarning($"Invalid Cloud Save item for key {entry.Key}. Could not retrieve the value as a string.");
    //                }
    //            }

    //        // Sort high scores by score in descending order
    //        highScores.Sort((a, b) => b.score.CompareTo(a.score));

    //        // Trim to top 10
    //        if (highScores.Count > 10)
    //        {
    //            highScores = highScores.GetRange(0, 10);
    //        }
    //    }
    //    catch (System.Exception ex)
    //    {
    //        Debug.LogError($"Error loading high scores: {ex.Message}");
    //    }

    //    return highScores;
    //}

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

                    // Use GetAsString to convert the Value property to a string, ToString() does not work here..
                    string rawValue = entry.Value.Value.GetAsString();

                    if (!string.IsNullOrEmpty(rawValue))
                    {
                        try
                        {
                            // Debugging rawValue
                            Debug.Log($"Raw value for key {entry.Key}: {rawValue}");

                            // Deserialize rawValue into a HighScoreEntry object
                            HighScoreEntry highScoreEntry = JsonUtility.FromJson<HighScoreEntry>(rawValue);

                            // Add to the high scores list
                            highScores.Add((playerName, highScoreEntry.Score, highScoreEntry.Kills));
                        }
                        catch (System.Exception ex)
                        {
                            Debug.LogWarning($"Invalid high score format for key {entry.Key}: {ex.Message}");
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"High score entry for key {entry.Key} does not contain a valid string value.");
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
        try
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
        catch(System.Exception ex)
        {
            Debug.LogError($"Error checking for new high score: {ex.Message}");
        }
    }
}