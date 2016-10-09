using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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
    private Vector2 lerpDestination;
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
            playerInRange = true;
            //I see no problem with this :^) -Rose
            playerObject = rayHit.collider.gameObject;
        }
        else
        {
            playerInRange = false;
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
    // Update is called once per frame
    public void EnemyUpdate ()
    {
        if (playerInRange)
            Attack();
        //Movement
        if (shake > 0)
        {
            Vector2 shakeOffset = UnityEngine.Random.insideUnitCircle * shakeAmount;
            transform.position = transform.position + new Vector3(shakeOffset.x, shakeOffset.y, 0f);
            shake -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shake = 0.0f;
            transform.position = OGPos;
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
    public bool checkInput(attackInputs input)
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
