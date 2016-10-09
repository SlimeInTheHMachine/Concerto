using UnityEngine;
using System.Collections;

public class PlatMover : MonoBehaviour
{
    public bool moveForward;
    public float up;
    public float right;
    GameObject beatManager;
    private double beatTime;

    private float startTime;
    private float timeBetweenPosChange;
    private Vector3 startPos;
    private Vector3 endPosHor;
    private Vector3 endPosVer;

    void Start()
    {
        beatManager = GameObject.Find("BeatManager");
        beatTime = beatManager.GetComponent<BeatMan>().BeatTime;
        startTime = Time.time;
        timeBetweenPosChange = (float)beatTime / 2;
        moveForward = true;
        startPos = transform.position;
        endPosVer = new Vector3(startPos.x, startPos.y + up);
        endPosHor = new Vector3(startPos.x + right, startPos.y);
    }
    public void moveVer()
    {

        if (moveForward)
        {

            float PosChanged = (Time.time - startTime) * Mathf.Abs(transform.position.y) / timeBetweenPosChange;
            float t = PosChanged / Mathf.Abs(endPosVer.y);
            transform.position = Vector3.Lerp(startPos, endPosVer, t);

            if (transform.position.y == endPosVer.y)
            {
                moveForward = false;
                startTime = Time.time;
            }
        }
        else
        {
            float PosChanged = (Time.time - startTime) * Mathf.Abs(transform.position.y) / timeBetweenPosChange;
            float t = PosChanged / Mathf.Abs(startPos.y);
            transform.position = Vector2.Lerp(endPosVer, startPos, t);

            if (transform.position.y == startPos.y)
            {
                moveForward = true;
                startTime = Time.time;
            }
        }
    }

    public void moveHor()
    {

        if (moveForward)
        {

            float PosChanged = (Time.time - startTime) * Mathf.Abs(transform.position.x) / timeBetweenPosChange;
            float t = PosChanged / Mathf.Abs(endPosHor.x);
            transform.position = Vector3.Lerp(startPos, endPosHor, t);

            if (transform.position.x == endPosHor.x)
            {
                moveForward = false;
                startTime = Time.time;
            }
        }
        else
        {
            float PosChanged = (Time.time - startTime) * Mathf.Abs(transform.position.x) / timeBetweenPosChange;
            float t = PosChanged / Mathf.Abs(startPos.x);
            transform.position = Vector2.Lerp(endPosHor, startPos, t);

            if (transform.position.x == startPos.x)
            {
                moveForward = true;
                startTime = Time.time;
            }
        }
    }
}