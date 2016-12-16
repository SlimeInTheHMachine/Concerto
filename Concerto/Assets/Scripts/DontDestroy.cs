using UnityEngine;
using System.Collections;

public class DontDestroy : MonoBehaviour {
    // Use this for initialization
    void Awake()
    {
        DontDestroyOnLoad(this);
        foreach (Transform child in this.transform)
        { DontDestroyOnLoad(child.gameObject); }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update () {
	
	}
}
