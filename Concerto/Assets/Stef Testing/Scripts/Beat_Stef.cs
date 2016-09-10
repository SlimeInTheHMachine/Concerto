using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Beat_Stef : MonoBehaviour
{

    GameObject beatManager;
    public bool onTime;
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

    [SerializeField]
    private GameObject LBar;
    [SerializeField]
    private GameObject RBar;
    private List<GameObject> Bars;
    public float dist;
    public double beatTime;

    

    // Use this for initialization
    void Start()
    {
        //Gets the time to be used for beats from the Beat Manager
        beatManager = GameObject.Find("BeatManager");
        beatTime = beatManager.GetComponent<BeatManager>().BeatTime;

        startTime = Time.time;
        score.text = "" + scoreNum;
        timeBetweenSizeChange = (float)beatTime / 2;
        //Debug.Log(timeBetweenSizeChange2);
        growing = true;
        source = GetComponent<AudioSource>();
        colorControl = gameObject.GetComponent<SpriteRenderer>();
        Bars = new List<GameObject>();


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Oscillation
        if (growing)
        {

            float sizeChanged = (Time.time - startTime) * transform.localScale.x / timeBetweenSizeChange;
            float t = sizeChanged / endSize.x;
            transform.localScale = Vector2.Lerp(startSize, endSize, t);

            if (transform.localScale.x >= endSize.x - marginOfError && source.isPlaying == false)
            {
                //Debug.Log("Test");
                GameObject tempLBar = Instantiate(LBar);
                GameObject tempRBar = Instantiate(RBar);
                Bars.Add(tempLBar);
                Bars.Add(tempRBar);

                float spdToMove = dist / timeBetweenBeats / 200.0f;

                Vector3 transL = new Vector3(transform.position.x - dist, transform.position.y, transform.position.z);
                Vector3 transR = new Vector3(transform.position.x + dist, transform.position.y, transform.position.z);

                tempLBar.transform.position = transL;
                tempRBar.transform.position = transR;
                tempLBar.GetComponent<Bar>().xspd = spdToMove;
                tempRBar.GetComponent<Bar>().xspd = -spdToMove;

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
            colorControl.color = new Color(0.4f, 1.0f, 0.4f);
            if (Input.GetButtonDown("Jump"))
            {
                shake = 0.3f;
                scoreNum++;
                score.text = "" + scoreNum;

            }
        }
        else
        {
            //This doesn't do anything!
            colorControl.color = new Color(0.1f, 0.6f, 0.9f);//(0.0f, 1.0f, 0.0f);
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
    void toggleTimer()
    {
        onTime = !onTime;
    }
}
