using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassManager : MonoBehaviour {

    /// <summary>
    /// Class Properties
    /// </summary>
    
    [Header("Scout")]
    public static float _scoutSpeed;
    public float scoutSpeed = 1; // CHANGEABLE
    public static float _scoutJumpHeight;
    public float scoutJumpHeight = 5; // CHANGEABLE

    [Header("Tank")]
    public static float _tankSpeed;
    public float tankSpeed = 3f; // CHANGEABLE
    public static float _tankJumpHeight;
    public float tankJumpHeight = 3f; // CHANGEABLE

    [Header("Support")]
    public static float _supportSpeed;
    public float supportSpeed = .9f; // CHANGEABLE
    public static float _supportJumpHeight;
    public float supportJumpHeight = 4f; // CHANGEABLE

    void Update()
    {
        // Update static variables to the public version


        _scoutSpeed = scoutSpeed;
        _scoutJumpHeight = scoutJumpHeight;


        _tankSpeed = tankSpeed;
        _tankJumpHeight = tankJumpHeight;

        _supportSpeed = supportSpeed;
        _supportJumpHeight = supportJumpHeight;
    }
}
