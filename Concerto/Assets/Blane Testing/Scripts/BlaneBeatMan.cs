using UnityEngine;
using System.Collections;

public class BlaneBeatMan : MonoBehaviour
{
    public bool onTime;
    public float marginOfError;
    //Keeps a running track of time passing
    private float timeCounter;
    //the time in seconds for a beat.
    public double BeatTime;

    private bool started, hitBeat;
	
    /// <summary>
    /// Gets the timeCounter
    /// </summary>
	public float TimeCounter
	{
		get { return timeCounter;}
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
    protected BlaneBeatMan (){}
	//static instance of BeatManager
	public static BlaneBeatMan instance = null;

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
            
		//Sets this to not be destroyed when reloading scene
		//DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start ()
	{
        //Add the beatTimer function to the onBeat event
        startBeat += beatTimer1;
        onBeat += beatTimer2;
        endBeat += beatTimer3;
    }

	void FixedUpdate()
	{
		//Keep track of more time passing
		timeCounter += Time.fixedDeltaTime;
        //Debug.Log(timeCounter);

        //Beginning of Beat - Margin
        if (!started && timeCounter >= (float)BeatTime - marginOfError && timeCounter < (float)BeatTime)
        {
            onTime = true;
            started = true;
            startBeat();
        }

        //Actual Beat
        if (!hitBeat && timeCounter >= (float)BeatTime && timeCounter < (float)BeatTime + marginOfError)
		{
			onBeat();
            hitBeat = true;
		}

        //End of Beat + Margin
        if (timeCounter >= (float)BeatTime + marginOfError )
        {
            timeCounter -= (float)BeatTime + marginOfError;
            endBeat();
            onTime = false;
            started = false;
            hitBeat = false;
        }
    }

    private void beatTimer1()
    {
        //if (onTime)
        //  Debug.Log("Start Beat at " + Time.fixedTime);
    }

    private void beatTimer2()
    {
        //if(onTime)
        //   Debug.Log("Beat Plays at " + Time.fixedTime);
    }

    private void beatTimer3()
    {
        //if (onTime)
        //   Debug.Log("End Beat at " + Time.fixedTime);
    }
}
