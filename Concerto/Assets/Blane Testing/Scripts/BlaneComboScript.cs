using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BlaneComboScript : MonoBehaviour {

    //Public Variables
    public bool Selected = false;
    public char button1, button2, button3;
    
    //Private Variables
    private Queue<char> combo, currentCombo;
    //temp Shake code
    private float shake;
    private float shakeAmount = 0.1f;
    private float decreaseFactor = 0.5f;
    private Vector2 OGPos;

    // Use this for initialization
    void Start () {
        //GameControl = GameObject.FindGameObjectWithTag("GameController");
        //Puts in three standard buttons
        combo = new Queue<char>();
        if (button1 != 0)
            combo.Enqueue(button1);
        if (button2 != 0)
            combo.Enqueue(button2);
        if (button3 != 0)
            combo.Enqueue(button3);
        //Deep Clone
        currentCombo = new Queue<char>(combo);
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

    public bool checkInput(char input)
    {
        //Reset on mess up
        if (input == '\0' || input == 'F')
        {
            currentCombo = new Queue<char>(combo);
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
            if (currentCombo.Count == 0)
            {
                Destroy(gameObject);
                return true;
            }
            return false;
        }
    }
}
