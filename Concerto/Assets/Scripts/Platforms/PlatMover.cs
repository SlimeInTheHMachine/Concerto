using UnityEngine;
using System.Collections;

public class PlatMover : MonoBehaviour
{
    public bool moveForward;
    public float up;
    public float right;
    public float lerpTime;
    GameObject beatManager;
    private double beatTime;

    private float startTime;
    private float timeBetweenPosChange;
    private Vector3 startPos;
    private Vector3 endPosHor;
    private Vector3 endPosVer;

    public Sprite sprite1;
    public Sprite sprite2;

    void Start()
    {
        beatManager = GameObject.Find("BeatManager");
        beatTime = beatManager.GetComponent<BeatMan>().BeatTime;
        startTime = Time.time;
        timeBetweenPosChange = (float)beatTime;
        moveForward = true;
        startPos = transform.position;
        endPosVer = new Vector3(startPos.x, startPos.y + up);
        endPosHor = new Vector3(startPos.x + right, startPos.y);
    }
    public void moveVer()
    {
        //Update next location to move to //Supersmooth lerp t = (lerpTime * Time.fixedDeltaTime),  (lerp = t*t*t * (t * (6f*t - 15f) + 10f))


        if (moveForward)
        {

            float PosChanged = (Time.time - startTime) * Mathf.Abs(transform.position.y) / timeBetweenPosChange;
            float t = PosChanged / Mathf.Abs(endPosVer.y);
            //float t= ((lerpTime * Time.fixedDeltaTime) * (lerpTime * Time.fixedDeltaTime) * (lerpTime * Time.fixedDeltaTime)) * ((lerpTime * Time.fixedDeltaTime) * ((6f * (lerpTime * Time.fixedDeltaTime)) - 15f) + 10f);
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
            //float t = ((lerpTime * Time.fixedDeltaTime) * (lerpTime * Time.fixedDeltaTime) * (lerpTime * Time.fixedDeltaTime)) * ((lerpTime * Time.fixedDeltaTime) * ((6f * (lerpTime * Time.fixedDeltaTime)) - 15f) + 10f);
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