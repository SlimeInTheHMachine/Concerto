using UnityEngine;
using System.Collections;

public class PlatformerBehavior : MonoBehaviour {

    //Public Variables
    public float bottomRaycastLength;
    public Vector2 jumpForce;
    public Vector2 movementForce;
    public Vector2 maxClampForce;
    public Vector2 minClampForce;
    public bool secondJump;
    public float frictionCoff;
    public Vector2 offset;

    //Private Variables
    private LayerMask platformLayerMask;
    private BoxCollider2D box;
    private Rigidbody2D rig;
    private Vector2 bottomRaycastOrigin;
    
	// Use this for initialization
	void Start () {
        //Layermask for platforms, used for raycasting
        platformLayerMask = LayerMask.GetMask("Platform");
        box = GetComponent<BoxCollider2D>();
        rig = GetComponent<Rigidbody2D>();
    }
	
    //general Update. Need to move physics and other things to respective update functions
    void Update()
    {
        //Raycast to use platforms
        bottomRaycastOrigin = new Vector2(transform.position.x, transform.position.y) - offset;

        //Using raycasts and layermasks we can do basic collisions for platforms relatively quickly
        RaycastHit2D rayHit = Physics2D.Raycast(bottomRaycastOrigin, Vector2.down, bottomRaycastLength, platformLayerMask.value);
        if (rayHit.collider != null)
        {
            //Show the object raycast is returning
            Debug.DrawLine(bottomRaycastOrigin, rayHit.collider.transform.position, Color.red);
            if (Input.GetButtonDown("Jump"))
            {
                //Chaos CONTROOOLS!!!
                rig.AddForce(jumpForce);
                secondJump = true;
            }
            //Stop the object from falling through the platform
            if(rig.velocity.y <= 0f)
            {
                //Compensate for force built up in air
                Vector2 tempForce = new Vector2(0, -(rig.mass * (rig.velocity.y / Time.deltaTime)));
                
                //Stop bouncing //needs to be in fixed or physics
                if (Mathf.Abs(tempForce.y) < minClampForce.y)
                    rig.velocity = new Vector2(rig.velocity.x, 0f);
                else
                    rig.AddForce(tempForce);
            }

            //Physics Forces on platform
            Vector2 normalForce = new Vector2(0f, (9.8f * rig.gravityScale));
            rig.AddForce(normalForce);
            if (Mathf.Abs(rig.mass * (rig.velocity.x / Time.deltaTime)) > minClampForce.x)
            {
                Vector2 friction = new Vector2(normalForce.y * frictionCoff, 0f);
                if(rig.velocity.x < 0f)
                    rig.AddForce(friction);
                else
                    rig.AddForce(-friction);
            }
        }
        else
        {
            //Jump in midair
            if (Input.GetButtonDown("Jump") && secondJump)
            {
                
                rig.AddForce(jumpForce);
                secondJump = false;
            }
        }

        //Chaos CONTROOOLS!!!

        if (Input.GetAxis("Horizontal") > 0)
            rig.AddForce(movementForce);
        else if(Input.GetAxis("Horizontal") < 0)
            rig.AddForce(-movementForce);

        //Keep forces from being obscene
        ClampForce();

    }

    //#JelloPunchesBack
    //Keep forces in check with equal and opposite
    void ClampForce()
    {
        Vector2 tempForce = new Vector2((rig.mass *(rig.velocity.x/Time.deltaTime)), (rig.mass * (rig.velocity.y / Time.deltaTime)));
        Vector2 addForceVector = new Vector2(0,0);

        if( Mathf.Abs(tempForce.x) > maxClampForce.x)
        {
            if (tempForce.x > 0f)
                addForceVector.x -= (tempForce.x - maxClampForce.x);
            else
                addForceVector.x -= (tempForce.x + maxClampForce.x);
        }

        if (Mathf.Abs(tempForce.y) > maxClampForce.y)
        {
            if (tempForce.y > 0f)
                addForceVector.y -= (tempForce.y - maxClampForce.y);
        }

        if (Mathf.Abs(tempForce.x) < minClampForce.x)
            rig.velocity = new Vector2(0f, rig.velocity.y);

        rig.AddForce(addForceVector);
    }
}
