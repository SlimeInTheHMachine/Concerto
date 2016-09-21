using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BlaneComboScript : MonoBehaviour {

    //Public Variables
    public bool Selected = false;
    public attackInputs button1, button2, button3, button4;
    
    //Private Variables
    private Queue<attackInputs> combo, currentCombo;
    //temp Shake code
    private float shake;
    private float shakeAmount = 0.1f;
    private float decreaseFactor = 0.5f;
    private Vector2 OGPos;

    // Use this for initialization
    void Start () {
        //GameControl = GameObject.FindGameObjectWithTag("GameController");
        //Puts in three standard buttons
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
            currentCombo = new Queue<attackInputs>(combo);
            return false;
        }
        else
        {
            if (input == currentCombo.Peek())
            {
                currentCombo.Dequeue();
                shake = 0.05f;
                OGPos = transform.position;
            }
            else
            {
                currentCombo = new Queue<attackInputs>(combo);
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
