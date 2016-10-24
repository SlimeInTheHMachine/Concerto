using UnityEngine;
using System.Collections;

public enum attackInputs { A, B, X, Y, None, Garbage, Down, Right, Left, Up }; // Arrows

public class Platformer : MonoBehaviour {

    //Public Variables
    public bool aerialMove;
    public bool tryingToFall;
    public float enemyRaycastLength, moveCastLength;
    public float lerpDistance;
    public float lerpTime;
    public int score;

    //Private Variables
    private LayerMask platformLayerMask, enemyLayerMask;
    private BoxCollider2D colliderBox;
    private Rect box;
    private RaycastHit2D rayUp;
    private RaycastHit2D rayRight;
    private RaycastHit2D rayDown;
    private RaycastHit2D rayLeft;
    private RaycastHit2D enemyRayHit;
    private float shake;

    private attackInputs attackInput;
    private ComboScript currentEnemy;

    private bool haveAttacked;
    private bool haveMoved;
    private bool grounded;
    private bool canFall;
    private bool joyStickInput;
    private int mashingMove;
    private bool flipped;
    public Vector2 lerpDestination;
    private Vector2 startPos;
    private Vector2 checkpointPos;

    private GameObject[] checks;
    private GameObject[] spikes;
    public Vector2 LerpDestination
    {
        get { return lerpDestination; }
        set { lerpDestination = value; }
    }
    public bool HaveMoved
    {
        get { return haveMoved; }
        set { haveMoved = value; }
    }
    public bool Grounded
    {
        get { return grounded; }
        set { grounded = value; }
    }
    public bool CanFall
    {
        get { return canFall; }
        set { canFall = value; }
    }

    //Called before all start functions
    void Awake()
    {

    }

    // Use this for initialization
    void Start () {
        //Layermask for platforms, used for raycasting
        platformLayerMask = LayerMask.GetMask("Platform");
        enemyLayerMask = LayerMask.GetMask("Enemy");
        colliderBox = GetComponent<BoxCollider2D>();
        box = new Rect(colliderBox.bounds.min.x, colliderBox.bounds.min.y, colliderBox.bounds.size.x, colliderBox.bounds.size.y);
        lerpDestination = transform.position;
        attackInput = attackInputs.None;
        BeatMan.startBeat += Reset;
        BeatMan.endBeat += sendNoInput;
        checks = GameObject.FindGameObjectsWithTag("Checkpoint");
        spikes = GameObject.FindGameObjectsWithTag("Spikes");
        startPos = transform.position;
        checkpointPos = startPos;
        mashingMove = 0;
    }

