using UnityEngine;
using System.Collections;

public class BeatManager : MonoBehaviour
{
    public bool onTime;
    public float marginOfError;

    [SerializeField]
	//the time in seconds for a beat.
	public double BeatTime;
	//Keeps a running track of time passing
	private float timeCounter;

    /// <summary>
    /// Gets the timeCounter
    /// </summary>
	public float TimeCounter
	{
		get { return timeCounter;}
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
        onBeat += beatTimer;

        //Gets the time to be used for beats from the Beat Manager
        beat = GameObject.Find("BeatManager");
    }
	
	// Update is called once per frame
	void Update ()
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
			timeCounter-= (float)BeatTime;
			//Trigger onbeat event
			onBeat();
		}

        //Checks whether the beat is accepting input within a margin of error
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
        Debug.Log("Beat Plays");
    }
}
