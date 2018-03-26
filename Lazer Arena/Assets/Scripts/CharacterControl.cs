using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterControl : MonoBehaviour
{
    Camera PlayerCam;

    [Header("FOV")]

    public float startFOV;
    public float sprintFOV;
    public float transitDurToSprint;
    public float transitDurToWalk;

    [Space]

    [Header("Movement")]

    CharacterController charControl;
    public static float personalSpeed = 1.0f; // The speed this character should travel at by default -- *This* variable should be adjusted based on class.

    [Range(1.2f, 3f)]
    public float sprintMultiplier = 1.5f;

    [Range(1.5f, 3f)]
    public float diagSpeed; // The divider by which the character should travel diagonally

    [Range(1.1f, 1.5f)]
    public float airDrag = 1.3f;

    float speedMultiplier; 
    public bool canJump = true;
    public static float jumpHeight = 5.0f;
    public float gravity;

    float prevY;

    void Awake()
    {
        PlayerCam = gameObject.GetComponentInChildren<Camera>();
        PlayerCam.fieldOfView = startFOV;
        speedMultiplier = personalSpeed;
    }
    void Start()
    {
        charControl = GetComponent<CharacterController>();
    }
    void Update()
    {
        
        // Calculate multiplier based on current movement conditions(sprinting, diagonal movement... etc)
        CalculateSpeedMultiplier();

        // Calculate target movement 
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal") * speedMultiplier * 10, 0, Input.GetAxis("Vertical") * speedMultiplier * 10);
        moveDirection.y = prevY;
        if (charControl.isGrounded) {
            moveDirection.y = 0.0f;
            prevY = 0.0f;
            moveDirection = transform.TransformDirection(moveDirection);
            if (canJump && Input.GetKeyDown(KeyCode.Space))
            {
                // Jump
                moveDirection.y = jumpHeight;
            }
        }
        else
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal") * speedMultiplier * 10, moveDirection.y, Input.GetAxis("Vertical") * speedMultiplier * 10);
            moveDirection = transform.TransformDirection(moveDirection);
        }
        
        // Apply Gravity
        moveDirection.y += gravity * Time.deltaTime;

        // Store Y Vector
        prevY = moveDirection.y;

        if (!charControl.isGrounded)
        {
            moveDirection.x = moveDirection.x / airDrag;
            moveDirection.z = moveDirection.z / airDrag;
        }

        // Apply target movement
        charControl.Move(moveDirection * Time.deltaTime);
        

    }

    void CalculateSpeedMultiplier()
    {
        // If sprinting
        if ((Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W)) && !(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            // Speed Up
            speedMultiplier = personalSpeed * sprintMultiplier;

            // Expand FOV 
            if (PlayerCam.fieldOfView < sprintFOV)
                PlayerCam.fieldOfView += .5f * Time.fixedDeltaTime * transitDurToSprint * 100;
        }
        else
        {
            // Go at default speed
            speedMultiplier = personalSpeed;

            // Shrink FOV
            if (PlayerCam.fieldOfView > startFOV)
            {
                PlayerCam.fieldOfView -= 1f * Time.fixedDeltaTime * transitDurToWalk * 100;
            }

            if (((Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.A))) && charControl.isGrounded))
            {
                // Slow down while going diagonal
                speedMultiplier = personalSpeed / diagSpeed;
            }
            else
                speedMultiplier = personalSpeed;
        }
    }
}
