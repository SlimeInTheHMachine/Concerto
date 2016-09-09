using UnityEngine;
using System.Collections;

public class BeatManager : MonoBehaviour
{

	//the time in seconds for a beat.
	public double BeatTime;
	//Keeps a running track of time passing
	private float timeCounter;
	
	//function to call on the beat
	public delegate void BeatFunction();
	//All functions of other beat-relevant objects in the scene
	public System.Collections.Generic.List<BeatFunction> BeatFunctions;
	//Event that calls BeatFunction
	public static event BeatFunction BeatEvent; 


	// Use this for initialization
	void Start ()
	{


	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void FixedUpdate()
	{
		//Keep track of more time passing
		timeCounter += Time.fixedDeltaTime;
		//If enough time has passed for a beat
		if (timeCounter >= (float)BeatTime)
		{
			timeCounter-= (float)BeatTime;
			//Trigger beat event here
		}
	}

	//Add an event to the list of delegates
	public void addDelegate()
	{
	}
	public void removeDelegate()
	{
	}
}
