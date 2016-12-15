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

public class ButtonLetter{
    public GameObject sprite;
    public attackInputs atkInput;


    public ButtonLetter(GameObject Sp, attackInputs input)
    {
        sprite = Sp;
        atkInput = input; 
    }
}

public class ComboScript : MonoBehaviour {

    //Public Variables
    public bool Selected = false;
    private bool flipped;
    public attackInputs button1, button2, button3, button4;
    [SerializeField]
    GameObject letter1, letter2, letter3, letter4;
    //Private Variables
    private Queue<ButtonLetter> combo, currentCombo;
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

    /// <summary>
    /// This is te number of letters that will appear above the enemys
    /// </summary>
    [SerializeField]
    int numOfLetters = 4;

    int moveCounter = 0; //2 beats of nothing
    bool lastMoveLeft = false;


    public int health = 3;
    // Use this for initialization
    void Start () {
       // flipped = true;
        attackCounter = 0;
        BoxCollider2D colliderBox = GetComponent<BoxCollider2D>();
        box = new Rect(colliderBox.bounds.min.x, colliderBox.bounds.min.y, colliderBox.bounds.size.x, colliderBox.bounds.size.y);
        enemyLayerMask = LayerMask.GetMask("Player");
        combo = new Queue<ButtonLetter>();
        foreach (Transform child in transform)
        {
            if(child.gameObject.name == "1")
            {
                ButtonLetter bLetter = new ButtonLetter(letter1, button1);
                combo.Enqueue(bLetter);
                Debug.Log("First");
                child.gameObject.GetComponent<SpriteRenderer>().sprite = letter1.GetComponent<SpriteRenderer>().sprite;
            }
            if (child.gameObject.name == "2")
            {
                ButtonLetter bLetter = new ButtonLetter(letter2, button2);
                combo.Enqueue(bLetter);
                Debug.Log("Secind");
                child.gameObject.GetComponent<SpriteRenderer>().sprite = letter2.GetComponent<SpriteRenderer>().sprite;
            }
            if (child.gameObject.name == "3")
            {
                ButtonLetter bLetter = new ButtonLetter(letter3, button3);
                combo.Enqueue(bLetter);
                Debug.Log("Third");
                child.gameObject.GetComponent<SpriteRenderer>().sprite = letter2.GetComponent<SpriteRenderer>().sprite;
            }
            if (child.gameObject.name == "4")
            {
                ButtonLetter bLetter = new ButtonLetter(letter4, button4);
                combo.Enqueue(bLetter);
                Debug.Log("Fourth");
                child.gameObject.GetComponent<SpriteRenderer>().sprite = letter2.GetComponent<SpriteRenderer>().sprite;
            }
        }
       
        currentCombo = new Queue<ButtonLetter>(combo);
        OGPos = transform.position;
        lerpDestination = this.transform.position;
    }

    void ResetQueue()
    {
        //currentCombo = new Queue<attackInputs>(combo);
        //if(lettersLeft == 1)
        //{
        //    letter3.GetComponent<MeshRenderer>().enabled = true;
        //    lettersLeft += 1;
        //}
        //if (lettersLeft == 2)
        //{
        //    letter2.GetComponent<MeshRenderer>().enabled = true;
        //    lettersLeft += 1;
        //}
        //if (lettersLeft == 3)
        //{
        //    letter1.GetComponent<MeshRenderer>().enabled = true;
        //    lettersLeft += 1;
        //}
        ////lettersLeft = 4;
        //if (lettersLeft != 4)
        //{
        //    lettersLeft = 4;
        //}
        
    }

