using UnityEngine;
using System.Collections;

public class button_Script_Credits : MonoBehaviour {
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        Application.LoadLevel("Credits");
    }
}
