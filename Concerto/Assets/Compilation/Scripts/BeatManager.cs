using UnityEngine;
using System.Collections;

public class BeatManager : MonoBehaviour
{
    public bool onTime;
    public float marginOfError;
    //Keeps a running track of time passing
    private float timeCounter;
    //the time in seconds for a beat.
    public double BeatTime;
	

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
	/// Occurs when on beat. Subscribers must be void.
	/// </summary>
	public static event BeatFunction onBeat;

    //Prevents other instances of BeatManager, since the constructor is restricted
    protected BeatManager (){}
	//static instance of BeatManager
	public static BeatManager instance = null;

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
        onBeat += beatTimer;        
    }
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void FixedUpdate()
	{
		//Keep track of more time passing
		timeCounter += Time.fixedDeltaTime;
		//Debug.Log(timeCounter);
		//If enough time has passed for a beat
		if (timeCounter >= (float)BeatTime)
		{
            //Potentially causing beat acceleration issue
            //timeCounter-= (float)BeatTime;

            //Set the timer back to nothing and start again.
            timeCounter = 0f;

			//Trigger onbeat event
			onBeat();
		}

        //Checks whether the beat is accepting input within a margin of error
        if (timeCounter >= BeatTime + marginOfError || timeCounter <= BeatTime - marginOfError)
            onTime = true;
        else
            onTime = false;
    }

    private void beatTimer()
    {
        Debug.Log("Beat Plays at " + Time.fixedTime);
    }
}
