using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* 
  This script handles a player's collision for the player's ship
  -- disables movement and shooting
  -- plays explosion VFX and audio when crash occurs
  -- checks if player still has remainig lives or has reached finish point
 */
public class PlayerCollision : MonoBehaviour
{
    // References to components managing movement, shooting, scene transitions, and audio
    private PlayerMovement playerMovement;
    private PlayerShooting playerShooting;
    private AudioSource audioSource;

    // Audio and VFX for crash/explosion
    [SerializeField] AudioClip explosionAudioClip;
    [SerializeField] ParticleSystem explosionVFX;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>(); // assign player movement component
        playerShooting = GetComponent<PlayerShooting>(); // assign player shooting component
        audioSource = GetComponent<AudioSource>();       // assign audio source for SFX
        
    }

    // Called when any collider enters the player's trigger collider
    void OnTriggerEnter(Collider other)
    {
        // if player already dead do nothing
        if (PersistentGameManager.Instance.isDead)
        {
            return;
        }
        // if collision is with Finish line set LevelCleared as true, show transition screen
        if (other.gameObject.tag == "Finish")
        {
            PersistentGameManager.Instance.levelCleared = true;
            _ = PersistentGameManager.Instance.ShowTransitionScreen(false);
            Time.timeScale = 0f;
            return;
        }

        // if player hasn't crashed and level is not cleared start the crash sequence
        // notify game Manager to handle death sequence
        if (!PersistentGameManager.Instance.isCrashed && !PersistentGameManager.Instance.levelCleared)
        {
            StartCrashSequence();
            PersistentGameManager.Instance.StartDeathSequence();
        }

    }

    // Disables player controls (movement , shooting)
    // plays explosion VFX & SFX
    void StartCrashSequence()
    {
        // disable controls
        playerMovement.enabled = false;
        playerShooting.enabled = false;

        // play VFX
        explosionVFX.Play();
        // stop audio source then play the crash SFX
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        audioSource.PlayOneShot(explosionAudioClip);
    }
}
