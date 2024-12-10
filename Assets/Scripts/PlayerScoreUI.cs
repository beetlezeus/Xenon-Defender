using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScoreUI : MonoBehaviour
{
    //private int enemyHitScore;
    //private int enemyKillCount;

    //private TMP_Text enemyHitText;
    //private TMP_Text enemyKillText;

    //void Start()
    //{
    //    // Reset scores at start of scene
    //    ResetScores();
    //    // Initialize UI references
    //    AssignUITextObjects();
    //}

    //public void ResetScores()
    //{
    //    enemyHitScore = 0;
    //    enemyKillCount = 0;
    //}

    //void AssignUITextObjects()
    //{
    //    // Safely find and assign the text elements, prevents NullReferenceException
    //    enemyHitText = GameObject.Find("Score Text")?.GetComponent<TMP_Text>();
    //    enemyKillText = GameObject.Find("Enemy Kill Text")?.GetComponent<TMP_Text>();

    //    // If either reference is missing, log a warning
    //    if (enemyHitText == null)
    //        Debug.LogWarning("Enemy Hit Text UI element not found.");
    //    if (enemyKillText == null)
    //        Debug.LogWarning("Enemy Kill Text UI element not found.");
    //}

    //public void UpdateEnemyHitScore(int hitScore)
    //{
    //    enemyHitScore += hitScore;

    //    if (enemyHitText == null)
    //    {
    //        AssignUITextObjects(); // Retry finding the text element if missing
    //    }

    //    if (enemyHitText != null)
    //    {
    //        enemyHitText.text = "SCORE: " + enemyHitScore.ToString();
    //    }
    //}

    //public void UpdateEnemyKillCount()
    //{
    //    enemyKillCount++;

    //    if (enemyKillText == null)
    //    {
    //        AssignUITextObjects(); // Retry finding the text element if missing
    //    }

    //    if (enemyKillText != null)
    //    {
    //        enemyKillText.text = "KILLS: " + enemyKillCount.ToString();
    //    }
    //}

}
