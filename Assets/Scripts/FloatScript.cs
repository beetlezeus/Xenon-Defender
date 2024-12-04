using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatScript : MonoBehaviour
{
    [SerializeField] float amplitude = 6f; // Oscillation height
    [SerializeField] float frequency = 2f; // Oscillation speed
    [SerializeField] float noiseScale = 0.8f; // Scale for Perlin Noise contribution
    [SerializeField] float offset = 4; // Random offset for Perlin Noise

    private Vector3 startPos;

    void Start()
    {
        // Store the starting position
        startPos = transform.position;

        // Randomize parameters for organic motion
        amplitude = Random.Range(5f, 10f);
        frequency = Random.Range(1.5f, 3f);
        offset = Random.Range(0f, 100f); // Ensure variation across objects
    }

    void Update()
    {
        // Calculate Perlin-based Y offset
        float noiseOffset = Mathf.PerlinNoise(Time.time + offset, 0f) * noiseScale;

        // Calculate Sinusoidal motion
        float waveOffset = amplitude * Mathf.Sin(Time.time * frequency);

        // Combine motions
        float newYPos = waveOffset + noiseOffset;

        // Apply motion to the object
        transform.position = startPos + new Vector3(0, newYPos, 0);
    }

}