    void FixedUpdate()
    {
        RaycastHit2D rayHit;
        if (flipped)
            rayHit = Physics2D.Raycast(new Vector2(transform.position.x + box.size.x / 2, transform.position.y - box.size.y / 4),
                Vector2.right, 1, enemyLayerMask.value);
        else
            rayHit = Physics2D.Raycast(new Vector2(transform.position.x - box.size.x / 2, transform.position.y - box.size.y / 4),
                Vector2.left, 1, enemyLayerMask.value);
        if(flipped)
            Debug.DrawRay(new Vector2(transform.position.x + box.size.x / 2, transform.position.y - box.size.y / 4), Vector2.right, Color.blue);
        else
            Debug.DrawRay(new Vector2(transform.position.x - box.size.x / 2, transform.position.y - box.size.y / 4), Vector2.left, Color.red);

        if (rayHit.collider != null)
        {
            //Debug.Log("Hitting Player");
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
    ///
    void Attack()
    {
        if (attackCounter == numOfLetters + 1)
        {
            //Debug.Log("Attacking");
            //EnemyClash();
            attackCounter = 0;
        }
        else
        {
            // Debug.Log("Getting Ready to Attack");
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

        }
        else
        {
            lerpDestination = new Vector2(transform.position.x - lerpDistance, transform.position.y);
            lastMoveLeft = true;

        }
        moveCounter = 0;
    }
    // Update is called once per frame
    public void EnemyUpdate ()
    {
        //Debug.Log("Update");
        if (playerInRange)
          Attack();
        else
        {
            //Movement
            if(moveCounter >= 2)
            {
                Move();
                //if (flipped)
                //    Flip();
            }
            else
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


    public void Attacked()
    {
        //Debug.Log("I'm attacked");
        //health -= 1;
        //if (PlatManager.instance.player.transform.position.x < transform.position.x)
        //{
        //    PlatManager.instance.player.GetComponent<Platformer>().GenerateRight();
        //}
        //else
        //{
        //    PlatManager.instance.player.GetComponent<Platformer>().GenerateLeft();
        //}
        //    if (health <= 0)
        //{
        //    Destroy(gameObject, 2f);
        //    //get tossed
        //    if(PlatManager.instance.player.transform.position.x < transform.position.x)
        //    {
        //       // PlatManager.instance.player.GetComponent<Platformer>().GenerateRight();
        //        gameObject.AddComponent<Rigidbody2D>();
        //        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 1) * 25, ForceMode2D.Impulse);
        //    }
        //    else
        //    {
        //       // PlatManager.instance.player.GetComponent<Platformer>().GenerateLeft();
        //        gameObject.AddComponent<Rigidbody2D>();
        //        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1, 1) * 25, ForceMode2D.Impulse);
        //    }
        //    Destroy(this);
        //}
    }
    /// <summary>
    /// Called by the enemy to initate a clash if the player misses enough beats
    /// </summary>
    public void EnemyClash()
    {
        if (!flipped)
        {
            playerObject.GetComponent<Platformer>().LerpDestination = new Vector2(playerObject.transform.position.x - 1f,
                playerObject.transform.position.y);
        }
        else
        {
            playerObject.GetComponent<Platformer>().LerpDestination = new Vector2(playerObject.transform.position.x + 1f,
                playerObject.transform.position.y);
        }
       // playerObject.GetComponent<Platformer>().LerpDestination= new Vector2(playerObject.transform.position.x + 1f, playerObject.transform.position.y);
        ////normal of this to Enemy
        //if (Vector2.Dot(transform.position, currentEnemy.GetComponent<Rigidbody2D>().transform.position) < 0)
        //lerpDestination = new Vector2(transform.position.x - 2.5f, transform.position.y);
        //else
        //    lerpDestination = new Vector2(transform.position.x + lerpDistance, transform.position.y);
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
    public bool CheckInput(attackInputs input)
    {
        Debug.Log(currentCombo.Peek().atkInput);
        //Reset on mess up
        if (input == attackInputs.Garbage)
        {
            ResetQueue();
            return false;
        }
        else
        {
            //if (input == currentCombo.Peek())
            //{
            //    currentCombo.Dequeue();
            //    shake = 0.05f;
            //    OGPos = transform.position;
            //    lettersLeft--;
            //    if (lettersLeft == 1)
            //    {
            //        letter3.GetComponent<MeshRenderer>().enabled = false;
            //    }
            //    if (lettersLeft == 2)
            //    {
            //        letter2.GetComponent<MeshRenderer>().enabled = false;
            //    }
            //    if (lettersLeft == 3)
            //    {
            //        letter1.GetComponent<MeshRenderer>().enabled = false;
            //    }
            //}
            //else
            //{
            //    ResetQueue();
            //    return false;
            //}
            //if (currentCombo.Count == 0)
            //{
            //    Destroy(gameObject);
            //    return true;
            //}
            return true;
        }
    }
}
