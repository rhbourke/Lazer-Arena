using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour {
    public static AudioClip _JumpSound;
    public static AudioClip _ScoutWalkLoop;
    public static AudioClip _ScoutRunLoop;
    public static AudioClip _RocketJumpLoop;
    public static AudioClip _RocketJumpLoopEnd;
    public static AudioClip _LandingSound;
    public static GameObject _RocketJumpParticle;
    public AudioClip JumpSound;
    public AudioClip ScoutWalkLoop;
    public AudioClip ScoutRunLoop;
    public AudioClip RocketJumpLoop;
    public AudioClip RocketJumpLoopEnd;
    public AudioClip LandingSound;
    public GameObject RocketJumpParticle;

    private void Awake()
    {
        _JumpSound = JumpSound;
        _ScoutWalkLoop = ScoutWalkLoop;
        _ScoutRunLoop = ScoutRunLoop;
        _RocketJumpLoop = RocketJumpLoop;
        _RocketJumpLoopEnd = RocketJumpLoopEnd;
        _LandingSound = LandingSound;
        _RocketJumpParticle = RocketJumpParticle;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
