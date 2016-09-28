﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class RoseComboScript : MonoBehaviour {

    //Public Variables
    public bool Selected = false;
    public attackInputs button1, button2, button3, button4;

    [SerializeField]
    GameObject letter1, letter2, letter3, letter4;
    //Private Variables
    private Queue<AttackInputWrapper> combo, currentCombo;
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
        combo = new Queue<AttackInputWrapper>();
        combo.Enqueue(btnHolder1);
        combo.Enqueue(btnHolder2);
        combo.Enqueue(btnHolder3);
        combo.Enqueue(btnHolder4);
        //Deep Clone
        currentCombo = new Queue<AttackInputWrapper>(combo);
        OGPos = transform.position;
        //Per enemy? //NOOOOOOOOOOOOO //Enemy Manager
        //BeatManager.onBeat += InputCheck;
    }

    // Update is called once per frame
    void Update ()
    {
        //needs to set the letter above it's head to the correct thing
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
            currentCombo = new Queue<AttackInputWrapper>(combo);
            return false;
        }
        else
        {
            if (input == currentCombo.Peek().thisInput)
            {
                currentCombo.Dequeue();
                shake = 0.05f;
                OGPos = transform.position;
            }
            else
            {
                currentCombo = combo;
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