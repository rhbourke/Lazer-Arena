using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour {
    public static AudioClip _JumpSound;
    public static AudioClip _JumpSound2;
    public static AudioClip _JumpSound3;
    public static AudioClip _JumpSound4;
    public static AudioClip _ScoutStartMove;
    public static AudioClip _ScoutWalkLoop;
    public static AudioClip _ScoutRunLoop;
    public static AudioClip _ScoutEndMove;
    public static AudioClip _ScoutBoostLoop;
    public static AudioClip _RocketJumpLoopEnd;
    public static AudioClip _LandingSound;
    public static AudioClip _LandingSound2;
    public static AudioClip _LandingSound3;
    public static AudioClip _LandingSound4;
    public static AudioClip _HeadHit;
    public static GameObject _LandingParticle;
    public static GameObject _ScoutBoostParticle;
    public static GameObject _DeathExplosion;
    public static GameObject _Lazer;
    public static GameObject _LazerExplosion;

    public GameObject LazerExplosion;
    public GameObject Lazer;
    public GameObject DeathExplosion;

    [Header ("Jumping")]
    public AudioClip JumpSound;
    public AudioClip JumpSound2;
    public AudioClip JumpSound3;
    public AudioClip JumpSound4;
    

    [Space]
    [Header("Landing")]
    public AudioClip LandingSound;
    public AudioClip LandingSound2;
    public AudioClip LandingSound3;
    public AudioClip LandingSound4;
    public GameObject LandingParticle;
    [Space]
    public AudioClip HeadHitSound;


    [Space]
    [Header("Scout")]
    public AudioClip ScoutStartMove;
    public AudioClip ScoutWalkLoop;
    public AudioClip ScoutRunLoop;
    public AudioClip ScoutEndMove;
    public AudioClip RocketJumpLoop;
    public AudioClip RocketJumpLoopEnd;
    public GameObject RocketJumpParticle;
    


    private void Awake()
    {
        _LazerExplosion = LazerExplosion;
        _Lazer = Lazer;
        _DeathExplosion = DeathExplosion;

        _JumpSound = JumpSound;
        _JumpSound2 = JumpSound2;
        _JumpSound3 = JumpSound3;
        _JumpSound4 = JumpSound4;
        _ScoutWalkLoop = ScoutWalkLoop;
        _ScoutRunLoop = ScoutRunLoop;
        _ScoutEndMove = ScoutEndMove;
        _ScoutStartMove = ScoutStartMove;
        _ScoutBoostLoop = RocketJumpLoop;
        _RocketJumpLoopEnd = RocketJumpLoopEnd;

        _LandingSound = LandingSound;
        _LandingSound2 = LandingSound2;
        _LandingSound3 = LandingSound3;
        _LandingSound4 = LandingSound4;
        _LandingParticle = LandingParticle;
        _HeadHit = HeadHitSound;

        _ScoutBoostParticle = RocketJumpParticle;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
