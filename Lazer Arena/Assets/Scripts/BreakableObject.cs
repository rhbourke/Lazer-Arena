using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour {
    public float objKillTime;
    public bool explodes;
	
    public void KillMe()
    {
        if (explodes)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 20f);
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.GetComponent<Rigidbody>() != null)
                {
                    collider.gameObject.GetComponent<Rigidbody>().AddExplosionForce(500f, this.gameObject.transform.position, 200f, 5f);
                }
            }
        }
        Quaternion rotation = Quaternion.identity;
        rotation.eulerAngles = new Vector3(0, 0, 0);
        Instantiate(EffectManager._DeathExplosion, transform.position, rotation);
        if (this.gameObject.GetComponent<Renderer>() != null)
        {
            this.gameObject.GetComponent<Renderer>().enabled = false;
        }
        else
            this.gameObject.SetActive(false);
        this.gameObject.GetComponent<Collider>().enabled = false;
        Destroy(this.gameObject, 2f);
    }
}
