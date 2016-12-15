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

    GameObject top, bottom, right, left;

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
    private Combo2   currentEnemy;

    //Sound Files
    private AudioClip[] dash;
    private AudioClip[] jump;
    private AudioClip fall;
    private AudioClip attackX;
    private AudioClip attackY;
    private AudioClip attackA;
    private AudioClip attackB;
    private AudioClip error;
    private AudioClip clash;
    private int beatCycleNum;
    private bool playedError;

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
    private GameObject[] nodes;
    private GameObject finish;
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

    [SerializeField]
    GameObject fightSprite, LeftFist, RightFist;
    //Called before all start functions
    void Awake()
    {
        beatCycleNum = 0;
        dash = new AudioClip[] { Resources.Load("Sounds/Synth Slide D3") as AudioClip,
            Resources.Load("Sounds/Synth Slide E3") as AudioClip,
            Resources.Load("Sounds/Synth Slide F3") as AudioClip,
            Resources.Load("Sounds/Synth Slide G3") as AudioClip};

        jump = new AudioClip[] { Resources.Load("Sounds/Synth Slide A3") as AudioClip,
            Resources.Load("Sounds/Synth Slide B3") as AudioClip,
            Resources.Load("Sounds/Synth Slide A3") as AudioClip,
            Resources.Load("Sounds/Synth Slide B3") as AudioClip};

        fall = Resources.Load("Sounds/Synth Slide C3") as AudioClip;
        
        attackX = Resources.Load("Sounds/Synth Trumpet C3") as AudioClip;
        attackY = Resources.Load("Sounds/Synth Trumpet D3") as AudioClip;
        attackA = Resources.Load("Sounds/Synth Trumpet E3") as AudioClip;
        attackB = Resources.Load("Sounds/Synth Trumpet F3") as AudioClip;

        error = Resources.Load("Sounds/Attack D4") as AudioClip;
        //clash;
    }

    // Use this for initialization
    void Start () {
        //Layermask for platforms, used for raycasting
        platformLayerMask = LayerMask.GetMask("Platform");
        enemyLayerMask = LayerMask.GetMask("Enemy");
        nodes = GameObject.FindGameObjectsWithTag("Nodes");
        colliderBox = GetComponent<BoxCollider2D>();
        box = new Rect(colliderBox.bounds.min.x, colliderBox.bounds.min.y, colliderBox.bounds.size.x, colliderBox.bounds.size.y);
        lerpDestination = transform.position;
        attackInput = attackInputs.None;
        BeatMan.startBeat += Reset;
        BeatMan.endBeat += sendNoInput;
        checks = GameObject.FindGameObjectsWithTag("Checkpoint");
        spikes = GameObject.FindGameObjectsWithTag("Spikes");
        finish = GameObject.Find("Finish");
        startPos = transform.position;
        checkpointPos = startPos;
        mashingMove = 0;
        foreach (Transform child in transform)
        {
            switch (child.name)
            {
                case "Top Node":
                    top = child.gameObject;
                    break;
                case "Bottom Node":
					bottom = child.gameObject;
                    break;
                case "Right Node":
					right = child.gameObject;
                    break;
                case "Left Node":
                    left = child.gameObject;
                    break;
            }
        }
    }

    private void DrawHelperAtCenter(
                   Vector3 direction, Color color, float scale)
    {
        Gizmos.color = color;
        Vector3 destination = transform.position + direction * scale;
        Gizmos.DrawLine(transform.position, destination);
    }
    void OnDrawGizmos()
    {
        Color color = Color.green;
        // local forward
        DrawHelperAtCenter(this.transform.forward, color, 2f);
    }
    void FixedUpdate()
    {

        //Raycasts
        rayUp = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.up, moveCastLength, platformLayerMask.value);
        //Debug.DrawRay(new Vector2(transform.position.x, transform.position.y), Vector2.up * moveCastLength, Color.blue);
        rayRight = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.right, moveCastLength, platformLayerMask.value);
        //Debug.DrawRay(new Vector2(transform.position.x, transform.position.y), Vector2.right * moveCastLength, Color.blue);
        rayDown = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.down, moveCastLength, platformLayerMask.value);
        //Debug.DrawRay(new Vector2(transform.position.x, transform.position.y), Vector2.down * moveCastLength, Color.blue);
        rayLeft = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.left, moveCastLength, platformLayerMask.value);
        //Debug.DrawRay(new Vector2(transform.position.x, transform.position.y), Vector2.left * moveCastLength, Color.blue);

        //Raycast to enemy in front
        if (!flipped)
        {
            enemyRayHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.right, enemyRaycastLength, enemyLayerMask.value);
            Debug.DrawRay(new Vector2(transform.position.x, transform.position.y), Vector2.right * enemyRaycastLength, Color.red);
        }
        else
        {
            enemyRayHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.left, enemyRaycastLength, enemyLayerMask.value);
            Debug.DrawRay(new Vector2(transform.position.x, transform.position.y), Vector2.left * enemyRaycastLength, Color.red);
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
        if (transform.position != new Vector3(lerpDestination.x, lerpDestination.y, transform.position.z))
        {
            transform.position = Vector2.Lerp(transform.position, lerpDestination, lerp);
        }
    }

    // Update is called once per frame
    void Update () {
        //Make sure joystick has returned to neutral state from last intput
        if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
        {
            joyStickInput = true;
            playedError = false;
        }

        
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
                        AudioMan.instance.AddClipToLiveQueue(jump[beatCycleNum]);
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
                            AudioMan.instance.AddClipToLiveQueue(jump[beatCycleNum]);
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
                        AudioMan.instance.AddClipToLiveQueue(fall);
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

                        AudioMan.instance.AddClipToLiveQueue(dash[beatCycleNum]);
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

                        AudioMan.instance.AddClipToLiveQueue(dash[beatCycleNum]);
                    }
                }
            }    
        }
        else
        {
            //If mashing move shake and don't move next beat
            if (joyStickInput && (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0))
            {
                shake = 30f;
                if (BeatMan.instance.onTime)
                    mashingMove = 3;
                else
                    mashingMove = 2;

                if (!playedError)
                {
                    AudioMan.instance.AddClipToLiveQueue(error);
                    playedError = true;
                }
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
        if (finish != null)
        {
            if (finish.GetComponent<BoxCollider2D>().IsTouching(this.GetComponent<BoxCollider2D>()))
            {
                    finishLevel();
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
        if (enemyRayHit.collider != null)
        {
            Debug.Log("I see an enemy");
                currentEnemy = enemyRayHit.transform.gameObject.GetComponent<Combo2>();
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
		//Hits hit;
		//See if there is combat input
		//If multiple inputs, Garbage Input
		AudioClip playAudio = null;
			
		if (UnityEngine.Input.GetButtonDown ("XButton")) {
			currentEnemy.GetComponent<Combo2> ().HitHigh (Hits.high);
			AudioMan.instance.AddClipToLiveQueue(attackX);
			haveAttacked = true;
			return;
		}
		if (UnityEngine.Input.GetButtonDown ("YButton")) {
			currentEnemy.GetComponent<Combo2> ().HitMed (Hits.med);
			AudioMan.instance.AddClipToLiveQueue(attackY);
			haveAttacked = true;
			return;
		}
		if (UnityEngine.Input.GetButtonDown ("BButton")) {
			currentEnemy.GetComponent<Combo2> ().HitLow (Hits.low);
			AudioMan.instance.AddClipToLiveQueue(attackB);
			haveAttacked = true;
			return;
		}
		//If Offbeat or button mash, Garbage Input
		if (!BeatMan.instance.onTime) {
			AudioMan.instance.AddClipToLiveQueue(error);
		}

	}
    public void GenerateRight(float updwn)
    {
        GameObject temp = (GameObject)Instantiate(fightSprite);
        Vector3 tempP = new Vector3(RightFist.transform.position.x, RightFist.transform.position.y + updwn,RightFist.transform.position.z);//)
        temp.transform.position = tempP;
        Destroy(temp, 0.5f);
    }
    public void GenerateLeft()
    {
        GameObject temp = (GameObject)Instantiate(fightSprite);

        temp.transform.position = LeftFist.transform.position;
        Destroy(temp, 0.5f);
    }
    void GenerateFightSprite()
    {
        GameObject temp = (GameObject)Instantiate(fightSprite);
        
        temp.transform.position = transform.position ;
        Destroy(temp, 0.5f);
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
            //if (!currentEnemy.CheckInput(attackInput))
            //{
            //   // Clash();
            //    currentEnemy = null;
            //}
        }
        haveAttacked = true;

        //Null Move
        //if not grounded and you haven't moved or mashing move
        if(!grounded && !haveMoved)
        {
            lerpDestination = new Vector2(transform.position.x, transform.position.y - lerpDistance);
            AudioMan.instance.AddClipToLiveQueue(fall);
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

        if (beatCycleNum <= 2)
            beatCycleNum++;
        else
            beatCycleNum = 0;
    }

    /// <summary>
    /// Respawn on death
    /// </summary>
    void ResettoCheck()
    {
        transform.position = new Vector3(checkpointPos.x, checkpointPos.y, 0f);
        lerpDestination = new Vector2(checkpointPos.x, checkpointPos.y);
    }

    /// <summary>
    /// go to the next level
    /// </summary>
    void finishLevel()
    {
        Application.LoadLevel("Menu");
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
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }


    void nodeMovement()
    {
        Collider[] topObjects;
        Collider[] bottomObjects;
        Collider[] rightObjects;
        Collider[] leftObjects;
        bool jump = true;
        bool fall = false;
        bool follow = false; 
        bool moveLeft = true;
        bool moveRight = true;
        GameObject movementNodeLeft;
        GameObject movementNodeRight;
        GameObject movementNodeTop;
        GameObject movementNodeBottom;

        if (top != null)
        {
            topObjects = Physics.OverlapSphere(top.transform.position, 1);
            foreach (Collider collider in topObjects)
            {
                switch (collider.tag)
                {
                    case "Platform":
                    case "Platform2":
                    case "fallThroughPlatforms":
                    case "Moving Vertical":
                    case "Moving Horizantal":
                        //Prevent Jumping if a platform is above it
                        jump = false;
                        movementNodeTop = null;
                        break;
                    case "Nodes":
                        movementNodeTop = collider.gameObject;
                        break;
                }
            }
        }
        if (bottom != null)
        { 
             bottomObjects = Physics.OverlapSphere(bottom.transform.position, 1);
            foreach (Collider collider in bottomObjects)
            {
                switch (collider.tag)
                {
                    case "fallThroughPlatforms":
                        //Set Up Falling if there is nothing below. 
                        
                        follow = false;
                        fall = true;
                        movementNodeBottom = null;
                        break;

                    case "Moving Vertical":
                    case "Moving Horizantal":
                        //Move the player with the object
                        follow = true;
                        fall = false;
                        movementNodeBottom = null;
                        break;

                    case "Platform":
                    case "Platform2":
                        //Prevent Player From falling through platforms
                        follow = false;
                        fall = false;
                        movementNodeBottom = null;
                        break;
                    case "Nodes":
                        movementNodeBottom = collider.gameObject;
                        break;
                }

            }
        }
        if (right != null)
        {
             rightObjects = Physics.OverlapSphere(right.transform.position, 1);
            foreach (Collider collider in rightObjects)
            {
                switch (collider.tag)
                {

                    case "Platform":
                    case "Platform2":
                    case "fallThroughPlatforms":
                    case "Moving Vertical":
                    case "Moving Horizantal":
                    case "Enemy":
                        // Prevent moving into the block
                        moveRight = false;
                        movementNodeRight = null;
                        break;
                    case "Nodes":
                        movementNodeRight = collider.gameObject;
                        break;
                }
            }
        }
        if (left != null)
        {
            leftObjects = Physics.OverlapSphere(left.transform.position, 1);
            foreach (Collider collider in leftObjects)
            {
                switch (collider.tag)
                {
                    case "Platform":
                    case "Platform2":
                    case "fallThroughPlatforms":
                    case "Moving Vertical":
                    case "Moving Horizantal":
                    case "Enemy":
                        moveLeft = false;
                        movementNodeLeft = null;
                        break;
                    case "Nodes":
                        movementNodeLeft = collider.gameObject;
                        break;
                }
            }
        }

        //Move the player
        //Lerp to the node returned from the collider checks if the coresponding variable below is not null.
        //movementNodeLeft, movementNodeRight, movementNodeTop, movementNodeBottom
        if (jump)
        {
            // Allows the player to jump
        }
        if (fall)
        {
            //Allow the player to fall if in the air/ on a falling platform
            //Position to lerp to if falling through platform  > collider.transform.GetChild(1).transform.position;
        }
        if (follow)
        {
            //Move the player along with moving platforms
        }
        if (moveLeft)
        {
            //Move if there isnt anything blocking the path 
        }
        if (moveRight)
        {
            //Move if there isnt anything blocking the path 
        }

    }
}
