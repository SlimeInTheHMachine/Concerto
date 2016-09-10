using UnityEngine;
using System.Collections;

public class TestSubscriber : MonoBehaviour {
	
	// Use this for initialization
	void Start ()
	{
		//beatManager.GetComponent<BeatManager>().BeatFunction test = subscriberTest;
		BeatManager.onBeat += subscriberTest;
	}

	void subscriberTest()
	{
		Debug.Log ("Entered subscriber function");
	}
}
