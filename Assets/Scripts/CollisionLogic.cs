using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionLogic : MonoBehaviour
{
    private PlayerControls playerControls;
    private SceneManagement sceneManager;
    private bool isPlayerDead = false;
    [SerializeField] ParticleSystem explosionVFX;

    private void Start()
    {
        //playerControls = GameObject.Find("Player Rig").GetComponentInChildren<PlayerControls>();
        playerControls = GetComponent<PlayerControls>();
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

        playerControls.enabled = false;
        explosionVFX.Play();
        sceneManager.RestartLevelWithDelay();
    }

    public bool PlayerCrashed()
    {

        return isPlayerDead;
    }
}
