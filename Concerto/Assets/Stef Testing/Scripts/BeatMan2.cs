using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BeatMan2 : MonoBehaviour
{
    public bool onTime;
    public float marginOfError;
    //the time in seconds for a beat.
    public double BeatTime;
    //Keeps a running track of time passing
    private float timeCounter;
    public float TimeCounter
    {
        get { return timeCounter; }
    }
    
    //function to call on the beat
    public delegate void BeatFunction();
    //All functions of other beat-relevant objects in the scene
    public System.Collections.Generic.List<BeatFunction> BeatFunctions;
    /// <summary>
    /// Occurs when on beat. Subscribers must be void.
    /// </summary>
    public static event BeatFunction onBeat;
    
    GameObject beat;

    // Use this for initialization
    void Start()
    {
        onBeat += beatTimer;

        //Gets the time to be used for beats from the Beat Manager
        beat = GameObject.Find("BeatManager");

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        //Debug.Log("Fixed Update");
        //Keep track of more time passing
        timeCounter += Time.fixedDeltaTime;
        //Debug.Log(timeCounter);
        //If enough time has passed for a beat
        if (timeCounter >= (float)BeatTime)
        {
            timeCounter -= (float)BeatTime;
            //Trigger onbeat event
            onBeat();
        }

        if (timeCounter >= BeatTime + marginOfError || timeCounter <= BeatTime - marginOfError)
        {
            onTime = true;
        }

        else
        {
            onTime = false;
        }

    }
    
    private void beatTimer()
    {

    }
}
