using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketJumpPS : MonoBehaviour {

    private void Awake()
    {
        GetComponent<ParticleSystem>().Stop();
    }
    void Update () {
        // Play Rocket boost emmision when sprinting in air(Rocket Jumping)
        if (!transform.parent.gameObject.GetComponent<CharacterController>().isGrounded && Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {
            GetComponent<ParticleSystem>().Play();
        }
        else
            GetComponent<ParticleSystem>().Stop();

    }

}
