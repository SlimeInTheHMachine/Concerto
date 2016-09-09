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
    private Rigidbody2D rig;
    private Vector2 bottomRaycastOrigin;
    private Vector2 lastVelocity;
    
	// Use this for initialization
	void Start () {
        //Layermask for platforms, used for raycasting
        platformLayerMask = LayerMask.GetMask("Platform");
        rig = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        //Raycast to use platforms
        bottomRaycastOrigin = new Vector2(transform.position.x, transform.position.y) - offset;

        //Using raycasts and layermasks we can do basic collisions for platforms relatively quickly
        RaycastHit2D rayHit = Physics2D.Raycast(bottomRaycastOrigin, Vector2.down, bottomRaycastLength, platformLayerMask.value);
        if (rayHit.collider != null)
        {
            //Physics Forces on platform
            Vector2 normalForce = new Vector2(0f, (9.8f * (rig.gravityScale * rig.mass)));
            rig.AddForce(normalForce);
            if (Mathf.Abs(rig.mass * ((lastVelocity.x - rig.velocity.x) / Time.fixedDeltaTime)) > minClampForce.x)
            {
                Vector2 friction = new Vector2(normalForce.y * frictionCoff, 0f);
                if (rig.velocity.x < 0f)
                    rig.AddForce(friction);
                else
                    rig.AddForce(-friction);
            }

            //Stop the object from falling through the platform
            if (rig.velocity.y <= 0f)
            {
                //Compensate for force built up in air
                rig.velocity = new Vector2(rig.velocity.x, 0f);
            }
        }

        //Keep forces from being obscene
        ClampForce();
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
        }
        else
        {
            //Jump in midair
            if (Input.GetButtonDown("Jump") && secondJump)
            {
                rig.velocity = new Vector2(rig.velocity.x, 0f);
                rig.AddForce(jumpForce);
                secondJump = false;
            }
        }

        //Chaos CONTROOOLS!!!

        if (Input.GetAxis("Horizontal") > 0)
            rig.AddForce(movementForce);
        else if(Input.GetAxis("Horizontal") < 0)
            rig.AddForce(-movementForce);

        lastVelocity = rig.velocity;
    }

    //#JelloPunchesBack
    //Keep forces in check with equal and opposite
    void ClampForce()
    {
        
        Vector2 tempForce = new Vector2((rig.mass *((lastVelocity.x - rig.velocity.x) / Time.fixedDeltaTime)), (rig.mass * ((lastVelocity.y - rig.velocity.y) / Time.fixedDeltaTime)));
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
            else
                addForceVector.y -= (tempForce.y + maxClampForce.y);
        }

        //Something limiting xforces // tempforce = 0 //why //#gah
        Debug.Log(tempForce);
        if (Mathf.Abs(tempForce.x) < minClampForce.x)
            rig.velocity = new Vector2(0f, rig.velocity.y);

        rig.AddForce(addForceVector);
    }
}
