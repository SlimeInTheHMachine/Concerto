using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BeatAnimationScript : MonoBehaviour {

	/// <summary>
	/// The attached animator that controls shrinking/growing animation
	/// </summary>
	private Animator animationComp;
	/// <summary>
	/// The attached sound source component, with a beat sound specified as its default.
	/// </summary>
	private AudioSource source;
	/// <summary>
	/// The image component of this object
	/// </summary>
	private Image thisImage;
	/// <summary>
	/// The color during OnTime.
	/// </summary>
	public Color BeatColor;
	/// <summary>
	/// The color outside of OnTime
	/// </summary>
	public Color RestColor;

	// Use this for initialization
	void Start () {
		//Find attached components
		animationComp = this.GetComponent<Animator> ();
		source = this.GetComponent<AudioSource> ();
		thisImage = this.GetComponent<Image> ();
		//Subscribe to onBeat()
		BeatManager.onBeat += startAnimation;
	}

	void Update()
	{
		if (BeatManager.instance.onTime)
			thisImage.color = BeatColor;
		else
			thisImage.color = RestColor;
	}

	void startAnimation()
	{
		Debug.Log ("startAnimation() called");
		//Play the animation in the Animator Component
		animationComp.Play (0);
		//Play the sound clip specified in the AudioSource component
		source.PlayOneShot(source.clip);
	}
}
