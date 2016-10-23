using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MetronomeParent : MonoBehaviour {

	private bool preActive;
	public bool active;

	private List<Image> images;

	// Use this for initialization
	void Start () {
		preActive = active;

		images = new List<Image> ();

		//with each transform
		foreach (Transform child in transform)
		{
			Image image = child.GetComponent<Image>();
			images.Add(image);
			if (!active) 
				image.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (active && !preActive) {
			foreach (Image image in images) {
				image.enabled = true;
			}
		} else if (!active && preActive) {
			foreach (Image image in images) {
				image.enabled = false;
			}
		}

		preActive = active;
	}
}
