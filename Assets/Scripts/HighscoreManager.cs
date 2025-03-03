using Unity.Services.CloudSave;
using Unity.Services.Leaderboards;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor.Rendering;


[System.Serializable]

// A simple data structure to hold JSON-deserialized high score metadata containing player kills and player initials string
public class HighScoreEntry
{ 
    public int Kills;
    public string PlayerName;
}

/*
   This class integrates with Unity's Leaderboards Service to save, retrieve, and check high scores. It allows us to:
   -- Submit a player's score and metadata
   -- Load the top scores from the leaderboard
   -- Determine if a new score is higher than previously saved scores
 */
public class HighscoreManager : MonoBehaviour
{

    /// REFERENCE: https://docs.unity.com/ugs/en-us/manual/leaderboards/manual/add-new-score
    //Saves a new high score the a specified Leaderboard ("Highscore_Leaderboard")
    // Metadata (player name , kills) is included so it can retrieved later
    public async Task SaveHighScore(string playerName, int score, int kills)
    {
        try
        {
            // create a dictionary for storing metadata as key-value pairs
            var metadata = new Dictionary<string, string>() { { "PlayerName", playerName }, { "Kills", kills.ToString() } };

            // submit the player's score and metadata to the LeaderboardService
            await LeaderboardsService.Instance.AddPlayerScoreAsync("HighScore_LeaderBoard", score, new AddPlayerScoreOptions { Metadata = metadata });
            Debug.Log($"Score {score} : kills {kills} submitted for player {playerName}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error submitting score: {ex.Message}");
        }
    }


  
    //REFERENCE: https://docs.unity.com/ugs/en-us/manual/leaderboards/manual/get-score
    // Loads the to 10 high scores from the "HighScore_Leaderboard"
    // Returns a list of tuples containing player name, score, kills
    public async Task<List<(string playerName, int score, int kills)>> LoadHighScores()
    {
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

                    try   // Attempt to create highScoreEntry object from the entry.Metadata JSON
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

                // Add the extracted or default values to the list
                highScores.Add((playerName, (int)entry.Score, kills));
                Debug.Log($"Loaded entry - {playerName}, Score {entry.Score} , Kills {kills}");
            }

            Debug.Log("Successfully loaded leaderboard scores.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error loading leaderboard scores: {ex.Message}");
        }

        // Return the list of scores
        return highScores;
    }

    // Checks if the player's new high score qualifies for a top 10 entry
    // Checks if the player's new high score is higher than their previously saved score
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