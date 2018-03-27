using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityEffects : MonoBehaviour {

    public GameObject RocketJumpParticle;
    public Transform RocketPos; // The position this particle effect is emitted from
    GameObject RocketJumpTemporaryParticle;


    // Turns the effect on and off. 
    bool isRocketJumping;
    bool justLanded;
	
	void Update () {

        // Checks for mechanic and updates the bool
        if (!GetComponent<CharacterController>().isGrounded && Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRocketJumping = true;
        }
        else
            isRocketJumping = false;

        // add landing dust poof logic here

        PlayEffects();
        
    }
    
    void PlayEffects()
    {
        ///
        /// Enables or disables effects
        /// 


        if (isRocketJumping)
        {
            // Enable Effects
            RocketJumpTemporaryParticle = Instantiate(RocketJumpParticle, RocketPos);
            RocketJumpTemporaryParticle.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            // Disable Effects
            if (RocketJumpTemporaryParticle != null)
            {
                RocketJumpTemporaryParticle.GetComponent<ParticleSystem>().Stop();
                Destroy(RocketJumpTemporaryParticle, 1f);
            }
        }

        // add landing dust poof logic here

    }
}

