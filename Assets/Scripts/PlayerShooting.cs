using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

/*
  This script manages the player's shooting mechanics
  -- reads input from the new Unity Input System
  -- toggles firing on / off based on input value
  -- optionally can play / stop firing sounds (commented out in current version)
 */
public class PlayerShooting : MonoBehaviour
{

    private float fireInput; // raw input value for firing

    // reference to audio source & audio clip for shooting audio (not used)
    private AudioSource audioSource; 
    [SerializeField] AudioClip fireSound;

    [Header("Input Mapping and Shooting Settings")]
    [SerializeField] GameObject[] fireBeams;      // array of beam particles
    [SerializeField] public InputAction fire;     // input action for setting bindings for player shooting


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // enables the fire input action when script becomes active
    void OnEnable()
    {
        fire.Enable();
    }

    // disables fire input action when script becomes inactive
    void OnDisable()
    {
        fire.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        ShipFiring();
    }

    // reads the firing input and toggles fire emission accordingly
    // can also handle audio (currently not in use)
    void ShipFiring()
    {
        fireInput = fire.ReadValue<float>(); // read the fire input as a float

        if (fireInput > 0.2)
        {
            ToggleFireBeams(true);
            //PlayFireSound();
        }
        else
        {
            ToggleFireBeams(false);
            //StopFireSound();
        }
    }

    // enable or disable the emissions of each of the particle systems shooting lasers
    void ToggleFireBeams(bool isShooting)
    {
        // iterate over the particle systems, for each of them toggle emission based on isShooting
        foreach (GameObject fireBeam in fireBeams)
        {
            var emission = fireBeam.GetComponent<ParticleSystem>().emission;

            emission.enabled = isShooting;
        }
    }

    //private void PlayFireSound()
    //{
    //    if (!audioSource.isPlaying)
    //    {
    //        audioSource.PlayOneShot(fireSound);
    //    }
    //}

    //private void StopFireSound()
    //{
    //    if (audioSource.isPlaying)
    //    {
    //        audioSource.Stop();
    //    }
    //}
}
