using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionNut : MonoBehaviour {

    AudioSource explosionSrc;
    public AudioClip explosionNut;
	// Use this for initialization
	void Start () {
        explosionSrc = GetComponent<AudioSource>();
        explosionSrc.loop = false;
        explosionSrc.clip = explosionNut;
        if (!explosionSrc.isPlaying)
        {
            explosionSrc.Play();
        }
        
        
    }
	
	// Update is called once per frame
	void Update () {
        if (!explosionSrc.isPlaying)
            Destroy(this.gameObject);
    }
}
