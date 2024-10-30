using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] ParticleSystem enemyExplosionFX;
    [SerializeField] ParticleSystem enemyHitVFX;
    [SerializeField] int enemyHealth = 1;      // how many hits to kill enemy
    [SerializeField] int enemyHitPoints = 100; // how much to award for each enemy hit
    [SerializeField] int enemyKillPoints = 500; // how much to award for a kill

    private Transform tempParent;
    private PlayerScore playerScore;
    //private Rigidbody enemyRB;

    private void Start()
    {
        playerScore = GameObject.Find("Game Manager").GetComponent<PlayerScore>();
        //AddRB();
        tempParent = GameObject.Find("Spawn At Runtime").transform;
    }

    //private void AddRB()
    //{
    //    enemyRB = gameObject.AddComponent<Rigidbody>();
    //    enemyRB.useGravity = false;
    //}

    private void OnParticleCollision(GameObject other)
    {
        DecrementEnemyHealth(1);
        UpdatScore(enemyHitPoints);

        if (enemyHealth <= 0)
        {
            StartDeathSequence();
        }
    }

    private void UpdatScore(int points)
    {
        playerScore.UpdateEnemyHitScore(points);
    }

    private void UpdateKillCount()
    {
        playerScore.UpdateEnemyKillCount();
    }

    public void DecrementEnemyHealth(int damageAmount)
    {
        ParticleSystem hitVFX = Instantiate(enemyHitVFX, transform.position, Quaternion.identity);
        hitVFX.transform.parent = tempParent;
        enemyHealth -= damageAmount;
    }

    void StartDeathSequence()
    {
        UpdatScore(enemyKillPoints);
        UpdateKillCount();
        ParticleSystem deathFX = Instantiate(enemyExplosionFX, transform.position, Quaternion.identity);
        deathFX.transform.parent = tempParent;
        Destroy(gameObject);
    }
}
