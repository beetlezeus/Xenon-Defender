using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] ParticleSystem enemyExplosionVFX;
    [SerializeField] Transform tempParent;
    [SerializeField] int enemyPoints = 100;

    private PlayerScore playerScore;

    private void Start()
    {
        playerScore = GameObject.Find("Game Manager").GetComponent<PlayerScore>();
    }

    private void OnParticleCollision(GameObject other)
    {
        UpdatScore();
        StartDeathSequence();
    }

    private void UpdatScore()
    {
        playerScore.UpdateEnemyKillScore(enemyPoints);
    }

    void StartDeathSequence()
    {
        ParticleSystem deathVFX = Instantiate(enemyExplosionVFX, transform.position, Quaternion.identity);
        deathVFX.transform.parent = tempParent;
        Destroy(gameObject);
    }
}
