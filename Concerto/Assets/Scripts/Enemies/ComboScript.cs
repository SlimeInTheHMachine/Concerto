using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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

public class ComboScript : MonoBehaviour {

    //Public Variables
    public bool Selected = false;
    public attackInputs button1, button2, button3, button4;
    [SerializeField]
    GameObject letter1, letter2, letter3, letter4;
    //Private Variables
    private Queue<attackInputs> combo, currentCombo;
    int lettersLeft = 4;
    //temp Shake code
    private float shake;
    private float shakeAmount = 0.1f;
    private float decreaseFactor = 0.5f;
    private Vector2 OGPos;
    private LayerMask enemyLayerMask;
    private Rect box;
    private bool playerInRange = false;
    private int attackCounter;
    private GameObject playerObject;
    public float lerpDistance;
    private Vector2 lerpDestination;
    public float lerpTime;

    int moveCounter = 0; //2 beats of nothing
    bool lastMoveLeft = false;
    // Use this for initialization
    void Start () {
        attackCounter = 0;
        BoxCollider2D colliderBox = GetComponent<BoxCollider2D>();
        box = new Rect(colliderBox.bounds.min.x, colliderBox.bounds.min.y, colliderBox.bounds.size.x, colliderBox.bounds.size.y);
        enemyLayerMask = LayerMask.GetMask("Player");
        //GameControl = GameObject.FindGameObjectWithTag("GameController");
        //Puts in three standard buttons
        AttackInputWrapper btnHolder1 = new AttackInputWrapper(button1);
        letter1.GetComponent<TextMesh>().text = btnHolder1.attackLetter;
        AttackInputWrapper btnHolder2 = new AttackInputWrapper(button2);
        letter2.GetComponent<TextMesh>().text = btnHolder2.attackLetter;
        AttackInputWrapper btnHolder3 = new AttackInputWrapper(button3);
        letter3.GetComponent<TextMesh>().text = btnHolder3.attackLetter;
        AttackInputWrapper btnHolder4 = new AttackInputWrapper(button4);
        letter4.GetComponent<TextMesh>().text = btnHolder4.attackLetter;
        combo = new Queue<attackInputs>();
        combo.Enqueue(button1);
        combo.Enqueue(button2);
        combo.Enqueue(button3);
        combo.Enqueue(button4);
        //Deep Clone
        currentCombo = new Queue<attackInputs>(combo);
        OGPos = transform.position;
        //Per enemy? //NOOOOOOOOOOOOO //Enemy Manager
        //BeatManager.onBeat += InputCheck;
        lerpDestination = this.transform.position;
    }

    void ResetQueue()
    {
        currentCombo = new Queue<attackInputs>(combo);
        if(lettersLeft == 1)
        {
            letter3.GetComponent<MeshRenderer>().enabled = true;
            lettersLeft += 1;
        }
        if (lettersLeft == 2)
        {
            letter2.GetComponent<MeshRenderer>().enabled = true;
            lettersLeft += 1;
        }
        if (lettersLeft == 3)
        {
            letter1.GetComponent<MeshRenderer>().enabled = true;
            lettersLeft += 1;
        }
        //lettersLeft = 4;
        if (lettersLeft != 4)
        {
            lettersLeft = 4;
        }
        
    }

    void FixedUpdate()
    {
        
        RaycastHit2D rayHit = Physics2D.Raycast(new Vector2(transform.position.x - box.size.x / 2, transform.position.y - box.size.y / 4), -Vector2.right, 3, enemyLayerMask.value);
        Debug.DrawRay(new Vector2(transform.position.x - box.size.x / 2, transform.position.y - box.size.y / 4), -Vector2.right, Color.red);
        if (rayHit.collider != null)
        {
            Debug.Log("Hitting Player");
            playerInRange = true;
            //I see no problem with this :^) -Rose
            playerObject = rayHit.collider.gameObject;
        }
        else
        {
            playerInRange = false;
        }


        transform.position = Vector2.Lerp(transform.position, lerpDestination, lerpTime * Time.fixedDeltaTime);
        if (transform.position == new Vector3(lerpDestination.x, lerpDestination.y, 0f))
        {
            lerpDestination = transform.position;
        }
    }
    /// <summary>
    /// Has a number of windups equal to the max number of spots + 1
    /// </summary>
    void Attack()
    {
        if(attackCounter == 5)
        {
            Debug.Log("Attacking");
            EnemyClash();
            attackCounter = 0;
        }
        else
        {
            Debug.Log("Getting Ready to Attack");
            ++attackCounter;
        }
    }

    /// <summary>
    /// Moves the enemy back and forth 1 square for now
    /// Proof of concept?
    /// </summary>
    void Move()
    {
        if (lastMoveLeft) //has to move right
        {
            lerpDestination = new Vector2(transform.position.x + lerpDistance, transform.position.y);
            lastMoveLeft = false;
        }else
        {
            lerpDestination = new Vector2(transform.position.x - lerpDistance, transform.position.y);
            lastMoveLeft = true;
        }
        moveCounter = 0;
    }
    // Update is called once per frame
    public void EnemyUpdate ()
    {
        Debug.Log("Update");
        if (playerInRange)
            Attack();
        else
        {
            //Movement
            if(moveCounter >= 2)
            {
                Move();
            }else
            {
                moveCounter++;
            }
        }

        if (shake > 0)
        {
            Vector2 shakeOffset = UnityEngine.Random.insideUnitCircle * shakeAmount;
            transform.position = transform.position + new Vector3(shakeOffset.x, shakeOffset.y, 0f);
            shake -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shake = 0.0f;
           //-* transform.position = OGPos;
        }
    }

    /// <summary>
    /// Called by the enemy to initate a clash if the player misses enough beats
    /// </summary>
    public void EnemyClash()
    {
        playerObject.GetComponent<Platformer>().LerpDestination= new Vector2(playerObject.transform.position.x - 2.5f, playerObject.transform.position.y);
        ////normal of this to Enemy
        //if (Vector2.Dot(transform.position, currentEnemy.GetComponent<Rigidbody2D>().transform.position) < 0)
        //lerpDestination = new Vector2(transform.position.x - 2.5f, transform.position.y);
        //else
        //    lerpDestination = new Vector2(transform.position.x + lerpDistance, transform.position.y);
    }
    public bool CheckInput(attackInputs input)
    {
        //Reset on mess up
        if (input == attackInputs.Garbage)
        {
            ResetQueue();
            return false;
        }
        else
        {
            if (input == currentCombo.Peek())
            {
                currentCombo.Dequeue();
                shake = 0.05f;
                OGPos = transform.position;
                lettersLeft--;
                if (lettersLeft == 1)
                {
                    letter3.GetComponent<MeshRenderer>().enabled = false;
                }
                if (lettersLeft == 2)
                {
                    letter2.GetComponent<MeshRenderer>().enabled = false;
                }
                if (lettersLeft == 3)
                {
                    letter1.GetComponent<MeshRenderer>().enabled = false;
                }
            }
            else
            {
                ResetQueue();
                return false;
            }
            if (currentCombo.Count == 0)
            {
                Destroy(gameObject);
                return true;
            }
            return true;
        }
    }
}
