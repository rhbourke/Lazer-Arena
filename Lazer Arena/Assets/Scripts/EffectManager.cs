using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour {
    public static AudioClip _JumpSound;
    public static AudioClip _WalkLoop;
    public static AudioClip _RunLoop;
    public static AudioClip _RocketJumpLoop;
    public static AudioClip _RocketJumpLoopEnd;
    public static AudioClip _LandingSound;
    public static GameObject _RocketJumpParticle;
    public AudioClip JumpSound;
    public AudioClip WalkLoop;
    public AudioClip RunLoop;
    public AudioClip RocketJumpLoop;
    public AudioClip RocketJumpLoopEnd;
    public AudioClip LandingSound;
    public GameObject RocketJumpParticle;

    private void Awake()
    {
        _JumpSound = JumpSound;
        _WalkLoop = WalkLoop;
        _RunLoop = RunLoop;
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
