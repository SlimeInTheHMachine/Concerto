using UnityEngine;
using System;
using System.Collections.Generic;



public class AudioMan : MonoBehaviour
{
    public AudioClip mainBeat;
    public AudioClip subBeat;
    public bool alternateQuery;
    private bool playingMainSound;

    private List<AudioClip> clips;
    private List<AudioClip> liveClips;
    private AudioSource audioSrc;
    private AudioSource liveAudioSrc;
    private List<float> liveClipsPitch;

    protected AudioMan() { }
    //static instance of BeatManager
    public static AudioMan instance = null;
    private void Awake()
    {
        //Check if instance exists
        if (instance == null)
            //If not, assign this to it.
            instance = this;
        else if (instance != this)
            //If so (somehow), destroy this object.
            Destroy(gameObject);


        DontDestroyOnLoad(audioSrc);
    }


    // Use this for initialization
    void Start () {
        clips = new List<AudioClip>();
        liveClips = new List<AudioClip>();
        //potential pitch shift stuff later
        liveClipsPitch = new List<float>();

        clips.Add(mainBeat);
        //clips.Add(subBeat);

        audioSrc = gameObject.AddComponent<AudioSource>();
        liveAudioSrc = gameObject.AddComponent<AudioSource>();

        if (!alternateQuery)
        {
            BeatMan.startBeat += PlayLongBeatAudio;
            BeatMan.offBeat += PlayOffBeatAudio;
        }
        else
        {
            BeatMan.startBeat += PlayShortBeatAudio;
        }
	}

    void Update()
    {
        if (liveClips.Count > 0)
        {
            for (int i = 0; i < liveClips.Count; i++)
                liveAudioSrc.PlayOneShot(liveClips[i]);
            liveClips.Clear();
        }
    }

    void PlayLongBeatAudio()
    {
        for (int i = 0; i < clips.Count; i++)
            audioSrc.PlayOneShot(clips[i]);
        clips.Clear();
        clips.Add(subBeat);
    }

    void PlayShortBeatAudio()
    {
        for (int i = 0; i < clips.Count; i++)
            audioSrc.PlayOneShot(clips[i]);

        clips.Clear();

        if (playingMainSound)
        {
            clips.Add(mainBeat);
            playingMainSound = false;
        }
        else
        {
            clips.Add(subBeat);
            playingMainSound = true;
        }
        
    }

    void PlayOffBeatAudio()
    {
        for (int i = 0; i < clips.Count; i++)
            audioSrc.PlayOneShot(clips[i]);

        clips.Clear();
        clips.Add(mainBeat);
    }

    public void AddClipToQueue(AudioClip aud)
    {
        clips.Add(aud);
    }

    public void AddClipToLiveQueue(AudioClip aud)
    {
        liveClips.Add(aud);
    }
}
