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
    float holdTime = 0;

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
                    if (hit.collider.gameObject.GetComponent<Combat>() != null)
                    {
                        if (hit.collider.gameObject.GetComponent<Combat>().isAlive) { 
                            enemy = hit.collider.gameObject;
                            Debug.Log("Hit player at range of " + hit.distance);
                            DamageEnemy(enemy);
                        }
                    }
                    else
                        holdTime = 0;
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
}
