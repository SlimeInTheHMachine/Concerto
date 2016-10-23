using UnityEngine;
using System.Collections;

public class UIBar : MonoBehaviour {

	public bool active;
	//public bool Active {get{return active;}}
	public float XSpd;
	public float YSpd;
	/// <summary>
	/// An integer that decreases every beat. The bar is set inactive when it hits zero.
	/// </summary>
	public int BeatsToLive;
	//public float distToLive;
	
	//public accessor
	public Vector3 Spd
	{
		get {return new Vector3(XSpd,YSpd,0);}
		set {XSpd = value.x; YSpd = value.y;}
	}
	
	// Use this for initialization
	void Start () {
		active = false;
		XSpd = 0;
		YSpd = 0;
	}
	
	/// <summary>
	/// Function to get called by bar manager, ticks down beatsToLive.
	/// </summary>
	public void onBeat()
	{
		BeatsToLive--;
		if (BeatsToLive <= 0)
		{
			active = false;
		}
		//unsubscribe here
	}
	
	public void deActivate()
	{
		//reset to starting position TODO: make that the right position
		transform.position = new Vector3();
		active = false;
		XSpd = 0;
		YSpd = 0;
	}
}
