using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterControl : MonoBehaviour
{
    public enum Class 
    {
        Scout,
        Tank,
        Support
        // Add classes here
    }
    public Class myClass;

    [Space]

    Camera PlayerCam;

    [Header("FOV")]

    public float startFOV;
    public float sprintFOV;
    public float transitDurToSprint;
    public float transitDurToWalk;

    [Space]

    CharacterController charControl;
    
    public static float personalSpeed = 1.0f; // The speed this character should travel at by default -- This variable is adjusted by speed boost and class type.
    
    [Header("Movement")]
    [Range(1.2f, 3f)]
    public float sprintMultiplier = 1.5f;

    [Range(1.5f, 3f)]
    public float diagSpeed; // The divider by which the character should travel diagonally

    [Range(1.1f, 1.5f)]
    public float airDrag = 1.3f;

    float speedMultiplier; 
    public static bool canJump = true;
    public static float jumpHeight = 5.0f;
    public float gravity;
    public bool airDragEnabled;

    float prevY;
    Vector3 moveDirection;

    // Add boost bools here. (True if the boost is being used by player)
    public static bool jumpBoosted = false;
    public static bool speedBoosted = false;


    void Start()
    {
        // Set references and defaults
        PlayerCam = gameObject.GetComponentInChildren<Camera>();
        PlayerCam.fieldOfView = startFOV;
        speedMultiplier = personalSpeed;
        charControl = GetComponent<CharacterController>();
    }
    void Update()
    {

        // Update variables based on class
        UpdateClass();

        // Check if any boosts are active, and use these multipliers
        BoostCheck();

        // Calculate multiplier based on current movement conditions(sprinting, diagonal movement... etc)
        CalculateSpeedMultiplier();

        // Calculate target movement 
        CalculateMovement();

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
                speedMultiplier = personalSpeed / Mathf.Sqrt(2);
            }
            else
                speedMultiplier = personalSpeed;
        }
    }
    void CalculateMovement()
    {
        
        moveDirection = new Vector3(Input.GetAxis("Horizontal") * speedMultiplier * 10, 0, Input.GetAxis("Vertical") * speedMultiplier * 10);

        // Update Y vector
        moveDirection.y = prevY;

        if (charControl.isGrounded)
        {
            moveDirection.y = 0.0f;
            prevY = 0.0f;
            moveDirection = transform.TransformDirection(moveDirection);
            if (canJump && Input.GetKeyDown(KeyCode.Space))
            {
                // Jump
                moveDirection.y = jumpHeight;
                AbilityEffects.jumped = true;
            }
        }
        else
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal") * speedMultiplier * 10, moveDirection.y, Input.GetAxis("Vertical") * speedMultiplier * 10);
            moveDirection = transform.TransformDirection(moveDirection);
        }

        if (Physics.Raycast(transform.position, transform.up * 1.2f, 1.2f))
        {
            //Hit your head, fall back down
            moveDirection.y -= jumpHeight;
        }

        // Apply Gravity
        moveDirection.y += gravity * Time.deltaTime;

        // Store Y Vector
        prevY = moveDirection.y;

        if (airDragEnabled)
        {
            // Apply Air Drag
            if (!charControl.isGrounded)
            {
                moveDirection.x = moveDirection.x / airDrag;
                moveDirection.z = moveDirection.z / airDrag;
            }
        }
    }
    void UpdateClass()
    {
        switch (myClass)
        {
            case Class.Scout:
                personalSpeed = ClassManager._scoutSpeed;
                jumpHeight = ClassManager._scoutJumpHeight;
                break;

            case Class.Tank:
                personalSpeed = ClassManager._tankSpeed;
                jumpHeight = ClassManager._tankJumpHeight;
                break;

            case Class.Support:
                personalSpeed = ClassManager._supportSpeed;
                jumpHeight = ClassManager._supportJumpHeight;
                break;

            ///
            /// Add other classes here
            ///
        }
    }

    //Checks for boosts and then uses multiplied variables 
    void BoostCheck()
    {
        if (jumpBoosted)
        {
            jumpHeight = Powerup.boostedJumpHeight;
        }
        if (speedBoosted)
        {
            personalSpeed = Powerup.boostedSpeed;
        }
        ///
        /// Add other boosts here
        ///
    }
}
