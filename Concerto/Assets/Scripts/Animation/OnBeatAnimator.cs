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
        public bool loop;
        public float framesPerSec;
        public float animDuration
        {
            get { return frames.Length * framesPerSec; }
            set { framesPerSec = frames.Length / value; }
        }

        public float FramesPerSec
        {
            get { return framesPerSec; }
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
    private SpriteRenderer spriteRen;
    private Animation currentAnim;
    private bool isPlaying;
    private float secsPerFrame;
    private float nextFrameTime;
    #endregion
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------
    #region Editor Support
    /// <summary>
    /// Fix the sprite order for multiple sprites being dragged in
    /// </summary>
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
        //make animations happen on the beat
        for (int i = 0; i < animations.Count(); i++)
        {
            animations[i].animDuration = (float)BeatMan.instance.BeatTime;
        }
        //play the first animation
        if (animations.Count > 0)
            PlayByIndex(0);
    }

    // Update is called once per frame
    void Update()
    {
        //Play an animation if you can and should
        if (!isPlaying || Time.time < nextFrameTime || spriteRen == null)
            return;
        //Move to the next frame
        currentFrame++;
        //Loop if you should
        if (currentFrame >= currentAnim.frames.Length)
        {
            if (!currentAnim.loop)
            {
                isPlaying = false;
                return;
            }
            currentFrame = 0;
        }
        //Update the to the correct frame
        spriteRen.sprite = currentAnim.frames[currentFrame];
        //calculate when the next time the frame should update is.
        nextFrameTime += secsPerFrame;
    }
    #endregion
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------
    #region Public Methods
    /// <summary>
    /// Play the specified animation
    /// </summary>
    /// <param name="name"></param>
    public void Play(string name)
    {
        
        int index = animations.FindIndex(a => a.name == name);
        if (index < 0)
            Debug.LogError(gameObject + ": No such animation: " + name);
        else
            PlayByIndex(index);
    }

    /// <summary>
    /// Play the correct animation Frame
    /// </summary>
    /// <param name="index"></param>
    public void PlayByIndex(int index)
    {
        if (index < 0)
            return;
        Animation anim = animations[index];

        currentAnim = anim;

        secsPerFrame = 1f / anim.FramesPerSec;
        currentFrame = -1;
        isPlaying = true;
        nextFrameTime = Time.time;
    }

    /// <summary>
    /// Stop the animation
    /// </summary>
    public void Stop()
    {
        isPlaying = false;
    }

    /// <summary>
    /// Restart the animation
    /// </summary>
    public void Resume()
    {
        isPlaying = true;
        nextFrameTime = Time.time + secsPerFrame;
    }
    #endregion
}
