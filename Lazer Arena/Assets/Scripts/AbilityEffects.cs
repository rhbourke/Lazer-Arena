using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The visual and audio effects carried by each player
public class AbilityEffects : MonoBehaviour {

    public AudioSource MovementSrc;
    public AudioSource RocketAudioSrc;
    public AudioSource JumpSrc;

    int numJumps = 1;


    public Transform RocketPos;

    GameObject RocketJumpTemporaryParticle;

    public bool movementEffects;
    public static bool jumped;
    bool landed = false;
    bool playedLandingSound = false;
    int numLands = 1;
    bool isMoving;
    bool hasPlayedStart = false;

    public bool rocketJumpEffects;
    bool isRocketJumping;

    private void Start()
    {
        // Grabs Globally set Effect sources
        
        RocketAudioSrc.clip = EffectManager._RocketJumpLoop;
    }

    void Update () {
        //Updates the effects based on what abilities are being used
        UpdateEffects();
    }


    void UpdateEffects()
    {
        // Play Ability Effects 
        if (CharacterControl.isScout)
        {
            if (ClassManager._SBoostEnabled)
            {
                RocketJumpCheck();
                RocketJumpEffects();
            }
        }


        // Play default movement effects
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
                RocketJumpTemporaryParticle = Instantiate(EffectManager._RocketJumpParticle, RocketPos) as GameObject;
                PlayRocketSound();
            }
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.LeftShift)) // You just started Rocket Jumping
        {
            if (CharacterControl.SFuel > 1)
            {
                isRocketJumping = true;
                RocketJumpTemporaryParticle = Instantiate(EffectManager._RocketJumpParticle, RocketPos) as GameObject;
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
        RocketAudioSrc.clip = EffectManager._RocketJumpLoop;
        RocketAudioSrc.loop = true;
        if (!RocketAudioSrc.isPlaying) {
            RocketAudioSrc.Play();
        }
    }
    void PlayRocketSoundEnd()
    {
        RocketAudioSrc.loop = false;
        RocketAudioSrc.clip = EffectManager._RocketJumpLoopEnd;
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
    float timeSinceLast = 0;
    void MovementEffects()
    {
        // Play the jump sound when jumping
        JumpSound();

        // Play the default movement sounds that are class specific
        WalkingSound();

        // Play dust particle and sound when landing
        LandingEffects();



        timeSinceLast += Time.deltaTime;

        if (CharacterControl.headHit && timeSinceLast > .2f && !GetComponent<CharacterController>().isGrounded)
        {
            JumpSrc.PlayOneShot(EffectManager._HeadHit);
            timeSinceLast = 0;
        }
    }

    void LandingEffects()
    {
        if (!GetComponent<CharacterController>().isGrounded)
        {
            landed = false;
        }

        if (!landed && GetComponent<CharacterController>().isGrounded)
        {
            playedLandingSound = false;
            landed = true;
            PlayLandingEffects();
        }
    }
    void PlayLandingEffects()
    {
        // Add dust poof particle instantiation here
        // Add a landing sound here
        if (numLands == 1 && !playedLandingSound)
        {
            playedLandingSound = true;
            JumpSrc.PlayOneShot(EffectManager._LandingSound);
            numLands++;
        }
        if (numLands == 2 && !playedLandingSound)
        {
            playedLandingSound = true;
            JumpSrc.PlayOneShot(EffectManager._LandingSound2);
            numLands++;
        }
        if (numLands == 3 && !playedLandingSound)
        {
            playedLandingSound = true;
            JumpSrc.PlayOneShot(EffectManager._LandingSound3);
            numLands++;
        }
        if (numLands == 4 && !playedLandingSound)
        {
            playedLandingSound = true;
            JumpSrc.PlayOneShot(EffectManager._LandingSound4);
            numLands = 1;
        }
    }
    void JumpSound()
    {
        if (jumped && (numJumps == 1))
        {
            JumpSrc.PlayOneShot(EffectManager._JumpSound);
            jumped = false;
            numJumps++;
        }
        if (jumped && (numJumps == 2))
        {
            JumpSrc.PlayOneShot(EffectManager._JumpSound2);
            jumped = false;
            numJumps++;
        }
        if (jumped && (numJumps == 3))
        {
            JumpSrc.PlayOneShot(EffectManager._JumpSound3);
            jumped = false;
            numJumps++;
        }
        if (jumped && (numJumps == 4))
        {
            JumpSrc.PlayOneShot(EffectManager._JumpSound4);
            jumped = false;
            numJumps = 1;
        }
    }
    void WalkingSound()
    {
        // Start moving sounds

        
        if (GetComponent<CharacterController>().isGrounded && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
        {
            isMoving = true;
            if (!hasPlayedStart)
            {
                StartCoroutine(PlayStartMoveSound(StartMovingSound()));
            }

            if (hasPlayedStart)
            {
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
            }

        }
        else
        {
            StopCoroutine(PlayStartMoveSound(StartMovingSound()));
            MovementSrc.clip = null;
            hasPlayedStart = false;
            if (isMoving)
            {
                MovementSrc.PlayOneShot(EndMovingSound());
                isMoving = false;
            }
        }
    }
    void ManageScoutSounds()
    {
        if (CharacterControl.isScoutBoosting)
        {
            MovementSrc.clip = EffectManager._ScoutRunLoop;
        }
        else
            MovementSrc.clip = EffectManager._ScoutWalkLoop;

    }
    void ManageTankSounds()
    {

    }
    void ManageSupportSounds()
    {

    }
    public AudioClip EndMovingSound()
    {
        if (CharacterControl.isScout){
            return EffectManager._ScoutEndMove;
        }
        return null;
    }
    public AudioClip StartMovingSound()
    {
        if (CharacterControl.isScout)
        {
            return EffectManager._ScoutStartMove;
        }
        return null;
    }
    IEnumerator PlayStartMoveSound(AudioClip StartMoveSound)
    {
        MovementSrc.loop = false;
        MovementSrc.clip = StartMoveSound;
        if(!MovementSrc.isPlaying)
            MovementSrc.Play();
        yield return new WaitForSeconds(StartMoveSound.length);
        hasPlayedStart = true;
    }
}

