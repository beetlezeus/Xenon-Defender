using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private float horizontalMovement;
    private float verticalMovement;
    [SerializeField] InputAction movement;
    [SerializeField] InputAction fire;
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
        horizontalMovement = movement.ReadValue<Vector2>().x;
        verticalMovement = movement.ReadValue<Vector2>().y;

        //float horizontalMovement = Input.GetAxis("Horizontal");
        //float verticalMovement = Input.GetAxis("Vertical");

        if (horizontalMovement != 0 || verticalMovement != 0)
        {
            Debug.Log("horizontal movement" + horizontalMovement);
            Debug.Log("vertical movement" + verticalMovement);
        }

    }
}
