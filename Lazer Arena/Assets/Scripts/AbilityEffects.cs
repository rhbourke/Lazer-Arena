using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The visual and audio effects carried by each player
public class AbilityEffects : MonoBehaviour {

    public AudioSource MovementSrc;
    public AudioSource RocketAudioSrc;
    public AudioSource JumpSrc;
    AudioClip ScoutWalkLoop;
    AudioClip ScoutRunLoop;
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
        ScoutWalkLoop = EffectManager._ScoutWalkLoop;
        ScoutRunLoop = EffectManager._ScoutRunLoop;
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
        if (CharacterControl.isScout)
        {
            if (ClassManager._SBoostEnabled)
            {
                RocketJumpCheck();
                RocketJumpEffects();
            }
        }
        if(movementEffects)
            MovementEffects();
    }

    void RocketJumpCheck()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.W)) // You just started Rocket Jumping
        {
            if (CharacterControl.SFuel > 1)
            {
                isRocketJumping = true;
                RocketJumpTemporaryParticle = Instantiate(RocketJumpParticle, RocketPos) as GameObject;
                PlayRocketSound();
            }
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.LeftShift)) // You just started Rocket Jumping
        {
            if (CharacterControl.SFuel > 1)
            {
                isRocketJumping = true;
                RocketJumpTemporaryParticle = Instantiate(RocketJumpParticle, RocketPos) as GameObject;
                PlayRocketSound();
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.W)) // You just stopped Rocket Jumping
        {
            if (isRocketJumping)
            {
                PlayRocketSoundEnd();
            }
            isRocketJumping = false;
        }
        if (CharacterControl.SFuel < 1)
        {
            if (isRocketJumping)
            {
                PlayRocketSoundEnd();
                DisableRocketParticle();
                isRocketJumping = false;
            }
            
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
        // Play the jump sound when jumping
        JumpSound();

        // Play the default movement sounds that are class specific
        WalkingSound();

        // Play dust particle and sound when landing
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
            //If walking, play these sounds


            MovementSrc.loop = true;

            if (CharacterControl.isScout)
            {
                ManageScoutSounds();
            }
            if (CharacterControl.isTank)
            {
                ManageTankSounds();
            }
            if (CharacterControl.isSupport)
            {
                ManageScoutSounds();
            }

            if (!MovementSrc.isPlaying)
                MovementSrc.Play();
        } else
            MovementSrc.clip = null;
    }
    void ManageScoutSounds()
    {
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            MovementSrc.clip = ScoutWalkLoop;
        }
        if (Input.GetKey(KeyCode.LeftShift) && !ClassManager._SBoostEnabled)
        {
            MovementSrc.clip = ScoutWalkLoop;
        }
        if (isRocketJumping)
        {
            if (CharacterControl.SFuel < 1)
            {
                MovementSrc.clip = ScoutWalkLoop;
                isRocketJumping = false;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                MovementSrc.clip = ScoutRunLoop;
            }
        }
        if (CharacterControl.SFuel < 1)
        {
            MovementSrc.clip = ScoutWalkLoop;
        }
    }
    void ManageTankSounds()
    {

    }
    void ManageSupportSounds()
    {

    }
}

