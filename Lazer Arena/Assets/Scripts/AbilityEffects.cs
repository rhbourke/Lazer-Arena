using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The visual and audio effects carried by each player
public class AbilityEffects : MonoBehaviour {

    public AudioSource MovementSrc;
    public AudioSource RocketAudioSrc;
    public AudioSource JumpSrc;
    AudioClip WalkingLoop;
    AudioClip RunningLoop;
    AudioClip JumpingSound;
    AudioClip RocketJumpLoop;
    AudioClip RocketJumpLoopEnd;
    AudioClip LandingSound;

    public Transform RocketPos;

    GameObject RocketJumpParticle;
    GameObject RocketJumpTemporaryParticle;

    public bool movementEffects;
    public static bool jumped;
    bool landed = false;

    public bool rocketJumpEffects;
    bool isRocketJumping;

    private void Start()
    {
        // Grabs Globally set Effect sources
        WalkingLoop = EffectManager._WalkLoop;
        RunningLoop = EffectManager._RunLoop;
        JumpingSound = EffectManager._JumpSound;
        RocketJumpLoop = EffectManager._RocketJumpLoop;
        RocketJumpLoopEnd = EffectManager._RocketJumpLoopEnd;
        LandingSound = EffectManager._LandingSound;
        RocketJumpParticle = EffectManager._RocketJumpParticle;

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
        if (!RocketJumpTemporaryParticle.GetComponent<ParticleSystem>().isPlaying)
        {
            RocketJumpTemporaryParticle.GetComponent<ParticleSystem>().Play();
        }
    }

    void DisableRocketParticle()
    {
        if (RocketJumpTemporaryParticle != null)
        {
            if (RocketJumpTemporaryParticle.GetComponent<ParticleSystem>().isPlaying)
                RocketJumpTemporaryParticle.GetComponent<ParticleSystem>().Stop();
            Destroy(RocketJumpTemporaryParticle, 3f);
        }
    }
    void MovementEffects()
    {
        JumpSound();
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
        JumpSrc.PlayOneShot(LandingSound);
    }
    void JumpSound()
    {
        if (jumped)
        {
            JumpSrc.PlayOneShot(JumpingSound);
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

