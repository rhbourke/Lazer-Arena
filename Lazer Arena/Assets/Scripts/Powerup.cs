using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {

    [Space]

    public Collider Collider;

    [Space]

    public float respawnTime;
    bool active = true;

    [Header("Type of Powerup")]
    public bool SpeedBoost;
    public bool JumpBoost;

    [Space]

    [Header("Properties")]
    [Range(1.1f, 3f)]
    public float multiplier;

    [Range(1f, 30f)]
    public float duration;

    float origSpeed;
    float origHeight;


    void Start () {
        // Stores original speeds before boosting them
        origSpeed = CharacterControl.personalSpeed;
        origHeight = CharacterControl.jumpHeight;

        Collider = GetComponent<Collider>();

        // Defaults to type speed boost if no type is set
        if(!SpeedBoost && !JumpBoost)
        {
            SpeedBoost = true;
        }
    }
	
	void Update () { 

        if (!active)
        {
            // Disable the Powerup when used
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<SphereCollider>().enabled = false;
        }
        else
        {
            // Re-enable the powerup
            GetComponent<MeshRenderer>().enabled = true;
            GetComponent<SphereCollider>().enabled = true;
        }

        // Makes sure only one type is enabled at a time
        if (SpeedBoost)
        {
            JumpBoost = false;
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
        active = false;
        Debug.Log("Picked up speed boost");

        CharacterControl.personalSpeed *= multiplier;
        yield return new WaitForSeconds(duration);
        CharacterControl.personalSpeed = origSpeed;

        StartCoroutine(RespawnPowerup());
    }
    IEnumerator GiveJumpBoost()
    {
        active = false;
        Debug.Log("Picked up jump boost");

        CharacterControl.jumpHeight *= multiplier;
        yield return new WaitForSeconds(duration);
        CharacterControl.jumpHeight = origHeight;

        StartCoroutine(RespawnPowerup());
    }
    IEnumerator RespawnPowerup()
    {
        yield return new WaitForSeconds(respawnTime);
        active = true;
    }
}
