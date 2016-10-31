using UnityEngine;
using System.Collections;

public class BeatMan : MonoBehaviour
{
    public bool onTime;
    private bool started, hitBeat;

    //the time in seconds for a beat.
    public double timeBetweenBeats;
    public double inputMarginOfError;
    public double maxInputMarginOfError, minInputMarginOfError;
    public double nextBeat;

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
    }

	void FixedUpdate()
	{
        //Beginning of Beat - Margin
        if (!started && Time.time >= nextBeat - inputMarginOfError)
        {
            onTime = true;
            started = true;
            startBeat();
        }

        //Actual Beat
        if (!hitBeat && Time.time >= nextBeat)
		{
            hitBeat = true;
            onBeat();
            //Debug.Log("ONBEAT");
		}

        //End of Beat + Margin
        if (Time.time >= nextBeat + inputMarginOfError )
        {
            nextBeat += timeBetweenBeats;
            
            endBeat();
            onTime = false;
            started = false;
            hitBeat = false;
        }
    }
}
