using UnityEngine;
using System.Collections;

public class NewPlatScript : MonoBehaviour {

    //Public Variables
    public float bottomRaycastLength;
    public float jumpForce;
    public float movementForce;
    public float frictionCoff;
    public float decelMultiVal;
    public Vector2 maxClampForce;
    public Vector2 minClampForce;
    public Vector2 raycastOffset;
    public bool secondJump;
    public bool tryingToFall = false;

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
        bottomRaycastOrigin = new Vector2(transform.position.x, transform.position.y) - raycastOffset;

        //Using raycasts and layermasks we can do basic collisions for platforms relatively quickly
        RaycastHit2D rayHit = Physics2D.Raycast(bottomRaycastOrigin, Vector2.down, bottomRaycastLength, platformLayerMask.value);
        if (rayHit.collider != null)
        {
            //Physics Forces on platform

            //Normal Force
            Vector2 normalForce = new Vector2(0f, (9.8f * (rig.gravityScale * rig.mass)));
            if (!tryingToFall)
                rig.AddForce(normalForce);

            //Friction Force
            if (Mathf.Abs(rig.mass * ((lastVelocity.x - rig.velocity.x) / Time.fixedDeltaTime)) > minClampForce.x)
            {
                Vector2 frictionForce = new Vector2(normalForce.y * frictionCoff, 0f);
                if (rig.velocity.x < 0f)
                    rig.AddForce(frictionForce);
                else
                    rig.AddForce(-frictionForce);
            }
        }

        //Natural Decel
        if (Input.GetAxis("Horizontal") == 0)
        {
            rig.velocity = new Vector2(rig.velocity.x * decelMultiVal, rig.velocity.y);
        }

        //Keep forces from being obscene
        ClampSpeeds();
        //Keep this for next frame
        lastVelocity = rig.velocity;
    }

	// Update is called once per frame
	void Update () {
        //Raycast to use platforms //Working
        bottomRaycastOrigin = new Vector2(transform.position.x, transform.position.y) - raycastOffset;

        //Using raycasts and layermasks we can do basic collisions for platforms relatively quickly //Working
        RaycastHit2D rayHit = Physics2D.Raycast(bottomRaycastOrigin, Vector2.down, bottomRaycastLength, platformLayerMask.value);
        if (rayHit.collider != null)
        {
            //Show the object raycast is returning
            Debug.DrawLine(bottomRaycastOrigin, rayHit.collider.transform.position, Color.red);
            if (Input.GetButtonDown("Jump"))
            {
                //Chaos CONTROOOLS!!! //Working
                rig.AddForce(new Vector2(0f, jumpForce));
                secondJump = true;
            }

            //Stop the object from falling through the platform //Working so long as collision is detected
            if (!tryingToFall && rig.velocity.y <= 0f)
            {
                //Compensate for force built up in air
                rig.velocity = new Vector2(rig.velocity.x, 0f);
            }
        }
        else
        {
            //Jump in midair //Working
            if (Input.GetButtonDown("Jump") && secondJump)
            {
                rig.velocity = new Vector2(rig.velocity.x, 0f);
                rig.AddForce(new Vector2(0f, jumpForce));
                secondJump = false;
            }
        }

        //Chaos CONTROOOLS!!! //Working
        if (Input.GetAxis("Horizontal") > 0)
            rig.AddForce(new Vector2(movementForce, 0f));
        else if (Input.GetAxis("Horizontal") < 0)
            rig.AddForce(new Vector2(-movementForce, 0f));

        if (Input.GetAxis("Vertical") < 0)
            tryingToFall = true;
        else
            tryingToFall = false;
    }

    //#JelloPunchesBack
    //Keep forces in check with equal and opposite
    void ClampSpeeds()
    {

        Vector2 currentTotalForce = new Vector2((rig.mass * ((lastVelocity.x - rig.velocity.x) / Time.fixedDeltaTime)), (rig.mass * ((lastVelocity.y - rig.velocity.y) / Time.fixedDeltaTime)));
        Vector2 addForceVector = new Vector2(0, 0);

        if (Mathf.Abs(currentTotalForce.x) > maxClampForce.x)
        {
            if (currentTotalForce.x > 0f)
                addForceVector.x -= (currentTotalForce.x - maxClampForce.x);
            else
                addForceVector.x -= (currentTotalForce.x + maxClampForce.x);
        }

        if (Mathf.Abs(currentTotalForce.y) > maxClampForce.y)
        {
            if (currentTotalForce.y > 0f)
                addForceVector.y -= (currentTotalForce.y - maxClampForce.y);
            else
                addForceVector.y -= (currentTotalForce.y + maxClampForce.y);
        }

        if (Mathf.Abs(currentTotalForce.x) < minClampForce.x)
            rig.velocity = new Vector2(0f, rig.velocity.y);

        rig.AddForce(addForceVector);
    }
}
