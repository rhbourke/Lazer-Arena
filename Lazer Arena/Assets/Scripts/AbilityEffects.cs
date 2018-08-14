using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The visual and audio effects carried by each player
public class AbilityEffects : MonoBehaviour {

    //The classes have different gun effects
    //tank
    bool machineGun = false;
    bool pointerGun = false;



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


    public GameObject LazerStartPoint;
    public GameObject LazerEndPoint;
    public bool lazerShootingHit;
    public bool lazerShootingAir;
    LineRenderer lazerRend;
    public float lazerPulseSpeed;
    public float maxLazerWidth;
    public float minLazerWidth;

    [HideInInspector]
    public Vector3 LazerHitPoint;

    [HideInInspector]
    public Quaternion LazerHitRot;

    private void Start()
    {
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
        SetWeaponType();
        lazerRend = LazerRenderer.GetComponent<LineRenderer>();
=======

        lazerRend = GetComponent<LineRenderer>();
>>>>>>> parent of 0f4e075... Added Lazer Fade Out
=======

        lazerRend = GetComponent<LineRenderer>();
>>>>>>> parent of 0f4e075... Added Lazer Fade Out
=======

        lazerRend = GetComponent<LineRenderer>();
>>>>>>> parent of 0f4e075... Added Lazer Fade Out
=======

        lazerRend = GetComponent<LineRenderer>();
>>>>>>> parent of 0f4e075... Added Lazer Fade Out
=======

        lazerRend = GetComponent<LineRenderer>();
>>>>>>> parent of 0f4e075... Added Lazer Fade Out
        lazerRend.enabled = false;

        charController = GetComponent<CharacterControl>();

        // We arent shooting the lazer at start
        lazerShootingAir = false;
        lazerShootingHit = false;
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


        LazerLineRender();
        }

        // IF DEAD
        if (!GetComponent<Combat>().isAlive)
        {
            if (living)
            {
                living = false;
                Quaternion rotation = Quaternion.identity;
                rotation.eulerAngles = new Vector3(0, 0, 0);
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

    //Code below rotates through the jump sound the player uses
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
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            if (scoutBoosting)
            {
                PlayRocketSoundEnd();
                DisableRocketParticle();
                scoutBoosting = false;
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
        yield return new WaitForSeconds(MovementSrc.clip.length);
        hasPlayedStart = true;
    }
    bool shootingLazer = false;
    bool x = false;
    float i = 0;
    void LazerLineRender()
    {
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD

        if (lazerShootingAir || lazerShootingHit)
        { // if you are shooting 
            UpdateWidth(); // Update the width of the lazer for a warping effect
            if (shootingLazer == false) // if you just started shooting
=======
        UpdateWidth(); // Update the width of the lazer for a warping effect
        if (lazerShootingAir || lazerShootingHit) { // if you are shooting 
            if(shootingLazer == false) // if you just started shooting
>>>>>>> parent of 0f4e075... Added Lazer Fade Out
=======
        UpdateWidth(); // Update the width of the lazer for a warping effect
        if (lazerShootingAir || lazerShootingHit) { // if you are shooting 
            if(shootingLazer == false) // if you just started shooting
>>>>>>> parent of 0f4e075... Added Lazer Fade Out
=======
        UpdateWidth(); // Update the width of the lazer for a warping effect
        if (lazerShootingAir || lazerShootingHit) { // if you are shooting 
            if(shootingLazer == false) // if you just started shooting
>>>>>>> parent of 0f4e075... Added Lazer Fade Out
=======
        UpdateWidth(); // Update the width of the lazer for a warping effect
        if (lazerShootingAir || lazerShootingHit) { // if you are shooting 
            if(shootingLazer == false) // if you just started shooting
>>>>>>> parent of 0f4e075... Added Lazer Fade Out
=======
        UpdateWidth(); // Update the width of the lazer for a warping effect
        if (lazerShootingAir || lazerShootingHit) { // if you are shooting 
            if(shootingLazer == false) // if you just started shooting
>>>>>>> parent of 0f4e075... Added Lazer Fade Out
            {
                lazerRend.startWidth = minLazerWidth; // lazer starts small
                lazerRend.endWidth = maxLazerWidth;
            }
            shootingLazer = true;
            if (pointerGun)
            {
                lazerRend.enabled = true;
            }
            if (machineGun)
            {
                if (i <= 1f && !x)
                {
                    i += 5f * Time.deltaTime;
                    lazerRend.enabled = true;
                }
                else
                {
                    x = true;
                    i -= 5f * Time.deltaTime;
                    lazerRend.enabled = false;

                }
                if (x && i <= 0)
                {
                    x = false;
                }

            }
            lazerRend.SetPosition(0, LazerStartPoint.transform.position); // Sets start point of lazer

            if (lazerShootingHit)
            {
                lazerRend.SetPosition(1, LazerHitPoint); // Sets end point of the lazer to be where it hit
                GameObject Impact = Instantiate(EffectManager._LazerExplosion, LazerHitPoint, LazerHitRot); // Spawns impact effects from the lazer
                Destroy(Impact, .6f);
            }
            else
            {
                lazerRend.SetPosition(1, LazerEndPoint.transform.position); // Sets end point of the lazer to be at the end point game object
                LazerHitPoint = new Vector3(0, 0, 0); // Resets these values
            }
        }
        else // if you arent shooting, turn off the lazer
        {
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
            if (lazerRend.startWidth > 0 || lazerRend.endWidth > 0)
            {
                //lazerRend.SetPosition(0, LazerStartPoint.transform.position); // keeps lazer attached
                //lazerRend.SetPosition(1, LazerEndPoint.transform.position);
                lazerRend.startWidth -= lazerFadeSpeed * Time.deltaTime; // fades out the lazer
                lazerRend.endWidth -= lazerFadeSpeed * Time.deltaTime;
            }
            else
            {
                shootingLazer = false;
                lazerRend.enabled = false;
            }
=======
>>>>>>> parent of 0f4e075... Added Lazer Fade Out
=======
>>>>>>> parent of 0f4e075... Added Lazer Fade Out
=======
>>>>>>> parent of 0f4e075... Added Lazer Fade Out
=======
>>>>>>> parent of 0f4e075... Added Lazer Fade Out
=======
>>>>>>> parent of 0f4e075... Added Lazer Fade Out
            shootingLazer = false;
            lazerRend.enabled = false;
        }
    }
    bool isbig = false;
    void UpdateWidth()
    {
        if (lazerRend.startWidth < maxLazerWidth && !isbig) //If width < than max
        {
            //Grow
            lazerRend.startWidth += lazerPulseSpeed * Time.deltaTime;
            lazerRend.endWidth -= lazerPulseSpeed * Time.deltaTime;
        }
        else // if width > max
        {
            isbig = true;
            //Shrink
            if (lazerRend.startWidth > minLazerWidth)
            {
                lazerRend.startWidth -= lazerPulseSpeed * Time.deltaTime;
                lazerRend.endWidth += lazerPulseSpeed * Time.deltaTime;
            }
            else
                isbig = false;
        }
    }
    void SetWeaponType()
    {
        if(GetComponent<CharacterControl>().isScout)
        {
            machineGun = false;
            pointerGun = true;
        }
        if (GetComponent<CharacterControl>().isSupport)
        {
            machineGun = true;
            pointerGun = false;
        }
    }
}

