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

    [HideInInspector]
    public Camera PlayerCam;

    [Header("FOV")]

    float startFOV;
    

    [Space]

    CharacterController charControl;
    
    public float personalSpeed = 1.0f; // The speed this character should travel at by default -- This variable is adjusted by speed boost and class type.



    bool living;
    float speedMultiplier;
    [HideInInspector]
    public bool canJump = true;
    [HideInInspector]
    public float jumpHeight = 5.0f;
    [Header("Movement")]
    public float gravity;

    [HideInInspector]
    public bool headHit;

    float prevY;
    Vector3 moveDirection;

    // Class
    [HideInInspector]
    public bool isScout;
    [HideInInspector]
    public bool isTank;
    [HideInInspector]
    public bool isSupport;

    // Abilities

    //Scout
    [HideInInspector]
    public bool isScoutBoosting = false;
    [HideInInspector]
    public float SFuel = 100f;
    bool canRechargeSFuel;
    float SBoostMult;


    

    // Add boost bools here. (True if the boost is being used by player)
    [HideInInspector]
    public bool jumpBoosted = false;
    [HideInInspector]
    public bool speedBoosted = false;

    private void Awake()
    {
        // Check what class we are
        UpdateClass();
    }
    void Start()
    {


        // Set object references and defaults
        PlayerCam = GetComponentInChildren<Camera>();
        startFOV = PlayerCam.fieldOfView;
        speedMultiplier = personalSpeed;
        charControl = GetComponent<CharacterController>();
    }
    void Update()
    {

        

        // Update variables based on class
        UpdateClass();

        if (GetComponent<Combat>().isAlive) // IF ALIVE, MOVE
        {
            living = true;

            // Check if any boosts are active, and use these multipliers
            BoostCheck();

            // Calculate multiplier based on current movement conditions(sprinting, diagonal movement... etc)
            CalculateSpeedMultiplier();

            // Calculate target movement 
            CalculateMovement();

            // Apply target movement
            charControl.Move(moveDirection * Time.deltaTime);

        }
        else if (living) // IF PLAYER JUST DIED, RESPAWN HIM
        {
            living = false;

            // Respawn the player
        }
            
    }

    void UpdateClass()
    {
        switch (myClass)
        {
            case Class.Scout:
                personalSpeed = ClassManager._scoutSpeed;
                jumpHeight = ClassManager._scoutJumpHeight;
                isScout = true;
                isTank = false;
                isSupport = false;
                break;

            case Class.Tank:
                personalSpeed = ClassManager._tankSpeed;
                jumpHeight = ClassManager._tankJumpHeight;
                isScout = false;
                isTank = true;
                isSupport = false;
                break;

            case Class.Support:
                personalSpeed = ClassManager._supportSpeed;
                jumpHeight = ClassManager._supportJumpHeight;
                isScout = false;
                isTank = false;
                isSupport = true;
                break;

                ///
                /// Add other classes here
                ///
        }
    }

    void CalculateSpeedMultiplier()
    {
        if (isScout)
        {
            if (ClassManager._SBoostEnabled)
            {
                SBoostMult = ClassManager._SBoostMultiplier;
                // Change speed based on whether or not player is using scout boost
                ScoutBoost();
            }
            else
            {
                speedMultiplier = personalSpeed;
                isScoutBoosting = false;
            }
        }
        else {
            // Sets speed multiplier to class's default speed
            speedMultiplier = personalSpeed;
        }
        if (((Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.A))) && charControl.isGrounded))
        {
            // Slow down while going diagonal
            speedMultiplier = personalSpeed / Mathf.Sqrt(2);
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
                GetComponent<AbilityEffects>().jumped = true;
            }
        }
        else
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal") * speedMultiplier * 10, moveDirection.y, Input.GetAxis("Vertical") * speedMultiplier * 10);
            moveDirection = transform.TransformDirection(moveDirection);
        }

        if (Physics.Raycast(transform.position, transform.up * 1.2f,  1.2f))
        {
            headHit = true;
            //Hit your head, fall back down

            
            if(!(moveDirection.y < gravity))
            {
                moveDirection.y -= jumpHeight;
            }
        }
        else
            headHit = false;

        // Apply Gravity
        moveDirection.y += gravity * Time.deltaTime;

        // Store Y Vector
        prevY = moveDirection.y;

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

    


    /// <summary>
    /// SCOUT ABILITIES
    /// </summary>
    void ScoutBoost()
    {
        Debug.Log("Scout boost fuel: " + SFuel);

        // If using scout boost mechanic
        if ((Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W)) && !(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && SFuel > 1)
        {
            isScoutBoosting = true;
            // Speed Up
            speedMultiplier = personalSpeed * SBoostMult;

            // Expand FOV 
            if (PlayerCam.fieldOfView < ClassManager._sprintFOV && !speedBoosted)
                PlayerCam.fieldOfView += .5f * Time.fixedDeltaTime * ClassManager._transitDurToSprint * 100;

            // Use fuel
            SFuel -= Time.deltaTime * ClassManager._SFuelUse;
        }  
        else
        {
            // If player was scout boosting, start a new delay timer for the recharge
            if (isScoutBoosting)
            {
                StartCoroutine(RechargeSFuelDelay());
            }

            // If not using scout boost input
            isScoutBoosting = false;

            // Go at default speed
            speedMultiplier = personalSpeed;

            // Shrink FOV
            if (PlayerCam.fieldOfView > startFOV && !speedBoosted)
            {
                PlayerCam.fieldOfView -= 1f * Time.fixedDeltaTime * ClassManager._transitDurToWalk * 100;
            }
            else
                speedMultiplier = personalSpeed;

            // Recharge Fuel if waited timer
            if (canRechargeSFuel)
            {
                if (SFuel < 100)
                    SFuel += Time.deltaTime * ClassManager._SFuelRecharge;
            }
        }
    }
    IEnumerator RechargeSFuelDelay()
    {
        canRechargeSFuel = false;
        yield return new WaitForSeconds(ClassManager._SFuelRechargeTime);
        canRechargeSFuel = true;
    }
    /// <summary>
    /// SCOUT ABILITIES END
    /// </summary>
}
