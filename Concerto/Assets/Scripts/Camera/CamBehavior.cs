using UnityEngine;
using System.Collections;

public class CamBehavior : MonoBehaviour {

    public GameObject player;
    public Canvas can;
    public float bobModAmount;
    private float bobMod = 0f;
    public float lerpTime;
    private Vector3 lerpDestination;

    void Start()
    {
        BeatMan.startBeat += CamBobUp;
        BeatMan.onBeat += CamBobDown;
        BeatMan.endBeat += CamReset;
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        transform.position = player.transform.position;
        lerpTime = (float)BeatMan.instance.inputMarginOfError;
        lerpDestination = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
    }
    // Update is called once per frame
    void Update()
    {
        //move if we're not there
        //transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        lerpDestination = new Vector3(player.transform.position.x, player.transform.position.y + bobMod, transform.position.z);
        transform.position = Vector2.Lerp(transform.position, lerpDestination, lerpTime);
        can.transform.position = transform.position;
    }

    void CamBobUp()
    {
        bobMod += bobModAmount;
        //lerpDestination = new Vector3(player.transform.position.x, player.transform.position.y + bobMod, transform.position.z);
    }

    void CamBobDown()
    {
        //lerpDestination = new Vector3(player.transform.position.x, player.transform.position.y - bobMod, transform.position.z);
        bobMod = -bobModAmount;
    }

    void CamReset()
    {
        //lerpDestination = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        bobMod = 0;
    }
}
