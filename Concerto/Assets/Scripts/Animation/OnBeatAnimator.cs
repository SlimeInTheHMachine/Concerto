using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class OnBeatAnimator : MonoBehaviour
{
    #region Public Properties
    [System.Serializable]
    public class Animation
    {
        public string name;
        public Sprite[] frames;
        public float framesPerSec;
        public bool loop;

        public float animDuration
        {
            get { return frames.Length * framesPerSec; }
            set { framesPerSec = value / frames.Length; }
        }


    }

    public List<Animation> animations = new List<Animation>();
    

    [HideInInspector]
    public int currentFrame;

    [HideInInspector]
    //Need a selector for the current animation still
    public bool done { get { return currentFrame >= currentAnim.frames.Length; } }

    [HideInInspector]
    //Need a bool for if anim is playing right now or not still
    public bool playing { get { return isPlaying; } }


    #endregion
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------
    #region Private Properties
    SpriteRenderer spriteRen;
    Animation currentAnim;
    bool isPlaying;
    float secsPerFrame;
    float nextFrameTime;
    #endregion
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------
    #region Editor Support
    [ContextMenu("Sort All Frames by Name")]
    void DoSort()
    {
        foreach (Animation anim in animations)
        {
            System.Array.Sort(anim.frames, (a, b) => a.name.CompareTo(b.name));
        }
        Debug.Log(gameObject.name + " animation frames have been sorted alphabetically.");
    }
    #endregion
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------
    #region MonoBehavior/Unity Methods

    // Use this for initialization
    void Start()
    {
        spriteRen = GetComponent<SpriteRenderer>();
        if (animations.Count > 0)
            PlayByIndex(0);

        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaying || Time.time < nextFrameTime || spriteRen == null)
            return;
        currentFrame++;
        if (currentFrame >= currentAnim.frames.Length)
        {
            if (!currentAnim.loop)
            {
                isPlaying = false;
                return;
            }
            currentFrame = 0;
        }
        spriteRen.sprite = currentAnim.frames[currentFrame];
        nextFrameTime += secsPerFrame;
    }
    #endregion
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------
    #region Public Methods
    public void Play(string name)
    {
        int index = animations.FindIndex(a => a.name == name);
        if (index < 0)
            Debug.LogError(gameObject + ": No such animation: " + name);
        else
            PlayByIndex(index);
    }

    public void PlayByIndex(int index)
    {
        if (index < 0)
            return;
        Animation anim = animations[index];

        currentAnim = anim;

        secsPerFrame = 1f / anim.framesPerSec;
        currentFrame = -1;
        isPlaying = true;
        nextFrameTime = Time.time;
    }

    public void Stop()
    {
        isPlaying = false;
    }

    public void Resume()
    {
        isPlaying = true;
        nextFrameTime = Time.time + secsPerFrame;
    }
    #endregion
}