    void FixedUpdate()
    {

        //Raycasts
        rayUp = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.up, moveCastLength, platformLayerMask.value);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y), Vector2.up * moveCastLength, Color.blue);
        rayRight = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.right, moveCastLength, platformLayerMask.value);
        rayDown = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.down, moveCastLength, platformLayerMask.value);
        rayLeft = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.left, moveCastLength, platformLayerMask.value);

        //Raycast to enemy in front
        if (!flipped)
        {
            enemyRayHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.right, enemyRaycastLength, enemyLayerMask.value);
            Debug.DrawRay(new Vector2(transform.position.x, transform.position.y), Vector2.right * enemyRaycastLength, Color.blue);
        }
        else
        {
            enemyRayHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.left, enemyRaycastLength, enemyLayerMask.value);
            Debug.DrawRay(new Vector2(transform.position.x, transform.position.y), Vector2.left * enemyRaycastLength, Color.blue);
        }

            //Physics related stuff
            //Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + box.size.y / 2), Vector2.down * moveCastLength, Color.red);
            if (rayDown.collider != null)
        {
            grounded = true;
            aerialMove = true;
            if (rayDown.collider.tag == "FallthroughPlatform")
                canFall = true;
            else
                canFall = false;
        }
        else
        {
            grounded = false;
            canFall = true;
        }

        //Update next location to move to //Supersmooth lerp t = (lerpTime * Time.fixedDeltaTime),  (lerp = t*t*t * (t * (6f*t - 15f) + 10f))
        float lerp = ((lerpTime * Time.fixedDeltaTime) * (lerpTime * Time.fixedDeltaTime) * (lerpTime * Time.fixedDeltaTime)) * ((lerpTime * Time.fixedDeltaTime) * ((6f * (lerpTime * Time.fixedDeltaTime)) - 15f) + 10f);
        //move if we're not there
        if(transform.position != new Vector3(lerpDestination.x, lerpDestination.y, transform.position.z))
        {
            transform.position = Vector2.Lerp(lerpDestination, transform.position, lerp);
        }
    }

    // Update is called once per frame
    void Update () {
        //Make sure joystick has returned to neutral state from last intput
        if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
            joyStickInput = true;

        
        if (BeatMan.instance.onTime && !haveMoved) //Won't work off beat, but won't kill a combo offbeat either currently
        {
            if(joyStickInput && mashingMove == 0) //Only move once per beat.
            {
                //Jump
                if (Input.GetAxis("Vertical") > 0 && rayUp.collider == null)
                {
                    if(grounded)
                    {
                        lerpDestination = new Vector2(transform.position.x, transform.position.y + lerpDistance);
                        haveMoved = true;
                        joyStickInput = false;
                    }
                    else
                    {
                        //Double Jump
                        if (aerialMove)
                        {
                            lerpDestination = new Vector2(transform.position.x, transform.position.y + lerpDistance);
                            haveMoved = true;
                            aerialMove = false;
                            joyStickInput = false;
                        }
                    }
                }
                //Fall
                else if (Input.GetAxis("Vertical") < 0)
                {   
                    if(canFall)
                    {
                        lerpDestination = new Vector2(transform.position.x, transform.position.y - lerpDistance);
                        haveMoved = true;
                        joyStickInput = false;
                    }
                }
                //Right
                else if (Input.GetAxis("Horizontal") > 0 && rayRight.collider == null)
                {
                    //Don't need to have a seperate aerialCheck because it will be set back to true on the ground
                    if (aerialMove)
                    {
                        lerpDestination = new Vector2(transform.position.x + lerpDistance, transform.position.y);
                        haveMoved = true;
                        joyStickInput = false;
                        aerialMove = false;

                        if (flipped)
                            Flip();
                    }
                }
                //Left
                else if (Input.GetAxis("Horizontal") < 0 && rayLeft.collider == null)
                { 
                    if (aerialMove)
                    {
                        lerpDestination = new Vector2(transform.position.x - lerpDistance, transform.position.y);
                        haveMoved = true;
                        joyStickInput = false;
                        aerialMove = false;

                        if (!flipped)
                            Flip();
                    }
                }
            }    
        }
        else
        {
            //If mashing move shake and don't move next beat
            if (joyStickInput && (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0))
            {
               // shake = 30f;
                //if (BeatMan.instance.onTime)
                  //  mashingMove = 3;
                //else
                    //mashingMove = 2;
            }
        }

        //Shake
        if (shake > 0f)
        {
            Vector2 shakeOffset = UnityEngine.Random.insideUnitCircle * 0.05f;
            transform.position = transform.position + new Vector3(shakeOffset.x, shakeOffset.y, 0f);
        }


        //Set Checkpoints 
        setCheckpoint();
        if (GameObject.Find("Finish") != null)
        {
            if (GameObject.Find("Finish").GetComponent<BoxCollider2D>().IsTouching(this.GetComponent<BoxCollider2D>()))
            {
                //Need end condition
            }
        }
        if (spikes != null)
        {
            for (int i = 0; i < spikes.Length; i++)
            {
                if (spikes[i].GetComponent<BoxCollider2D>().IsTouching(this.GetComponent<BoxCollider2D>()))
                {
                    ResettoCheck();
                }
            }
        }

        //Attack
        if (enemyRayHit.collider != null && (Input.GetButtonDown("AButton") || Input.GetButtonDown("BButton") || Input.GetButtonDown("XButton") || Input.GetButtonDown("YButton")))
        {
                currentEnemy = enemyRayHit.transform.gameObject.GetComponent<ComboScript>();
                CombatInput();
        }
    }

    /// <summary>
    /// Set the checkpoint to respawn at
    /// </summary>
    void setCheckpoint()
    {
        if (checks != null)
        {
            for (int i = 0; i < checks.Length; i++)
            {
                if (checks[i].GetComponent<BoxCollider2D>().IsTouching(this.GetComponent<BoxCollider2D>()))
                {
                    checkpointPos = new Vector2(checks[i].transform.position.x, checks[i].transform.position.y);
                    checks[i].transform.position = new Vector2(checks[i].transform.position.x, 1000f);
                }
            }
        }
    }

    /// <summary>
    /// Parse attack inputs and send it to the enemy
    /// </summary>
    void CombatInput()
    {
        //See if there is combat input
        //If multiple inputs, Garbage Input
        if (Input.GetButtonDown("AButton"))
        {
            if (attackInput == attackInputs.None)
                attackInput = attackInputs.A;
            else
                attackInput = attackInputs.Garbage;
        }
        if (Input.GetButtonDown("BButton"))
        {
            if (attackInput == attackInputs.None)
                attackInput = attackInputs.B;
            else
                attackInput = attackInputs.Garbage;
        }
        if (Input.GetButtonDown("XButton"))
        {
            if (attackInput == attackInputs.None)
                attackInput = attackInputs.X;
            else
                attackInput = attackInputs.Garbage;
        }
        if (Input.GetButtonDown("YButton"))
        {
            if (attackInput == attackInputs.None)
                attackInput = attackInputs.Y;
            else
                attackInput = attackInputs.Garbage;
        }

        //If Offbeat or button mash, Garbage Input
        if (!BeatMan.instance.onTime || haveAttacked)
            attackInput = attackInputs.Garbage;

        //Clash back if you mess up
        if(!currentEnemy.CheckInput(attackInput))
        {
            //clash code here
            Clash();
            currentEnemy = null;
        }
        //We've attempted to attack this beat
        haveAttacked = true;    
    }

    /// <summary>
    /// Bounce away from enemy
    /// </summary>
    void Clash()
    {
        if (flipped)
            lerpDestination = new Vector2(transform.position.x + lerpDistance, transform.position.y);
        else
            lerpDestination = new Vector2(transform.position.x - lerpDistance, transform.position.y);
    }

    /// <summary>
    /// In the event of no input, clash away from enemy and break combo or fall if in the air
    /// </summary>
    void sendNoInput()
    {
        //Null Attack
        if (currentEnemy != null && !haveAttacked)
        {
            attackInput = attackInputs.None;
            if (!currentEnemy.CheckInput(attackInput))
            {
                Clash();
                currentEnemy = null;
            }
        }
        haveAttacked = true;

        //Null Move
        //if not grounded and you haven't moved or mashing move
        if(!grounded && !haveMoved)
        {
            lerpDestination = new Vector2(transform.position.x, transform.position.y - lerpDistance);
        }

        if(mashingMove > 0)
            mashingMove--;
    }

    /// <summary>
    /// Reset controls at the end of the beat
    /// </summary>
    void Reset()
    {
        haveAttacked = false;
        attackInput = attackInputs.None;
        haveMoved = false;
        if (mashingMove == 0)
        {
            shake = 0f;
        }
        else
            mashingMove--;
    }

    /// <summary>
    /// Respawn on death
    /// </summary>
    void ResettoCheck()
    {
        transform.position = new Vector3(checkpointPos.x,checkpointPos.y,0f);
        lerpDestination = new Vector2(checkpointPos.x, checkpointPos.y);
    }

    /// <summary>
    /// Flip the character, right default
    /// </summary>
    void Flip()
    {
        //Change Flip Boolean
        if (flipped)
            flipped = false;
        else
            flipped = true;
        //Flip sprite
    }
}
