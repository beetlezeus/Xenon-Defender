using Unity.Services.CloudSave;
using Unity.Services.Leaderboards;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor.Rendering;

[System.Serializable]
public class HighScoreEntry
{ 
    public int Kills;
    public string PlayerName;
}

public class HighscoreManager : MonoBehaviour
{
    public async Task SaveHighScore(string playerName, int score, int kills)
    {
        ///////// OLD IMPLEMENTATION WITH CLOUD SAVE (TIED TO AUTHENTICATED USER) LOCAL HIGH SCORES ///////
        /// THIS CAN BE USED FOR IMPLEMENTING LOCAL HIGH-SCORES IN ADDITON TO GLOBAL LEADERBOARD //////////
        /// 
        //try
        //{
        //    string uniqueKey = $"HighScore_{playerName}";

        //    // Create a high score entry and serialize it as JSON
        //    var highScoreEntry = new HighScoreEntry { Score = score, Kills = kills };
        //    string jsonValue = JsonUtility.ToJson(highScoreEntry);

        //    // Verify JSON serialization
        //    Debug.Log($"Serialized JSON: {jsonValue}");

        //    // Save the JSON string to Cloud Save
        //    var data = new Dictionary<string, object> { { uniqueKey, jsonValue } };
        //    await CloudSaveService.Instance.Data.Player.SaveAsync(data);

        //    Debug.Log($"High score saved: {uniqueKey} -> {jsonValue}");
        //}
        //catch (System.Exception ex)
        //{
        //    Debug.LogError($"Error saving high score: {ex.Message}");
        //}

        /////////// NEW IMPLEMENTATION, GLOBAL LEADERBOARD  /////////////////////////////
        ///
        /// REFERENCE: https://docs.unity.com/ugs/en-us/manual/leaderboards/manual/add-new-score /////////////
        /// 
        try
        {
            var metadata = new Dictionary<string, string>() { { "PlayerName", playerName }, { "Kills", kills.ToString() } };

            await LeaderboardsService.Instance.AddPlayerScoreAsync("HighScore_LeaderBoard", score, new AddPlayerScoreOptions { Metadata = metadata });
            Debug.Log($"Score {score} : kills {kills} submitted for player {playerName}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error submitting score: {ex.Message}");
        }
    }


    public async Task<List<(string playerName, int score, int kills)>> LoadHighScores()
    {
        ///////// OLD IMPLEMENTATION WITH CLOUD SAVE (TIED TO AUTHENTICATED USER) LOCAL HIGH SCORES ///////
        /// THIS CAN BE USED FOR IMPLEMENTING LOCAL HIGH-SCORES IN ADDITON TO GLOBAL LEADERBOARD //////////

        //var highScores = new List<(string playerName, int score, int kills)>();
        //try
        //{
        //    // Load all data from Cloud Save
        //    var savedData = await CloudSaveService.Instance.Data.Player.LoadAllAsync();

        //    foreach (var entry in savedData)
        //    {
        //        if (entry.Key.StartsWith("HighScore_"))
        //        {
        //            string playerName = entry.Key.Replace("HighScore_", ""); // Extract player name

        //            // Use GetAsString to convert the Value property to a string, ToString() does not work here..
        //            string rawValue = entry.Value.Value.GetAsString();

        //            if (!string.IsNullOrEmpty(rawValue))
        //            {
        //                try
        //                {
        //                    // Debugging rawValue
        //                    Debug.Log($"Raw value for key {entry.Key}: {rawValue}");

        //                    // Deserialize rawValue into a HighScoreEntry object
        //                    HighScoreEntry highScoreEntry = JsonUtility.FromJson<HighScoreEntry>(rawValue);

        //                    // Add to the high scores list
        //                    highScores.Add((playerName, highScoreEntry.Score, highScoreEntry.Kills));
        //                }
        //                catch (System.Exception ex)
        //                {
        //                    Debug.LogWarning($"Invalid high score format for key {entry.Key}: {ex.Message}");
        //                }
        //            }
        //            else
        //            {
        //                Debug.LogWarning($"High score entry for key {entry.Key} does not contain a valid string value.");
        //            }
        //        }
        //    }

        //    // Sort high scores by score in descending order
        //    highScores.Sort((a, b) => b.score.CompareTo(a.score));

        //    // Trim to top 10
        //    if (highScores.Count > 10)
        //    {
        //        highScores = highScores.GetRange(0, 10);
        //    }
        //}
        //catch (System.Exception ex)
        //{
        //    Debug.LogError($"Error loading high scores: {ex.Message}");
        //}

        //return highScores;


        /////////// NEW IMPLEMENTATION, GLOBAL LEADERBOARD  /////////////////////////////
        ///
        //REFERENCE: https://docs.unity.com/ugs/en-us/manual/leaderboards/manual/get-score ////////////////
        ///

        var highScores = new List<(string playerName, int score, int kills)>();

        try
        {
            // Retrieve the top 10 leaderboard entries
            var scoreOptions = new GetScoresOptions { IncludeMetadata = true, Limit = 10 };
            var leaderboardScores = await LeaderboardsService.Instance.GetScoresAsync("HighScore_LeaderBoard", scoreOptions);

            foreach (var entry in leaderboardScores.Results)
            {
                // Extract the chosen name & kills from metadata
                int kills = 0;
                string playerName = "";

                if (entry.Metadata != null)
                {
                    Debug.Log($"Raw Metadata for entry {entry.Metadata}");
                    Debug.Log($"Raw Metadata type: {entry.Metadata.GetType()}");

                    try   // NEEDS DEBBUGGING. CHECK OUT OLD IMPLEMENTATION FOR POINTERS
                    {
                        HighScoreEntry highScoreEntry = JsonUtility.FromJson<HighScoreEntry>(entry.Metadata);

                        // Extract PlayerName from metadata
                        try
                        {
                            playerName = highScoreEntry.PlayerName;
                            Debug.Log($"Successfully Loaded name:  {playerName}");


                        }
                        catch (System.Exception ex)
                        {
                            Debug.Log($"Failed to extract player name. Error: {ex.Message}");
                        }

                        // Extract Kills from metadata
                        try
                        {
                            kills = highScoreEntry.Kills;
                            Debug.Log($"Successfully Loaded Kills:  {kills}");
                        }
                        catch (System.Exception ex)
                        {
                            Debug.Log($"Failed to extract Kills. Error: {ex.Message}");
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError($"Failed to parse metadata for entry: {entry.PlayerName}. Error: {ex.Message}");
                    }
                }

                highScores.Add((playerName, (int)entry.Score, kills));
                Debug.Log($"Loaded entry - {playerName}, Score {entry.Score} , Kills {kills}");
            }

            Debug.Log("Successfully loaded leaderboard scores.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error loading leaderboard scores: {ex.Message}");
        }

        return highScores;
    }



    public async Task CheckForNewHighScore(int score)
    {
        try
        {
            //get player's playerID and load the highScores
            var playerId = AuthenticationService.Instance.PlayerId;
            var leaderboardScores = await LeaderboardsService.Instance.GetScoresAsync("HighScore_LeaderBoard");

            var highScores = await LoadHighScores();

            //check if player already has a high score saved
            foreach (var entry in leaderboardScores.Results)
            {
                if(entry.PlayerId == playerId)
                {
                    if(entry.Score > score)
                    {
                        Debug.Log($"Score of {score} didn't beat existing highscore: {entry.Score}");
                        return; // end execution here
                    }
                }
            }
            // Check if the new score qualifies for the top 10 by comparing it to last element in list
            if (highScores.Count < 10 || score > highScores[^1].score)
            {
                Debug.Log("New High Score!");
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