using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontRenderMeLocal : MonoBehaviour {
	

	// Do not render this gameobj. DISABLE THIS COMPONENT IF YOU ARE NOT THE LOCAL PLAYER
	void Update () {
        gameObject.GetComponent<Renderer>().enabled = false;
    }
}
