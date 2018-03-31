using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontRenderMeLocal : MonoBehaviour {
	
	void Start () {
        
	}
	
	// Do not render this gameobj
	void Update () {
        gameObject.GetComponent<Renderer>().enabled = false;
    }
}
