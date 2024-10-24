using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private float xMovementInput;
    private float yMovementInput;
    private float pitch;
    private float yaw;
    private float roll;
    
    [SerializeField] int controlSpeed = 32; // variable for adjusting movement speed on player input
    [SerializeField] float xRange = 12.0f;    // range of movement to prevent ship from going offscreen
    [SerializeField] float yRange = 7.0f;    // range of movement to prevent ship from going offscreen

    [SerializeField] float positionPitchCoefficient = -2.0f;
    [SerializeField] float controlPitchCoefficient = -15.0f;

    [SerializeField] float positionYawCoefficient = 2.0f;
    [SerializeField] float controlRollCoefficient = -30.0f;

    [SerializeField] float rotationSpeed = 10f;

    [SerializeField] InputAction movement; //input action for setting bindings for player movement
    [SerializeField] InputAction fire;     // input action for setting bindings for player shooting

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

        yaw = transform.localPosition.x * positionYawCoefficient; ;

        roll = xMovementInput * controlRollCoefficient;

        //transform.localRotation = Quaternion.Euler(pitch, yaw, roll);

        Quaternion targetRotation = Quaternion.Euler(pitch, 0f, roll);

        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);
        
    }
}
