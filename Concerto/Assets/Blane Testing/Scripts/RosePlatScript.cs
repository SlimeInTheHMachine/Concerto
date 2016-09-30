using UnityEngine;
using System.Collections;

public class RosePlatScript : MonoBehaviour {

    //Public Variables
    public float bottomRaycastLength, enemyRaycastLength;
    public float jumpForce;
    public float movementForce;
    public float clashForce;
    public float frictionCoff;
    public float decelMultiVal;
    public Vector2 maxClampSpeed;
    public Vector2 minClampSpeed;
    public Vector2 raycastOffset;
    public bool secondJump;
    public bool tryingToFall = false;
    public int score;
    public RoseComboScript currentEnemy;

    //Private Variables
    private LayerMask platformLayerMask, enemyLayerMask;
    private Rigidbody2D rig;
    private BoxCollider2D colliderBox;
    private Rect box;
    private attackInputs attackInput;
    private Vector2 bottomRaycastOrigin;
    
    private bool haveAttacked;

    // Use this for initialization
    void Start () {
        //Layermask for platforms, used for raycasting
        platformLayerMask = LayerMask.GetMask("Platform");
        enemyLayerMask = LayerMask.GetMask("Enemy");
        rig = GetComponent<Rigidbody2D>();
        colliderBox = GetComponent<BoxCollider2D>();
        attackInput = attackInputs.None;
        BlaneBeatMan.startBeat += ResetAttack;
        BlaneBeatMan.endBeat += sendNoInput;
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
            if (Mathf.Abs(rig.velocity.x) > minClampSpeed.x)
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

    }

    //#JelloPunchesBack
    //Keep forces in check with equal and opposite
    void ClampSpeeds()
    {
        //check for speeds
        Vector2 currentSpeed = rig.velocity;
        Vector2 addForceVector = new Vector2(0, 0);

        if (Mathf.Abs(currentSpeed.x) > maxClampSpeed.x)
        {
            if (currentSpeed.x > 0f)
                addForceVector.x -= (currentSpeed.x - maxClampSpeed.x);
            else
                addForceVector.x -= (currentSpeed.x + maxClampSpeed.x);
        }

        if (Mathf.Abs(currentSpeed.y) > maxClampSpeed.y)
        {
            if (currentSpeed.y > 0f)
                addForceVector.y -= (currentSpeed.y - maxClampSpeed.y);
            else
                addForceVector.y -= (currentSpeed.y + maxClampSpeed.y);
        }

        //stop is small enough
        if (Mathf.Abs(currentSpeed.x) < minClampSpeed.x)
            rig.velocity = new Vector2(0f, rig.velocity.y);

        //find the opposite force
        Vector2 accelForce = new Vector2(rig.mass * (addForceVector.x / Time.fixedDeltaTime), rig.mass * (addForceVector.y / Time.fixedDeltaTime));

        rig.AddForce(accelForce);
    }

    // Update is called once per frame
    void Update () {

        box = new Rect(colliderBox.bounds.min.x, colliderBox.bounds.min.y, colliderBox.bounds.size.x, colliderBox.bounds.size.y);

        //Raycast to use platforms //Working
        bottomRaycastOrigin = new Vector2(transform.position.x, transform.position.y) - raycastOffset;

        //Using raycasts and layermasks we can do basic collisions for platforms relatively quickly //Working
        RaycastHit2D rayHit = Physics2D.Raycast(bottomRaycastOrigin, Vector2.down, bottomRaycastLength, platformLayerMask.value);
        if (rayHit.collider != null)
        {
            //Show the object raycast is returning
            Debug.DrawLine(bottomRaycastOrigin, rayHit.collider.transform.position, Color.red);
            if (Input.GetAxis("Vertical") > 0)
            {
                //Chaos CONTROOOLS!!!
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
            //Jump in midair
            if (Input.GetAxis("Vertical") > 0 && secondJump)
            {
                rig.velocity = new Vector2(rig.velocity.x, 0f);
                rig.AddForce(new Vector2(0f, jumpForce));
                secondJump = false;
            }
        }

        //Chaos CONTROOOLS!!!
        if (Input.GetAxis("Horizontal") > 0)
        {
            //Instant stop
           // if(rig.velocity.x < 0f)
           //     rig.velocity = new Vector2(0f, rig.velocity.y);
            rig.AddForce(new Vector2(movementForce, 0f));
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            //Instant stop
           // if (rig.velocity.x > 0f)
           //     rig.velocity = new Vector2(0f, rig.velocity.y);
            rig.AddForce(new Vector2(-movementForce, 0f));
        }

        if (Input.GetAxis("Vertical") < 0)
            tryingToFall = true;
        else
            tryingToFall = false;

        //if (Input.GetButtonDown("Submit"))
        //{
        //    Debug.Log("Fire1");
        //}
        //Attack
        //Raycast to enemy
        rayHit = Physics2D.Raycast(new Vector2(transform.position.x + box.size.x/2, transform.position.y), Vector2.right, enemyRaycastLength, enemyLayerMask.value);
        Debug.DrawRay(new Vector2(transform.position.x + box.size.x / 2, transform.position.y), Vector2.right * enemyRaycastLength, Color.blue);

        if (rayHit.collider != null && (Input.GetButtonDown("AButton") || Input.GetButtonDown("BButton") || Input.GetButtonDown("XButton")
            || Input.GetButtonDown("YButton")))
        {
            Debug.Log("We hit a thing that was important");
            currentEnemy = rayHit.transform.gameObject.GetComponent<RoseComboScript>();
            Debug.DrawRay(new Vector2(transform.position.x + box.size.x / 2, transform.position.y), Vector2.right * enemyRaycastLength, Color.yellow);
            CombatInput();
        }
    }

    void CombatInput()
    {
        if (currentEnemy != null)
        {
            //See if there is combat input
            //If multiple inputs, Garbage Input
            if (Input.GetButtonDown("YButton"))
            {
                if (attackInput == attackInputs.None)
                    attackInput = attackInputs.Up;
                else
                    attackInput = attackInputs.Garbage;
            }
            if (Input.GetButtonDown("XButton"))
            {
                if (attackInput == attackInputs.None)
                    attackInput = attackInputs.Left;
                else
                    attackInput = attackInputs.Garbage;
            }
            if (Input.GetButtonDown("AButton"))
            {
                if (attackInput == attackInputs.None)
                    attackInput = attackInputs.Down;
                else
                    attackInput = attackInputs.Garbage;
            }
            if (Input.GetButtonDown("BButton"))
            {
                if (attackInput == attackInputs.None)
                    attackInput = attackInputs.Right;
                else
                    attackInput = attackInputs.Garbage;
            }

            //If Offbeat or button mash, Garbage Input
            if (!BlaneBeatMan.instance.onTime || haveAttacked)
                attackInput = attackInputs.Garbage;

            //Clash back if you mess up
            if(!currentEnemy.checkInput(attackInput))
            {
                //clash code here
                rig.AddForce(new Vector2(clashForce, 0f));
                currentEnemy = null;
            }

            haveAttacked = true;
        }
       
    }

    //Not Firing in time
    void sendNoInput()
    {
        if (currentEnemy != null && !haveAttacked)
        {
            attackInput = attackInputs.None;
            if (!currentEnemy.checkInput(attackInput))
            {
                //clash code here
                rig.AddForce(new Vector2(clashForce, 0f));
                currentEnemy = null;
            }
        }
        haveAttacked = true;
    }

    void ResetAttack()
    {
        haveAttacked = false;
        attackInput = attackInputs.None;
    }
}
