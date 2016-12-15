using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UICircleTwin : MonoBehaviour {

	/// <summary>
	/// The attached animator that controls shrinking/growing animation
	/// </summary>
	private Animator animationComp;
	/// <summary>
	/// The image component of this object
	/// </summary>
	private Image thisImage;
	/// <summary>
	/// The color when this is the smaller metronome circle.
	/// </summary>
	public Color InnerColor;
	/// <summary>
	/// The color when this is the outside metronome circle.
	/// </summary>	
	public Color OuterColor;
	[SerializeField]
	bool innerColor;

	// Use this for initialization
	void Start () {
		//Find attached components
		animationComp = this.GetComponent<Animator> ();
		thisImage = this.GetComponent<Image> ();
		//Subscribe to onBeat()
		BeatMan.onBeat += startAnimation;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void startAnimation()
	{
		if (innerColor) {
			thisImage.color = InnerColor;
			//Debug.Log ("startAnimation() called");
			//Play the animation in the Animator Component
			animationComp.Play (0);
		} else {
			thisImage.color = OuterColor;
			animationComp.StopPlayback();
		}

		innerColor = !innerColor;
	}
}
