using UnityEngine;
using System.Collections;

public class CamBehavior : MonoBehaviour {

    public GameObject player;
    public Canvas can;
    public float bobMod;

    private int bobVal;
    bool topBob = true;

    void Start()
    {
        BlaneBeatMan.onBeat += CamBob;
    }

    // Update is called once per frame
    void Update()
    {
        if (bobVal == 0)
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        }
        else
        {
            if(topBob)
            {
                transform.position = new Vector3(player.transform.position.x, player.transform.position.y + bobMod, transform.position.z);
                topBob = false;
            }
            else
            {
                transform.position = new Vector3(player.transform.position.x, player.transform.position.y - bobMod, transform.position.z);
                topBob = true;
                bobVal -= 1;
            }
        }

        can.transform.position = transform.position;
    }

    void CamBob()
    {
        bobVal = 1;
    }
}
