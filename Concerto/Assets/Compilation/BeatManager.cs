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
	/// <summary>
	/// Occurs when on beat. Subscribers must be void.
	/// </summary>
	public static event BeatFunction onBeat; 

	// Use this for initialization
	void Start ()
	{
		onBeat += tempTestMethod;
		onBeat += tempTestMethod2;

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
	}

	private void tempTestMethod()
	{
		Debug.Log("Test method triggered");
	}

	private void tempTestMethod2()
	{
		Debug.Log("Test method2 triggered");
	}
}
