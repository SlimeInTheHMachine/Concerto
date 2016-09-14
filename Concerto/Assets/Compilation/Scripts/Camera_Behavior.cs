using UnityEngine;
using System.Collections;

public class Camera_Behavior : MonoBehaviour {

    public GameObject player;
    public Canvas can;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        can.transform.position = transform.position;
    }
}
