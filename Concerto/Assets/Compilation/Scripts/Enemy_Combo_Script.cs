using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Enemy_Combo_Script : MonoBehaviour
{
    GameObject beatManager;
    public double beatTime;

    public bool Selected = false;
    //[SerializeField]
    //public GameObject GameControl;
    [SerializeField]
    GameObject TextGameObject;
    [SerializeField]
    Queue<string> ComboLetters;

    /// <summary>
    /// Bool whether or not the ability to press buttons is there.
    /// </summary>
    

    // Use this for initialization
    void Start()
    {
        //Gets the time to be used for beats from the Beat Manager
        beatManager = GameObject.Find("BeatManager");
        beatTime = beatManager.GetComponent<BeatManager>().BeatTime;

        //GameControl = GameObject.FindGameObjectWithTag("GameController");
        //Puts in three standard buttons
        ComboLetters = new Queue<string>();
        ComboLetters.Enqueue("a");
        ComboLetters.Enqueue("b");
        ComboLetters.Enqueue("c");


        TextGameObject = gameObject.transform.Find("Enemy Letter").gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        TextGameObject.GetComponent<TextMesh>().text = ComboLetters.Peek();
        if (!Selected)
        {
            try
            {
                if (ComboLetters.Peek() == "EndQueue")
                {
                    Destroy(this.gameObject);
                }

                if (Input.GetKeyDown(ComboLetters.Peek()) && beatManager.GetComponent<BeatManager>().onTime)
                {
                    string temp = ComboLetters.Dequeue();
                    Debug.Log("Dequeue'd " + temp);
                    if (ComboLetters.Count == 1)
                    {
                        ComboLetters.Enqueue("EndQueue");
                    }
                }
            }
            catch (InvalidOperationException e)
            {
                Debug.Log(e);
                Destroy(this.gameObject);
            }
        }
    }
}
