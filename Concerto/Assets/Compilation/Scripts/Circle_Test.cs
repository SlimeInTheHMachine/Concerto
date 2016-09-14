using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Circle_Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        this.GetComponent<RectTransform>().localScale += new Vector3(3f,0,0);

    }
}
