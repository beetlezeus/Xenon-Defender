using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] ParticleSystem enemyExplosionVFX;
    [SerializeField] ParticleSystem enemyHitVFX;
    [SerializeField] Transform tempParent;
    [SerializeField] int enemyHealth = 1;
    [SerializeField] int enemyScorePoints = 100;

    private PlayerScore playerScore;

    private void Start()
    {
        playerScore = GameObject.Find("Game Manager").GetComponent<PlayerScore>();
    }

    private void OnParticleCollision(GameObject other)
    {
        DecrementEnemyHealth(1);
        UpdatScore();

        if (enemyHealth <= 0)
        {
            StartDeathSequence();
        }
    }

    private void UpdatScore()
    {
        playerScore.UpdateEnemyKillScore(enemyScorePoints);
    }

    public void DecrementEnemyHealth(int damageAmount)
    {
        ParticleSystem hitVFX = Instantiate(enemyHitVFX, transform.position, Quaternion.identity);
        hitVFX.transform.parent = tempParent;
        enemyHealth -= damageAmount;
    }

    void StartDeathSequence()
    {
        ParticleSystem deathVFX = Instantiate(enemyExplosionVFX, transform.position, Quaternion.identity);
        deathVFX.transform.parent = tempParent;
        Destroy(gameObject);
    }
}
