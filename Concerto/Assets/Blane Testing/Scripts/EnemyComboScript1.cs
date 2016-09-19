using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EnemyComboScript1 : MonoBehaviour {

    //Public Variables
    public bool Selected = false;
    public char button1, button2, button3;

    //Private Variables
    private Queue<char> combo, currentCombo;

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
        Debug.Log(combo.Peek());
        Debug.Log(currentCombo.Peek());
        currentCombo.Dequeue();
        Debug.Log(combo.Peek());
        Debug.Log(currentCombo.Peek());


        //Per enemy? //NOOOOOOOOOOOOO
        //BeatManager.onBeat += InputCheck;
    }

    // Update is called once per frame
    void Update ()
    {
        //Movement
	}

    public void checkInput(char input)
    {
        //Reset on mess up
        if (input == 0)
        {
            currentCombo = new Queue<char>(combo);
        }

        else
        {

        }
    }
}
