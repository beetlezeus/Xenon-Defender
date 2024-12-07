using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionLogic : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerShooting playerShooting;
    private SceneManagement sceneManager;
    private bool isPlayerDead = false;
    private AudioSource audioSource;
    [SerializeField] AudioClip explosionAudioClip;
    [SerializeField] ParticleSystem explosionVFX;

    private void Start()
    {
        //playerControls = GameObject.Find("Player Rig").GetComponentInChildren<PlayerControls>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooting = GetComponent<PlayerShooting>();
        audioSource = GetComponent<AudioSource>();
        sceneManager = GameObject.Find("Game Manager").GetComponent<SceneManagement>();
        
    }


    void OnTriggerEnter(Collider other)
    {
        StartCrashSequence();
        isPlayerDead = true;

    }

    void StartCrashSequence()
    {
        //playerControls.enabled = false;
        //explosionVFX.transform.parent = null; // REMOVE IF DONT WANT SPACESHIP TO DISAPPEAR
        //explosionVFX.Play();
        //Destroy(gameObject); // REMOVE IF DON'T WANT SPACESHIP TO DISAPPEAR
        //sceneManager.RestartLevelWithDelay();

        playerMovement.enabled = false;
        playerShooting.enabled = false;
        explosionVFX.Play();
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(explosionAudioClip);
        }
        else
        {
            audioSource.PlayOneShot(explosionAudioClip);
        }
        sceneManager.RestartLevelWithDelay();
    }

    public bool PlayerCrashed()
    {

        return isPlayerDead;
    }
}
