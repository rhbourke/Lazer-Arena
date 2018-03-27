using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The visual and audio effects carried by each player
public class AbilityEffects : MonoBehaviour {

    public AudioSource MovementSrc;
    public AudioSource RocketAudioSrc;
    public AudioSource JumpSrc;
    public AudioClip WalkingLoop;
    public AudioClip RunningLoop;
    public AudioClip JumpSound;
    public AudioClip RocketJumpLoop;
    public AudioClip RocketJumpLoopEnd;

    public Transform RocketPos;

    public GameObject RocketJumpParticle;
    GameObject RocketJumpTemporaryParticle;

    public bool movementEffects;
    public static bool jumped;
    bool landed = false;

    public bool rocketJumpEffects;
    bool isRocketJumping;

    private void Start()
    {
        RocketAudioSrc.clip = RocketJumpLoop;
    }

    void Update () {

        //Updates the effects based on what abilities are being used
        UpdateEffects();
    }


    void UpdateEffects()
    {
        if (rocketJumpEffects)
        {
            RocketJumpCheck();
            RocketJumpEffects();
        }
        if(movementEffects)
            MovementEffects();
    }

    void RocketJumpCheck()
    {
        if (!GetComponent<CharacterController>().isGrounded && Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.LeftShift)) //You just started Rocket Jumping
        {
            isRocketJumping = true;
            RocketJumpTemporaryParticle = Instantiate(RocketJumpParticle, RocketPos) as GameObject;
            PlayRocketSound();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.W) || GetComponent<CharacterController>().isGrounded) // You just stopped Rocket Jumping
        {
            if (isRocketJumping)
            {
                PlayRocketSoundEnd();
            }
            isRocketJumping = false;
        }
    }

    void RocketJumpEffects()
    {
        if (isRocketJumping)
        {
            // Enable Effects
            EnableRocketParticle();
        }
        else
        {
            // Disable Effects
            DisableRocketParticle();
        }
    }

    void PlayRocketSound()
    {
        RocketAudioSrc.clip = RocketJumpLoop;
        RocketAudioSrc.loop = true;
        if (!RocketAudioSrc.isPlaying) {
            RocketAudioSrc.Play();
        }
    }
    void PlayRocketSoundEnd()
    {
        RocketAudioSrc.loop = false;
        RocketAudioSrc.clip = RocketJumpLoopEnd;
        RocketAudioSrc.Play();
    }

    void EnableRocketParticle()
    {
        RocketJumpTemporaryParticle.transform.position = RocketPos.transform.position;
        RocketJumpTemporaryParticle.GetComponent<ParticleSystem>().Play();
    }

    void DisableRocketParticle()
    {
        if (RocketJumpTemporaryParticle != null)
        {
            RocketJumpTemporaryParticle.GetComponent<ParticleSystem>().Stop();
            Destroy(RocketJumpTemporaryParticle, 1f);
        }
    }
    void MovementEffects()
    {
        JumpingSound();
        WalkingSound();
        LandingEffects();
        
    }
    void LandingEffects()
    {
        if (!GetComponent<CharacterController>().isGrounded)
        {
            landed = false;
        }

        if (!landed && GetComponent<CharacterController>().isGrounded)
        {
            landed = true;
            PlayLandingEffects();
        }
    }
    void PlayLandingEffects()
    {
        // Add dust poof particle instantiation here
        // Add a landing sound here
    }
    void JumpingSound()
    {
        if (jumped)
        {
            JumpSrc.PlayOneShot(JumpSound);
            jumped = false;
        }
    }
    void WalkingSound()
    {
        if (GetComponent<CharacterController>().isGrounded && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
        {
            MovementSrc.loop = true;

            if (!Input.GetKey(KeyCode.LeftShift))
            {
                MovementSrc.clip = WalkingLoop;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                MovementSrc.clip = RunningLoop;
            }

            if (!MovementSrc.isPlaying)
                MovementSrc.Play();
        } else
            MovementSrc.clip = null;
    }
}

