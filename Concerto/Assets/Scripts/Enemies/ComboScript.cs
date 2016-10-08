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

    // Use this for initialization
    void Start () {
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
    // Update is called once per frame
    void Update ()
    {
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
