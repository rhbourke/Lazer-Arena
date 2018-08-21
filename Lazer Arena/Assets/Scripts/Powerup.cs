using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {
    public enum PowerupType
    {
        Agility,
        
    }

    GameObject Player;

    [Header("Type of Powerup")]
    public PowerupType powerupType;
    [Space]
    Collider Collider;
    bool active = true;

    // Determined by powerupType
    bool AgilityBoost;
    CharacterControl charController;
    
    [Header("Properties")]
    [Range(1.1f, 3f)]
    public float multiplier;

    [Range(1f, 30f)]
    public float duration;

    public float respawnTime;

    public static float boostedSpeed; // These variables are used for updating control speeds on the character control script
    public static float boostedJumpHeight;

    float normalFOV;
    public float agilityFOVIncrease;
    public float FOVTransitionTime = .5f;
    bool increasingFOV = false;
    bool decreasingFOV = false;


    void Start () {

        
        Collider = GetComponent<Collider>();

        // Defaults to type agility boost if no type is set
        if(!AgilityBoost) // Make sure to add the other types here
        {
            AgilityBoost = true;
        }
        
        // Make sure only one type of boost is true
        switch (powerupType)
        {
            case PowerupType.Agility:
                AgilityBoost = true;
                break;


            // Add more cases for different types of powerups 
        }
    }
	
	void Update () {
        
        if (!active)
        {
            // Disable when used
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<SphereCollider>().enabled = false;
        }
        else
        {
            // Re-enable the powerup after respawning
            GetComponent<MeshRenderer>().enabled = true;
            GetComponent<SphereCollider>().enabled = true;
        }
        if (increasingFOV)
        {
            IncreaseFOV();
        }
        if (decreasingFOV)
        {
            DecreaseFOV();
        }
	}
    void OnTriggerEnter(Collider collider)
    {
        if (active)
        {
            if (collider.gameObject.tag == "Player")
            {
                Player = collider.gameObject;
                charController = Player.GetComponent<CharacterControl>();
                normalFOV = charController.PlayerCam.fieldOfView;

                if (AgilityBoost) // If this is a speed boost
                {
                    // Give speed boost 
                    StartCoroutine(GiveSpeedBoost());
                    StartCoroutine(GiveJumpBoost());
                }
            }
        }
    }
    IEnumerator GiveSpeedBoost()
    {
        active = false; // Deactivate Powerup

        charController.speedBoosted = true; // Powerup is being used
        increasingFOV = true;
        boostedSpeed = charController.personalSpeed * multiplier; // Updates the target speed (to assign when powerup is being used)
        yield return new WaitForSeconds(duration);
        charController.speedBoosted = false; // Powerup is no longer being used
        decreasingFOV = true; // Time to resize the fov back to normal

        StartCoroutine(RespawnPowerup());
    }
    IEnumerator GiveJumpBoost()
    {
        active = false; // Deactivate Powerup

        charController.jumpBoosted = true; // True when powerup is being used

        boostedJumpHeight = charController.jumpHeight * multiplier; // Updates the target speed (to assign when powerup is being used)
        yield return new WaitForSeconds(duration);
        charController.jumpBoosted = false; // Powerup is no longer being used

        StartCoroutine(RespawnPowerup());
    }
    void IncreaseFOV()
    {
        if (charController.PlayerCam.fieldOfView < normalFOV + agilityFOVIncrease)
            charController.PlayerCam.fieldOfView += .5f * Time.fixedDeltaTime * FOVTransitionTime * 100;

        if(charController.PlayerCam.fieldOfView >= normalFOV + agilityFOVIncrease)
        {
            increasingFOV = false;
        }
    }
    void DecreaseFOV()
    {
        if (charController.PlayerCam.fieldOfView > normalFOV )
            charController.PlayerCam.fieldOfView -= .5f * Time.fixedDeltaTime * FOVTransitionTime * 100;

        if (charController.PlayerCam.fieldOfView <= normalFOV)
        {
            decreasingFOV = false;
        }
    }
    IEnumerator RespawnPowerup()
    {
        yield return new WaitForSeconds(respawnTime); // Wait for respawn timer
        active = true; // Reactivate Powerup
    }
}
