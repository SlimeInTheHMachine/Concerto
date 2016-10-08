using UnityEngine;
using System.Collections;


public enum attackInputs { A, B, X, Y, None, Garbage, Down, Right, Left, Up }; // Arrows


public class AttackInputWrapper
{
    public bool Keyboard { get; set; } //uneccesary if I use my Own Axis
    public attackInputs thisInput = attackInputs.None;

    public string attackLetter;
    public AttackInputWrapper(attackInputs input)
    {
        thisInput = input;
        SetLetterArbitrarily();
    }

    void SetLetterArbitrarily()
    {
        switch (thisInput)
        {
            case attackInputs.A:
                attackLetter = "A";
                break;
            case attackInputs.B:
                attackLetter = "B";
                break;
            case attackInputs.X:
                attackLetter = "X";
                break;
            case attackInputs.Y:
                attackLetter = "Y";
                break;
            case attackInputs.None:
                attackLetter = "-";
                break;
            case attackInputs.Garbage:
                attackLetter = null;
                break;
            case attackInputs.Down:
                attackLetter = "S";
                break;
            case attackInputs.Left:
                attackLetter = "A";
                break;
            case attackInputs.Right:
                attackLetter = "D";
                break;
            case attackInputs.Up:
                attackLetter = "W";
                break;
        }
    }
}
public class Platformer : MonoBehaviour {

    //Public Variables
    public bool aerialMove;
    public bool tryingToFall = false;
    public float enemyRaycastLength, floorCastLength;
    public float lerpDistance;
    public float lerpTime;
    public int score;

    //Private Variables
    private LayerMask platformLayerMask, enemyLayerMask;
    private Rigidbody2D rig;
    private BoxCollider2D colliderBox;
    private Rect box;
    private attackInputs attackInput;
    private BlaneComboScript currentEnemy;

    private bool haveAttacked;
    private bool haveMoved;
    private bool grounded;
    private bool canFall;
    private bool joyStickInput;
    private Vector2 lerpDestination;
    private Vector2 startPos;
    public Vector2 checkpointPos;

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

    // Use this for initialization
    void Start () {
        //Layermask for platforms, used for raycasting
        platformLayerMask = LayerMask.GetMask("Platform");
        enemyLayerMask = LayerMask.GetMask("Enemy");
        rig = GetComponent<Rigidbody2D>();
        rig.gravityScale = 0;
        colliderBox = GetComponent<BoxCollider2D>();
        box = new Rect(colliderBox.bounds.min.x, colliderBox.bounds.min.y, colliderBox.bounds.size.x, colliderBox.bounds.size.y);
        lerpDestination = transform.position;
        attackInput = attackInputs.None;
        BlaneBeatMan.startBeat += Reset;
        BlaneBeatMan.endBeat += sendNoInput;
        checks = GameObject.FindGameObjectsWithTag("Checkpoint");
        spikes = GameObject.FindGameObjectsWithTag("Spikes");
        startPos = transform.position;
        checkpointPos = startPos;
    }

