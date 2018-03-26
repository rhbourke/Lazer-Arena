using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {
    public enum PowerupType
    {
        Speed,
        Jump
    }

    [Header("Type of Powerup")]
    public PowerupType powerupType;
    [Space]
    Collider Collider;
    bool active = true;

    // Determined by powerupType
    bool SpeedBoost;
    bool JumpBoost;

    
    [Header("Properties")]
    [Range(1.1f, 3f)]
    public float multiplier;

    [Range(1f, 30f)]
    public float duration;

    public float respawnTime;

    public static float boostedSpeed; // These variables are used for updating control speeds on the character control script
    public static float boostedJumpHeight;
    
    void Start () {

        Collider = GetComponent<Collider>();

        // Defaults to type speed boost if no type is set
        if(!SpeedBoost && !JumpBoost) // Make sure to add the other types here
        {
            SpeedBoost = true;
        }
        
        // Make sure only one type of boost is true
        switch (powerupType)
        {
            case PowerupType.Speed:
                SpeedBoost = true;
                JumpBoost = false;
                break;

            case PowerupType.Jump:
                JumpBoost = true;
                SpeedBoost = false;
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
	}
    void OnTriggerEnter(Collider collider)
    {
        if (active)
        {
            if (collider.gameObject.tag == "Player")
            {
                Debug.Log("Collided with Player");
                if (SpeedBoost) // If this is a speed boost
                {
                    // Give speed boost 
                    StartCoroutine(GiveSpeedBoost());
                }
                if (JumpBoost) // If this is a jump boost
                {
                    // Give jump boost
                    StartCoroutine(GiveJumpBoost());
                }
            }
        }
    }
    IEnumerator GiveSpeedBoost()
    {
        active = false; // Deactivate Powerup
        Debug.Log("Picked up speed boost");

        CharacterControl.speedBoosted = true; // Powerup is being used

        boostedSpeed = CharacterControl.personalSpeed * multiplier; // Updates the target speed (to assign when powerup is being used)
        yield return new WaitForSeconds(duration);
        CharacterControl.speedBoosted = false; // Powerup is no longer being used

        StartCoroutine(RespawnPowerup());
    }
    IEnumerator GiveJumpBoost()
    {
        active = false; // Deactivate Powerup
        Debug.Log("Picked up jump boost");

        CharacterControl.jumpBoosted = true; // True when powerup is being used

        boostedJumpHeight = CharacterControl.jumpHeight * multiplier; // Updates the target speed (to assign when powerup is being used)
        yield return new WaitForSeconds(duration);
        CharacterControl.jumpBoosted = false; // Powerup is no longer being used

        StartCoroutine(RespawnPowerup());
    }
    IEnumerator RespawnPowerup()
    {
        yield return new WaitForSeconds(respawnTime); // Wait for respawn timer
        active = true; // Reactivate Powerup
    }
}
