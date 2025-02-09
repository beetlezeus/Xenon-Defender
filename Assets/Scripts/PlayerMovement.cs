using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

/* 
 * This script handles a player's ship movement and rotation in a 2D space (x and y axes).
 * Movement is clamped within specified ranges to prevent going off-screen.
 * Rotation is influenced by both the ship's position on-screen and the user's control input.
 */

public class PlayerMovement : MonoBehaviour
{
    // raw input values for horizontal (x) and vertical (y) movement
    private float xMovementInput;
    private float yMovementInput;
    // Rotation angles around pitch (X-axis), yaw (Y-axis), and roll (Z-axis)
    private float pitch;
    private float yaw;
    private float roll;

    [Header("Ship Movement Settings")]
    [SerializeField] int controlSpeed = 32; // Movement speed factor
    [SerializeField] float xRange = 12.0f;  // Horizontal boundary
    [SerializeField] float yRange = 7.0f;    // Vertical boundary
    [SerializeField] float rotationSpeed = 10f; // How fast the ship rotates to its target orientation

    [Header("Screen Position & Input movement factors")]
    [SerializeField] float positionPitchCoefficient = -2.0f; // How much vertical position affects pitch
    [SerializeField] float controlPitchCoefficient = -15.0f; // How much vertical input affects pitch
    [SerializeField] float positionYawCoefficient = 2.0f;    // How much horizontal position affects yaw
    [SerializeField] float controlYawCoefficient = 2.0f;
    [SerializeField] float controlRollCoefficient = -30.0f;  // How much horizontal input affects roll


    [Header("Movement Input Mapping")]
    [SerializeField] public InputAction movement; //input action for setting bindings for player movement

    // Called when the script is enabled. Enables the movement action.
    void OnEnable()
    {
        movement.Enable();
    }

    // Called when the script is disabled. Disables the movement action.
    void OnDisable()
    {
        movement.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        ShipPosition();
        ShipRotation();
    }

    // Adjusts the local position of the ship based on input, clamped within boundaries
    void ShipPosition()
    {
        // Read player input
        xMovementInput = movement.ReadValue<Vector2>().x;
        yMovementInput = movement.ReadValue<Vector2>().y;

        // calculate movement offsets
        float xOffset = xMovementInput * Time.deltaTime * controlSpeed;
        float yOffset = yMovementInput * Time.deltaTime * controlSpeed;

        // calculate new X position & clamp
        float offsetXPos = transform.localPosition.x + xOffset;
        float clampedXPos = Mathf.Clamp(offsetXPos, -xRange, xRange);

        // calculate new Y position & clamp
        float offsetYPos = transform.localPosition.y + yOffset;
        float clampedYPos = Mathf.Clamp(offsetYPos, -yRange, yRange);

        // apply clamped position
        transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
    }

    // Calculates and applies the rotation based on the ship's position and input
    void ShipRotation()
    {
        // Calculate pitch from position and vertical input
        float positionPitch = transform.localPosition.y * positionPitchCoefficient;
        float controlPitch = yMovementInput * controlPitchCoefficient;
        pitch = positionPitch + controlPitch;

        // Calculate yaw from position and horizontal input
        float positionYaw = transform.localPosition.x * positionYawCoefficient;
        float controlYaw = xMovementInput * controlYawCoefficient;

        yaw = positionYaw + controlYaw;

        // Roll is directly influenced by horizontal input
        roll = (xMovementInput) * controlRollCoefficient;

        // Create a target rotation based on calculated angles
        Quaternion targetRotation = Quaternion.Euler(pitch, yaw , roll);

        // Smoothly rotate toward the target rotation
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
