using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The visual and audio effects carried by each player
public class AbilityEffects : MonoBehaviour {

    public AudioSource MovementSrc;
    public AudioSource AbilitySrc;
    public AudioSource JumpSrc;

    public Transform ScoutBoostPos; // The position boost is emitted from, set to Ability By default

    GameObject ScoutBoostTemporaryParticle;
    GameObject LandingTemporaryParticle;
    CharacterControl charController;

    bool living;
    bool isMoving; 
    public bool movementEffects;
    [HideInInspector]
    public bool jumped;
    bool landed = false;
    bool playedLandingSound = false;
    bool hasPlayedStart = false;
    bool scoutBoosting;

    int numLands = 1;
    public float groundDist;
    float timeSinceLastLanding;

    private void Start()
    {
        charController = GetComponent<CharacterControl>();
    }

    void Update () {
        UpdateEffects();
    }


    void UpdateEffects()
    {
        // IF ALIVE
        if (GetComponent<Combat>().isAlive) {
            living = true;

            // First calls class based abilities
            if (charController.isScout)
            {
                if (ClassManager._SBoostEnabled)
                {
                    ScoutBoostCheck();
                    ScoutBoostEffects();
                }
            }
        // Then calls default movement effects, that may be altered by class
        if (movementEffects)
            MovementEffects();
        }

        // IF DEAD
        if (!GetComponent<Combat>().isAlive)
        {
            if (living)
            {
                living = false;
                Quaternion rotation = Quaternion.identity;
                rotation.eulerAngles = new Vector3(-90, 0, 0);
                Instantiate(EffectManager._DeathExplosion, transform.position, rotation, transform);
            }
        }

    }

    
    void MovementEffects()
    {
        JumpSound();
        LandingEffects();
        HeadHit();

        // Play the default movement sounds that are class specific
        WalkingSound();
    }

    
    void LandingEffects()
    {
        timeSinceLastLanding += Time.deltaTime;
        if (!GetComponent<CharacterController>().isGrounded)
        {
            landed = false;
        }

        if (!landed && GetComponent<CharacterController>().isGrounded && timeSinceLastLanding > .4f)
        {
            timeSinceLastLanding = 0;
            playedLandingSound = false;
            landed = true;
            PlayLandingEffects();
        }
    }

    void PlayLandingEffects()
    {
        // Dust particle effect played when landing. groundDist should be checked, it tells how far down to put the particle, maybe try to extend the collider up instead of down for each model so feet dist can stay the same.
        Quaternion rotation = Quaternion.identity;
        rotation.eulerAngles = new Vector3(-90, 0, 0);
        LandingTemporaryParticle = Instantiate(EffectManager._LandingParticle, transform.position + transform.up * groundDist, rotation, this.gameObject.GetComponentInChildren<FX>().gameObject.transform) as GameObject;
        Destroy(LandingTemporaryParticle, 1.5f);

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

    int numJumps = 1;

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

                if (charController.isScout)
                {
                    ManageScoutSounds();
                }
                if (charController.isTank)
                {
                    ManageTankSounds();
                }
                if (charController.isSupport)
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

    float timeSinceLastHeadHit = 0;
    void HeadHit()
    {
        timeSinceLastHeadHit += Time.deltaTime;
        if (charController.headHit && timeSinceLastHeadHit > .2f && !GetComponent<CharacterController>().isGrounded)
        {
            JumpSrc.PlayOneShot(EffectManager._HeadHit);
            timeSinceLastHeadHit = 0;
        }
    }

    void ScoutBoostCheck()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.W) && charController.isScoutBoosting) // You just started Scout boosting
        {
            if (charController.isScoutBoosting)
            {
                scoutBoosting = true;
                ScoutBoostTemporaryParticle = Instantiate(EffectManager._ScoutBoostParticle, ScoutBoostPos) as GameObject;
                PlayRocketSound();
            }
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.LeftShift) && charController.isScoutBoosting) // You just started scout boosting
        {
            if (charController.isScoutBoosting)
            {
                scoutBoosting = true;
                ScoutBoostTemporaryParticle = Instantiate(EffectManager._ScoutBoostParticle, ScoutBoostPos) as GameObject;
                PlayRocketSound();
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || (Input.GetKeyUp(KeyCode.W))) // You just stopped Rocket Jumping
        {
            if (scoutBoosting)
            {
                PlayRocketSoundEnd();
                DisableRocketParticle();
            }
            scoutBoosting = false;
        }
        if (charController.SFuel < 1)
        {
            if (scoutBoosting)
            {
                PlayRocketSoundEnd();
                DisableRocketParticle();
                scoutBoosting = false;
            }
            
        }
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if (scoutBoosting)
            {
                PlayRocketSoundEnd();
                DisableRocketParticle();
                scoutBoosting = false;
            }
        }
        if (charController.isScoutBoosting)
        {
            if (!scoutBoosting)
            {
                scoutBoosting = true;
                ScoutBoostTemporaryParticle = Instantiate(EffectManager._ScoutBoostParticle, ScoutBoostPos) as GameObject;
                PlayRocketSound();
            }
        }
    }

    void ScoutBoostEffects()
    {
        if (scoutBoosting)
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
        AbilitySrc.clip = EffectManager._ScoutBoostLoop;
        AbilitySrc.loop = true;
        if (!AbilitySrc.isPlaying) {
            AbilitySrc.Play();
        }
    }
    void PlayRocketSoundEnd()
    {
        AbilitySrc.loop = false;
        AbilitySrc.clip = EffectManager._RocketJumpLoopEnd;
        AbilitySrc.Play();
    }

    void EnableRocketParticle()
    {
        if (ScoutBoostTemporaryParticle != null)
        {
            ScoutBoostTemporaryParticle.transform.position = ScoutBoostPos.transform.position;
            if (!ScoutBoostTemporaryParticle.GetComponent<ParticleSystem>().isPlaying)
            {
                ScoutBoostTemporaryParticle.GetComponent<ParticleSystem>().Play();
            }
        }
    }

    void DisableRocketParticle()
    {
        if (ScoutBoostTemporaryParticle != null)
        {
            if (ScoutBoostTemporaryParticle.GetComponent<ParticleSystem>().isPlaying)
                ScoutBoostTemporaryParticle.GetComponent<ParticleSystem>().Stop();
            Destroy(ScoutBoostTemporaryParticle, 3f);
        }
    }
    
    void ManageScoutSounds()
    {
        if (charController.isScoutBoosting)
        {
            StopCoroutine(PlayStartMoveSound(StartMovingSound()));
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
        if (charController.isScout){
            return EffectManager._ScoutEndMove;
        }
        return null;
    }
    public AudioClip StartMovingSound()
    {
        if (charController.isScout)
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

