using UnityEngine;
using System.Collections;

public class BeatAnimationScript : MonoBehaviour {

	private Animator animationComp;
	//[SerializeField]
	//private Animation Grow;
	//[SerializeField]
	//private Animation Shrink;

	// Use this for initialization
	void Start () {
		animationComp = this.GetComponent<Animator> ();
		BeatManager.onBeat += startAnimation;
		//Grow = animationComp.
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void startAnimation()
	{
		Debug.Log ("startAnimation() called");
		animationComp.Play (0);
	}
}
