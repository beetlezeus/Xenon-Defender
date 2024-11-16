using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScore : MonoBehaviour
{
    private int enemyHitScore = 0;
    private int enemyKillCount = 0;

    private TMP_Text enemyHitText;
    private TMP_Text enemyKillText;
    
    // Start is called before the first frame update
    void Start()
    {
        enemyHitText = GameObject.Find("Score Text").GetComponent<TMP_Text>();
        enemyKillText = GameObject.Find("Enemy Kill Text").GetComponent<TMP_Text>();
    }

    public void UpdateEnemyHitScore(int hitScore)
    {
        enemyHitScore += hitScore;
        enemyHitText.text = "SCORE: " + enemyHitScore.ToString();
    }

    public void UpdateEnemyKillCount()
    {
        enemyKillCount ++;
        enemyKillText.text = "KILLS: " + enemyKillCount.ToString();
    }
}
