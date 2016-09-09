using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class OnBeatRose: MonoBehaviour {

    public bool growing;
    private float timeBetweenSizeChange;
    public float timeBetweenBeats;
    public float marginOfError;
    public Vector2 startSize;
    public Vector2 endSize;
    private float startTime;
    private SpriteRenderer colorControl;
    public Transform cameraPos;

    public int scoreNum;
    public Text score;

    public AudioSource source;
    public AudioClip beat;

    public float shake;
    public float shakeAmount;
    public float decreaseFactor;


    public bool Pressable = false;
    // Use this for initialization
    void Start() {
        startTime = Time.time;
        score.text = "" + scoreNum;
        timeBetweenSizeChange = timeBetweenBeats / 2;
        growing = true;
        source = GetComponent<AudioSource>();
        colorControl = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        //Oscillation
        if (growing)
        {
            float sizeChanged = (Time.time - startTime) * transform.localScale.x / timeBetweenSizeChange;
            float t = sizeChanged / endSize.x;
            transform.localScale = Vector2.Lerp(startSize, endSize, t);

            if (transform.localScale.x >= endSize.x - marginOfError && source.isPlaying == false)
            {
                source.PlayOneShot(beat);
            }

            if (transform.localScale.x >= endSize.x)
            {
                growing = false;
                startTime = Time.time;
            }
        }
        else
        {
            float sizeChanged = (Time.time - startTime) * transform.localScale.x / timeBetweenSizeChange;
            float t = sizeChanged / startSize.x;
            transform.localScale = Vector2.Lerp(endSize, startSize, t);
            if (transform.localScale.x <= startSize.x)
            {
                growing = true;
                startTime = Time.time;
            }
        }


    }
    void Update()
    {
        //Input
        if (transform.localScale.x >= endSize.x - marginOfError)
        {
          
            Pressable = true;
            Debug.Log("Is pressable? " + Pressable);
            colorControl.color = new Color(0.0f, 255.0f, 0.0f);
            if (Input.GetButtonDown("Jump"))
            {
                shake = 0.3f;
                scoreNum++;
                score.text = "" + scoreNum;
                
            }
        }
        else
        {
            Pressable = false;
            Debug.Log("Is pressable? " + Pressable);
            colorControl.color = new Color(255.0f, 0.0f, 0.0f);
        }
        //Screen Shake 
        if (shake > 0)
        {
            cameraPos.localPosition = Random.insideUnitCircle * shakeAmount;
            shake -= Time.deltaTime * decreaseFactor;

        }
        else
        {
            shake = 0.0f;
        }
    }
}