    void FixedUpdate()
    {
        //Physics related stuff
        RaycastHit2D rayHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + box.size.y / 2), Vector2.down, floorCastLength, platformLayerMask.value);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + box.size.y / 2), Vector2.down * floorCastLength, Color.red);
        if (rayHit.collider != null)
        {
            grounded = true;
            aerialMove = true;
        }
        else
        {
            grounded = false;
        }
        
        transform.position = Vector2.Lerp(transform.position, lerpDestination, lerpTime * Time.fixedDeltaTime);
        if(transform.position == new Vector3(lerpDestination.x, lerpDestination.y, 0f))
        {
            lerpDestination = transform.position;
        }
    }

    // Update is called once per frame
    void Update () {
        box = new Rect(colliderBox.bounds.min.x, colliderBox.bounds.min.y, colliderBox.bounds.size.x, colliderBox.bounds.size.y);

        if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
            joyStickInput = true;

        if (BlaneBeatMan.instance.onTime)
        {
            if(!haveMoved && joyStickInput)
            {
                if (Input.GetAxis("Vertical") > 0)
                {
                    if(grounded)
                    {
                        lerpDestination = new Vector2(transform.position.x, transform.position.y + lerpDistance);
                        haveMoved = true;
                        joyStickInput = false;
                    }
                    else
                    {
                        if (aerialMove)
                        {
                            lerpDestination = new Vector2(transform.position.x, transform.position.y + lerpDistance);
                            haveMoved = true;
                            aerialMove = false;
                            joyStickInput = false;
                        }
                    }
                }
                else if (Input.GetAxis("Vertical") < 0)
                {
                    RaycastHit2D ray = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + box.size.y / 2), Vector2.down, floorCastLength, platformLayerMask.value);
                    if ((ray.collider != null && ray.collider.tag == "FallthroughPlatform") || !grounded)
                        canFall = true;
                    else
                        canFall = false;
                    if(canFall)
                    {
                        lerpDestination = new Vector2(transform.position.x, transform.position.y - lerpDistance);
                        haveMoved = true;
                        joyStickInput = false;
                    }
                }
                else if (Input.GetAxis("Horizontal") > 0)
                {
                    if (aerialMove)
                    {
                        lerpDestination = new Vector2(transform.position.x + lerpDistance, transform.position.y);
                        haveMoved = true;
                        joyStickInput = false;
                        aerialMove = false;
                    }
                }
                else if (Input.GetAxis("Horizontal") < 0)
                {
                    if (aerialMove)
                    {
                        lerpDestination = new Vector2(transform.position.x - lerpDistance, transform.position.y);
                        haveMoved = true;
                        joyStickInput = false;
                        aerialMove = false;
                    }
                }
            }    
        }


        //Set Checkpoints 
        setCheckpoint();
        if (GameObject.Find("Finish") != null)
        {
            if (GameObject.Find("Finish").GetComponent<BoxCollider2D>().IsTouching(this.GetComponent<BoxCollider2D>()))
            {
                ResettoStart();
            }
        }
        for (int i = 0; i < spikes.Length; i++)
        {
            if (spikes[i].GetComponent<BoxCollider2D>().IsTouching(this.GetComponent<BoxCollider2D>()))
            {
                ResettoCheck();
            }
        }

        //Attack
        //Raycast to enemy
        RaycastHit2D rayHit = Physics2D.Raycast(new Vector2(transform.position.x + box.size.x/2, transform.position.y), Vector2.right, enemyRaycastLength, enemyLayerMask.value);
        if (rayHit.collider != null && (Input.GetButtonDown("AButton") || Input.GetButtonDown("BButton") || Input.GetButtonDown("XButton") || Input.GetButtonDown("YButton")))
        {
                currentEnemy = rayHit.transform.gameObject.GetComponent<BlaneComboScript>();
                CombatInput();
        }
    }
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
        if (!BlaneBeatMan.instance.onTime || haveAttacked)
            attackInput = attackInputs.Garbage;

        //Clash back if you mess up
        if(!currentEnemy.checkInput(attackInput))
        {
            //clash code here
            Clash();
            currentEnemy = null;
        }

        haveAttacked = true;    
    }

    void Clash()
    {
        //normal of this to Enemy
        if(Vector2.Dot(transform.position, currentEnemy.GetComponent<Rigidbody2D>().transform.position) < 0)
            lerpDestination = new Vector2(transform.position.x - lerpDistance, transform.position.y );
        else
            lerpDestination = new Vector2(transform.position.x + lerpDistance, transform.position.y);
    }

    //Not Firing in time
    void sendNoInput()
    {
        //Null Attack
        if (currentEnemy != null && !haveAttacked)
        {
            attackInput = attackInputs.None;
            if (!currentEnemy.checkInput(attackInput))
            {
                Clash();
                currentEnemy = null;
            }
        }
        haveAttacked = true;

        //Null Move
        if(!grounded && !haveMoved)
        {
            lerpDestination = new Vector2(transform.position.x, transform.position.y - lerpDistance);
        }
    }

    void Reset()
    {
        haveAttacked = false;
        attackInput = attackInputs.None;
        haveMoved = false;
    }
    void ResettoCheck()
    {
        transform.position = new Vector3(checkpointPos.x,checkpointPos.y,0f);
        lerpDestination = new Vector2(checkpointPos.x, checkpointPos.y);
    }

    void ResettoStart()
    {
        transform.position = new Vector3(startPos.x, startPos.y, 0f);
        lerpDestination = new Vector2(startPos.x, startPos.y);
    }
}
