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

    private bool enemyDead = false;

    private void Start()
    {
        playerScore = GameObject.Find("Game Manager").GetComponent<PlayerScore>();
        tempParent = GameObject.Find("Spawn At Runtime").transform;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (enemyDead)
        {
            return;
        }

        ProcessHit();

        if (enemyHealth <= 0)
        {
            StartDeathSequence();
        }
    }


    private void ProcessHit()
    {
        enemyHealth -= 1;
        UpdatScore(enemyHitPoints);
        PlayHitVFX();
    }

    private void UpdatScore(int points)
    {
        playerScore.UpdateEnemyHitScore(points);
    }

    private void PlayHitVFX()
    {
        ParticleSystem hitVFX = Instantiate(enemyHitVFX, transform.position, Quaternion.identity);
        hitVFX.transform.parent = tempParent;
    }

    void PlayDeathVFX()
    {
        ParticleSystem deathFX = Instantiate(enemyExplosionFX, transform.position, Quaternion.identity);
        deathFX.transform.parent = tempParent;
    }

    void StartDeathSequence()
    {
        enemyDead = true;
        GetComponent<Collider>().enabled = false;
        playerScore.UpdateEnemyKillCount();
        UpdatScore(enemyKillPoints);
        PlayDeathVFX();
        Destroy(gameObject);
    }
}
