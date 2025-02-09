using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{

    private float fireInput;
    private AudioSource audioSource;
    [SerializeField] AudioClip fireSound;

    [Header("Input Mapping and Shooting Settings")]
    [SerializeField] GameObject[] fireBeams;
    [SerializeField] public InputAction fire;     // input action for setting bindings for player shooting


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        fire.Enable();
    }

    void OnDisable()
    {
        fire.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        ShipFiring();
    }

    void ShipFiring()
    {
        fireInput = fire.ReadValue<float>();

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

    void ToggleFireBeams(bool isShooting)
    {
        foreach (GameObject fireBeam in fireBeams)
        {
            var emission = fireBeam.GetComponent<ParticleSystem>().emission;

            emission.enabled = isShooting;
        }
    }

    private void PlayFireSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(fireSound);
        }
    }

    private void StopFireSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
