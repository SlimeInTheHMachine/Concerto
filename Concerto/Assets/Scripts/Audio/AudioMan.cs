using UnityEngine;
using System;



public class AudioMan : MonoBehaviour
{
    private AudioSource audioSrc;
    // Use this for initialization
    void Start () {
        audioSrc = transform.GetComponent<AudioSource>();
        BeatMan.startBeat += PlayAudio;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void PlayAudio()
    {
        audioSrc.Play();
    }
}
