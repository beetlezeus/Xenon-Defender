using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScore : MonoBehaviour
{
    private int enemyKillScore;

    private TMP_Text enemyKillText;
    
    // Start is called before the first frame update
    void Start()
    {
        enemyKillText = GameObject.Find("Score Text").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateEnemyKillScore(int killScore)
    {
        enemyKillScore += killScore;
        enemyKillText.text = "SCORE: " + enemyKillScore.ToString();
    }
}
