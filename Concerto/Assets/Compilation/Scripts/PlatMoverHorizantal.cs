using UnityEngine;
using System.Collections;

public class PlatMoverHorizantal : MonoBehaviour {
    public bool moveForward;
    public float right;
    GameObject beatManager;

    private float startTime;
    private float timeBetweenPosChange;
    private double beatTime;
    private Vector3 startPos;
    private Vector3 endPos;
    
    void Start()
    {
        beatManager = GameObject.Find("BeatManager");
        beatTime = beatManager.GetComponent<BlaneBeatMan>().BeatTime;
        startTime = Time.time;
        timeBetweenPosChange = (float)beatTime/2;
        moveForward = true;
        startPos = transform.position;
        endPos = new Vector3(startPos.x + right, startPos.y);
    }
    public void move()
    {
   
        if (moveForward)
        {

            float PosChanged = (Time.time - startTime) * Mathf.Abs(transform.position.x) / timeBetweenPosChange;
            float t = PosChanged / Mathf.Abs(endPos.x);
            transform.position = Vector3.Lerp(startPos, endPos, t);

            if (transform.position.x == endPos.x)
            {
                moveForward = false;
                startTime = Time.time;
            }
        }
        else
        {
            float PosChanged = (Time.time - startTime) * Mathf.Abs(transform.position.x) / timeBetweenPosChange;
            float t = PosChanged / Mathf.Abs(startPos.x);
            transform.position = Vector2.Lerp(endPos, startPos, t);

            if (transform.position.x == startPos.x )
            {
                moveForward = true;
                startTime = Time.time;
            }
        }
    }
}
