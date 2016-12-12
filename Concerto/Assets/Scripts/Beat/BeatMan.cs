using UnityEngine;
using System.Collections;

public class BeatMan : MonoBehaviour
{
    public bool onTime;
    public bool started, hitBeat, offBeatHit;

    //the time in seconds for a beat.
    public double timeBetweenBeats;
    public double inputMarginOfError;
    public double maxInputMarginOfError, minInputMarginOfError;
    public double nextBeat;

    private double offBeatTime;

    public double BeatTime
    {
        get { return timeBetweenBeats; }
    }
	
	//function to call on the beat
	public delegate void BeatFunction();

    /// <summary>
	/// Occurs within the margin before the beat. Subscribers must be void.
	/// </summary>
	public static event BeatFunction startBeat;

    /// <summary>
    /// Occurs when on beat. Subscribers must be void.
    /// </summary>
    public static event BeatFunction onBeat;

    /// <summary>
	/// Occurs within the margin after the beat. Subscribers must be void.
	/// </summary>
	public static event BeatFunction endBeat;

    /// <summary>
	/// Occurs directly between beats. Subscribers must be void.
	/// </summary>
	public static event BeatFunction offBeat;


    //Prevents other instances of BeatManager, since the constructor is restricted
    protected BeatMan (){}
	//static instance of BeatManager
	public static BeatMan instance = null;

	//Awake is before all Start functions
	void Awake()
	{
		//Check if instance exists
		if (instance == null)
			//If not, assign this to it.
			instance = this;
		else if (instance != this)
			//If so (somehow), destroy this object.
			Destroy(gameObject);
	}

    void Start()
    {
        nextBeat = timeBetweenBeats + Time.time + Time.fixedDeltaTime;
        inputMarginOfError = timeBetweenBeats * 0.2;
        if (inputMarginOfError > maxInputMarginOfError)
            inputMarginOfError = maxInputMarginOfError;
        else if(inputMarginOfError < minInputMarginOfError)
            inputMarginOfError = minInputMarginOfError;

        offBeatTime = nextBeat + timeBetweenBeats / 2;
    }

    void FixedUpdate()
	{
       
        //Beginning of Beat - Margin
        if (!started && startBeat != null && Time.time >= nextBeat - inputMarginOfError)
        {
            onTime = true;
            started = true;
            startBeat();
        }

        //Actual Beat
        if (!hitBeat && onBeat != null && Time.time >= nextBeat)
		{
            hitBeat = true;
            onBeat();
            Debug.Log("ONBEAT");
		}

        //End of Beat + Margin
        if (endBeat != null && Time.time >= nextBeat + inputMarginOfError )
        {
            nextBeat += timeBetweenBeats;
            
            onTime = false;
            started = false;
            hitBeat = false;
            offBeatHit = false;
            endBeat(); 
        }

        if(!offBeatHit && offBeat != null && Time.time >= offBeatTime /*- inputMarginOfError*/)
        {
            offBeatTime = nextBeat + timeBetweenBeats / 2;
            offBeatHit = true;
            offBeat();
        }
    }
}
