using UnityEngine;
using System.Collections;

public class Bars : MonoBehaviour {


    public float xspd;

    public GameObject beatMeter;
    // Use this for initialization
    void Start()
    {
        beatMeter = GameObject.Find("Beat");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x + xspd, transform.position.y, transform.position.z);

        if (xspd > 0 && transform.position.x > beatMeter.transform.position.x)
        {
            Destroy(gameObject);
        }
        if (xspd < 0 && transform.position.x < beatMeter.transform.position.x)
        {
            Destroy(gameObject);
        }
    }
}
