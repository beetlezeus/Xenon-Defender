using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
  This is the dedicated script for handling Enemy behavior.
  -- handles enemy health, hit logic, scoring, and explosion effects
  -- Relies on the global PersistentManager to update the score and track kills during gameplay
 */
public class EnemyScript : MonoBehaviour
{
    // Particle effect references for explosion and being hit
    [SerializeField] ParticleSystem enemyExplosionFX;
    [SerializeField] ParticleSystem enemyHitVFX;

    // enemy health & scoring values
    [SerializeField] int enemyHealth = 1;      // how many hits to kill enemy
    [SerializeField] int enemyHitPoints = 100; // how much to award for each enemy hit
    [SerializeField] int enemyKillPoints = 500; // how much to award for a kill

    // Temporary parent for spawned VFX
    private Transform tempParent;

    // flag for tracking if enemy has already been killed, prevents double triggers
    private bool enemyDead = false;

    private void Start()
    {
        // get references needed for spawning VFX
        tempParent = GameObject.Find("Spawn At Runtime").transform;
    }

    // Called when a particle from the player's laser beams collides with the enemy ship
    private void OnParticleCollision(GameObject other)
    {
        // if enemy is dead do nothing, return. prevents inaccurate hits
        if (enemyDead)
        {
            return;
        }

        // call ProcessHit to process the enemy hit
        ProcessHit();

        // if enemy is dead, start death sequence
        if (enemyHealth <= 0)
        {
            StartDeathSequence();
        }
    }

    // Performs actions to process enemy hits
    private void ProcessHit()
    {
        enemyHealth -= 1; // decrement enemy health
        UpdatScore(enemyHitPoints);  // update the player's score 
        PlayHitVFX();  // show hit / impact VFX
    }

    // updates player score using the PersistentGameManager
    private void UpdatScore(int points)
    {
   
        PersistentGameManager.Instance.UpdateEnemyHitScore(points);
    }

    // displays the enemy hit VFX
    private void PlayHitVFX()
    {
        //Instantiate the enemy hit effect at the enemy's current position
        ParticleSystem hitVFX = Instantiate(enemyHitVFX, transform.position, Quaternion.identity);

        //parent it to tempParent for organized hierarchy
        hitVFX.transform.parent = tempParent;
    }

    // displays the enemy death VFX
    void PlayDeathVFX()
    {
        //Instantiate the enemy death effect at the enemy's current position
        ParticleSystem deathFX = Instantiate(enemyExplosionFX, transform.position, Quaternion.identity);
        //parent it to tempParent for organized hierarchy
        deathFX.transform.parent = tempParent;
    }

    // Called after health reaches zero, marking the enemy as dead
    void StartDeathSequence()
    {
        enemyDead = true; // set enemyDead flag to true
        GetComponent<Collider>().enabled = false; // disable collider
        
        PersistentGameManager.Instance.UpdateEnemyKillCount(); // notify PersistentGameManager to update kill counts
        UpdatScore(enemyKillPoints); // update score with kill points
        PlayDeathVFX(); // display death effcts and audio
        Destroy(gameObject); // destroy the enemy game object
    }
}
