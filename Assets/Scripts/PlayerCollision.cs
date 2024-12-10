using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerShooting playerShooting;
    private SceneManagement sceneManager;
    private AudioSource audioSource;
    [SerializeField] AudioClip explosionAudioClip;
    [SerializeField] ParticleSystem explosionVFX;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerShooting = GetComponent<PlayerShooting>();
        audioSource = GetComponent<AudioSource>();
        sceneManager = GameObject.Find("Game Manager").GetComponent<SceneManagement>();
        
    }


    void OnTriggerEnter(Collider other)
    {
        if (!PersistentGameManager.Instance.isCrashed)
        {
            StartCrashSequence();
            //PersistentGameManager.Instance.DecrementLives();
            PersistentGameManager.Instance.StartDeathSequence();
        }

    }

    void StartCrashSequence()
    {
        playerMovement.enabled = false;
        playerShooting.enabled = false;

        explosionVFX.Play();
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        audioSource.PlayOneShot(explosionAudioClip);

        sceneManager.RestartLevelWithDelay();
    }
}
