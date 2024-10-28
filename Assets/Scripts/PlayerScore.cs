using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    private int enemyKillScore;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateEnemyKillScore(int killScore)
    {
        enemyKillScore += killScore;
        Debug.Log("Player Score is " + enemyKillScore);
    }
}
