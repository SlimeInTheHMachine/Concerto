using UnityEngine;
using System.Collections;

public class PlatMoverVertical : MonoBehaviour
{
    bool moveForward;
    public float up;
    GameObject beatManager;

    private float startTime;
    private float timeBetweenPosChange;
    private double beatTime;
    private Vector3 startPos;
    private Vector3 endPos;

    // Use this for initialization
    void Start()
    {
        beatManager = GameObject.Find("BeatManager");
        beatTime = beatManager.GetComponent<BeatManager>().BeatTime;
        startTime = Time.time;
        timeBetweenPosChange = (float)beatTime;
        moveForward = true;
        startPos = transform.position;
        endPos = new Vector3(startPos.x , startPos.y + up);
    }

    void FixedUpdate()
    {
        if (moveForward)
        {

            float PosChanged = (Time.time - startTime) * Mathf.Abs(transform.position.y) / timeBetweenPosChange;
            float t = PosChanged / Mathf.Abs(endPos.y);
            transform.position = Vector3.Lerp(startPos, endPos, t);

            if (transform.position.y == endPos.y)
            {
                moveForward = false;
                startTime = Time.time;
            }
        }
        else
        {
            float PosChanged = (Time.time - startTime) * Mathf.Abs(transform.position.y) / timeBetweenPosChange;
            float t = PosChanged / Mathf.Abs(startPos.y);
            transform.position = Vector2.Lerp(endPos, startPos, t);

            if (transform.position.y == startPos.y)
            {
                moveForward = true;
                startTime = Time.time;
            }
        }
    }
}
