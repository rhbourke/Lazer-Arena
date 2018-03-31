using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour {

    public bool isAlive = true;
    CharacterControl charControl;
    float vitality;
    float power;
    public float range;
    GameObject enemy;
    GameObject breakableObj;
    float holdTime = 0;
    float holdTimeEnviornment = 0;
    bool killedObj;

    void Start () {
        charControl = GetComponent<CharacterControl>();
	}
	
	void Update () {
        if (isAlive)
        {
            if (charControl.isScout)
            {
                vitality = ClassManager._ScoutVitality;
                power = ClassManager._ScoutPower;
            }

            if (Input.GetKey(KeyCode.Mouse0))
            {
                Debug.DrawRay(charControl.PlayerCam.transform.position, charControl.PlayerCam.transform.forward * range, Color.red);
                RaycastHit hit;
                if (Physics.Raycast(charControl.PlayerCam.transform.position, charControl.PlayerCam.transform.forward * range, out hit))
                {
                    if (hit.collider.gameObject.GetComponent<Combat>() != null && hit.collider.gameObject != this.gameObject)
                    {
                        if (hit.collider.gameObject.GetComponent<Combat>().isAlive) { 
                            enemy = hit.collider.gameObject;
                            Debug.Log("Hit player at range of " + hit.distance);
                            DamageEnemy(enemy);
                        }
                    }
                    else
                        holdTime = 0;

                    if (hit.collider.gameObject.GetComponent<BreakableObject>() != null)
                    {
                        breakableObj = hit.collider.gameObject;
                        killedObj = false;
                        DamageObject(breakableObj);
                    }
                    else
                        holdTimeEnviornment = 0;
                }
            }
        } 
	}
    void DamageEnemy(GameObject targetPlayer)
    {
        holdTime += Time.deltaTime;
        if(holdTime >= (targetPlayer.GetComponent<Combat>().vitality / targetPlayer.GetComponent<Combat>().power))
        {
            targetPlayer.GetComponent<Combat>().isAlive = false;
        }
    }
    void DamageObject(GameObject targetObject)
    {
        if (!killedObj) { 
            holdTimeEnviornment += Time.deltaTime;
            if (holdTimeEnviornment >= targetObject.GetComponent<BreakableObject>().objKillTime)
            {
                targetObject.GetComponent<BreakableObject>().KillMe();
                killedObj = true;
                holdTimeEnviornment = 0;
            }
        }
    }
}
