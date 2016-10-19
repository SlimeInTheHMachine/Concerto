using UnityEngine;
using System.Collections;

public class BeatMan : MonoBehaviour
{
    public bool onTime;
    public float marginOfError;
    //Keeps a running track of time passing
    private float timeCounter;
    //the time in seconds for a beat.
    public float beatTime;

    private bool started, hitBeat;
    private float nextBeat;
	
    /// <summary>
    /// Gets the timeCounter
    /// </summary>
	public float TimeCounter
	{
		get { return timeCounter;}
	}

    public float BeatTime
    {
        get { return beatTime; }
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
        nextBeat = Time.time + beatTime;
		//Sets this to not be destroyed when reloading scene
		//DontDestroyOnLoad(gameObject);
	}

	void FixedUpdate()
	{
		//Keep track of more time passing
		timeCounter += Time.fixedDeltaTime;
       // Debug.Log(timeCounter);

        //Beginning of Beat - Margin
        if (!started && timeCounter >= nextBeat - marginOfError)
        {
            onTime = true;
            started = true;
            startBeat();
        }

        //Actual Beat
        if (!hitBeat && timeCounter >= nextBeat)
		{
            hitBeat = true;
            onBeat();
            Debug.Log("ONBEAT");
		}

        //End of Beat + Margin
        if (timeCounter >= nextBeat + marginOfError )
        {
            Debug.Log(timeCounter);
            nextBeat = Time.time + beatTime;
            endBeat();
            onTime = false;
            started = false;
            hitBeat = false;
        }
    }
}
