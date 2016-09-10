using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BeatTimerDebugText : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//BeatManager.onBeat += UpdateText;
	}
	
	// Update is called once per frame
	void Update () {
		this.GetComponent<Text> ().text = BeatManager.instance.TimeCounter.ToString();
	}


}
