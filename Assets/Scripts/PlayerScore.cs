using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScore : MonoBehaviour
{
    private int enemyHitScore;
    private int enemyKillCount;

    private TMP_Text enemyHitText;
    private TMP_Text enemyKillText;
    
    // Start is called before the first frame update
    void Start()
    {
        enemyHitText = GameObject.Find("Score Text").GetComponent<TMP_Text>();
        enemyKillText = GameObject.Find("Enemy Kill Text").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateEnemyHitScore(int hitScore)
    {
        enemyHitScore += hitScore;
        enemyHitText.text = "SCORE: " + enemyHitScore.ToString();
    }

    public void UpdateEnemyKillCount()
    {
        enemyKillCount += 1;
        enemyKillText.text = "KILLS: " + enemyKillCount.ToString();
    }
}
