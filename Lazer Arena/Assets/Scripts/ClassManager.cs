using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassManager : MonoBehaviour {

    /// <summary>
    /// Class Properties
    /// </summary>
    static CharacterControl characterControl = new CharacterControl();

    
    public static float _scoutSpeed;
    [Header("Scout")]
    [Space]
    public float ScoutVitality;
    public static float _ScoutVitality;
    public float ScoutPower;
    public static float _ScoutPower;
    public float scoutSpeed = 1; 
    public static float _scoutJumpHeight;
    public float scoutJumpHeight = 5; 
    public static float _SFuelRecharge;
    public float SFuelRechargeSpeed = 10;
    public static float _SFuelUse;
    public float SFuelUseSpeed = 20;
    public static float _SFuelRechargeTime;
    public float SFuelRechargeTime = 3f;
    public static bool _SBoostEnabled;
    public bool SBoostEnabled;
    public static float _SBoostMultiplier;
    public float SBoostMultiplier;
    public float sprintFOV;
    public float transitDurToSprint;
    public float transitDurToWalk;
    public static float _sprintFOV;
    public static float _transitDurToSprint;
    public static float _transitDurToWalk;



    public static float _tankSpeed;
    [Header("Tank")]
    [Space]
    public float tankSpeed = 3f; // CHANGEABLE
    public static float _tankJumpHeight;
    public float tankJumpHeight = 3f; // CHANGEABLE
    

    
    public static float _supportSpeed;
    [Header("Support")]
    [Space]
    public float supportSpeed = .9f; // CHANGEABLE
    public static float _supportJumpHeight;
    public float supportJumpHeight = 4f; // CHANGEABLE


    void Update()
    {
        // Update static variables to the public version

        _ScoutVitality = ScoutVitality;
        _ScoutPower = ScoutPower;
        _scoutSpeed = scoutSpeed;
        _scoutJumpHeight = scoutJumpHeight;
        _SFuelRecharge = SFuelRechargeSpeed;
        _SFuelUse = SFuelUseSpeed;
        _SFuelRechargeTime = SFuelRechargeTime;
        _SBoostEnabled = SBoostEnabled;
        _SBoostMultiplier = SBoostMultiplier;
        _sprintFOV = sprintFOV;
        _transitDurToSprint = transitDurToSprint;
        _transitDurToWalk = transitDurToWalk;

        _tankSpeed = tankSpeed;
        _tankJumpHeight = tankJumpHeight;

        _supportSpeed = supportSpeed;
        _supportJumpHeight = supportJumpHeight;



        
    }
}
