using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterControl : MonoBehaviour
{
    Camera PlayerCam;
    Rigidbody PlayerRB;

    [Header("FOV")]

    public float startFOV;
    public float sprintFOV;
    public float transitDurToSprint;
    public float transitDurToWalk;

    [Space]

    [Header("Movement")]

    public static float personalSpeed = 10.0f; // The speed this character should travel at by default -- *This* variable should be adjusted based on class.

    [Range(1.2f, 3f)]
    public float sprintMultiplier = 1.5f;

    [Range(1.5f, 3f)]
    public float diagSpeed; // The divider by which the character should travel diagonally

    float speedMultiplier; 

    public bool canJump = true;
    public static float jumpHeight = 2.0f;
    public float jumpGravity = 10.0f;

    void Awake()
    {
        PlayerCam = gameObject.GetComponentInChildren<Camera>();
        PlayerCam.fieldOfView = startFOV;
        PlayerRB = GetComponent<Rigidbody>();
        PlayerRB.freezeRotation = true;
        speedMultiplier = personalSpeed;
    }

    void FixedUpdate()
    {
        ///
        /// First we will calculate the speedMultiplier 
        ///


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
            if(PlayerCam.fieldOfView > startFOV)
            {
                PlayerCam.fieldOfView -= 1f * Time.fixedDeltaTime * transitDurToWalk * 100;
            }

            if (((Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.A))) && IsGrounded()))
            {
                // Slow down while going diagonal
                speedMultiplier = personalSpeed / diagSpeed;
            }
            else
                speedMultiplier = personalSpeed;
        }

        ///
        /// Now we use the speedMultiplier to move the character
        ///

        // Calculate how fast we should be moving on X and Y
        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal") * speedMultiplier, PlayerRB.velocity.y, (Input.GetAxis("Vertical")) * speedMultiplier);


        // Apply the target velocity to the local space of the Rigidbody
        PlayerRB.velocity = transform.TransformDirection(targetVelocity);


        if (IsGrounded())
        {
            if (canJump && Input.GetButton("Jump"))
            {
                // Store the current velocity
                Vector3 velocity = PlayerRB.velocity;
                // Jump
                PlayerRB.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.y);
            }
        } 
    }

    bool IsGrounded()
    {
        // Checks for Raycast collision slightly under the player. ***Values may need future adjustment.***
        if (Physics.Raycast(transform.position, new Vector3(0, -1.1f, 0), 1.1f)){
            return true;
        } else
            return false;
    }

    float CalculateJumpVerticalSpeed()
    {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        return Mathf.Sqrt(2 * jumpHeight * jumpGravity);
    }
}
