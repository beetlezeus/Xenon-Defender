using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float xMovementInput;
    private float yMovementInput;
    private float pitch;
    private float yaw;
    private float roll;

    [Header("Ship Movement Settings")]
    [SerializeField] int controlSpeed = 32; // variable for adjusting movement speed on player input
    [SerializeField] float xRange = 12.0f;    // range of movement to prevent ship from going offscreen
    [SerializeField] float yRange = 7.0f;    // range of movement to prevent ship from going offscreen
    [SerializeField] float rotationSpeed = 10f;

    [Header("Screen Position & Input movement factors")]
    [SerializeField] float positionPitchCoefficient = -2.0f;
    [SerializeField] float controlPitchCoefficient = -15.0f;
    [SerializeField] float positionYawCoefficient = 2.0f;
    [SerializeField] float controlYawCoefficient = 2.0f;
    [SerializeField] float controlRollCoefficient = -30.0f;


    [Header("Movement Input Mapping")]
    [SerializeField] public InputAction movement; //input action for setting bindings for player movement

    // Start is called before the first frame update
    void Start()
    {
  
    }

    void OnEnable()
    {
        movement.Enable();
    }

    void OnDisable()
    {
        movement.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (PersistentGameManager.Instance.isDead)
        {
            return;
        }
        ShipPosition();
        ShipRotation();
    }

    void ShipPosition()
    {
        xMovementInput = movement.ReadValue<Vector2>().x;
        yMovementInput = movement.ReadValue<Vector2>().y;

        float xOffset = xMovementInput * Time.deltaTime * controlSpeed;
        float yOffset = yMovementInput * Time.deltaTime * controlSpeed;

        float offsetXPos = transform.localPosition.x + xOffset;
        float clampedXPos = Mathf.Clamp(offsetXPos, -xRange, xRange);

        float offsetYPos = transform.localPosition.y + yOffset;
        float clampedYPos = Mathf.Clamp(offsetYPos, -yRange, yRange);


        transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
    }

    void ShipRotation()
    {
        float positionPitch = transform.localPosition.y * positionPitchCoefficient;
        float controlPitch = yMovementInput * controlPitchCoefficient;
        pitch = positionPitch + controlPitch;

        float positionYaw = transform.localPosition.x * positionYawCoefficient;
        float controlYaw = xMovementInput * controlYawCoefficient;

        yaw = positionYaw + controlYaw;

        roll = xMovementInput * controlRollCoefficient;

        //transform.localRotation = Quaternion.Euler(pitch, yaw, roll);

        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, roll);

        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);
        
    }
}
