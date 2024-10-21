using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private float xMovement;
    private float yMovement;
    [SerializeField] int offsetBoost = 38; // variable for adjusting movement speed on player input
    [SerializeField] float xRange = 12.0f;    // range of movement to prevent ship from going offscreen
    [SerializeField] float yRange = 7.0f;    // range of movement to prevent ship from going offscreen

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
        xMovement = movement.ReadValue<Vector2>().x;
        yMovement = movement.ReadValue<Vector2>().y;

        float xOffset = xMovement * Time.deltaTime * offsetBoost;
        float yOffset = yMovement * Time.deltaTime * offsetBoost;

        float offsetXPos = transform.localPosition.x + xOffset;
        float clampedXPos = Mathf.Clamp(offsetXPos, -xRange, xRange);

        float offsetYPos = transform.localPosition.y + yOffset;
        float clampedYPos = Mathf.Clamp(offsetYPos, -yRange, yRange);

        //float horizontalMovement = Input.GetAxis("Horizontal");
        //float verticalMovement = Input.GetAxis("Vertical");

        //if (xMovement != 0 || yMovement != 0)
        //{
        //    Debug.Log("horizontal movement" + xMovement);
        //    Debug.Log("vertical movement" + yMovement);
        //}

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);

    }
}
