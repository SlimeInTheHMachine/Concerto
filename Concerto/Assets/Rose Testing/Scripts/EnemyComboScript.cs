using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EnemyComboScript : MonoBehaviour {

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
    bool pressable = false;
	// Use this for initialization
	void Start () {

        //GameControl = GameObject.FindGameObjectWithTag("GameController");
        //Puts in three standard buttons
        ComboLetters = new Queue<string>();
        ComboLetters.Enqueue("a");
        ComboLetters.Enqueue("b");
        ComboLetters.Enqueue("c");
        BeatManager.onBeat += subscriberTest;
        BeatManager.safeBeforeBeat += SetBeatGood;


        TextGameObject = gameObject.transform.Find("Enemy Letter").gameObject;
    }

    void SetBeatGood()
    {
        pressable = true;
    }
    void subscriberTest()
    {
        pressable = false;
        Debug.Log("Entered subscriber function");
    }

    // Update is called once per frame
    void Update () {
       
        TextGameObject.GetComponent<TextMesh>().text = ComboLetters.Peek();
        if (!Selected )
        {
            try
            {
                if(ComboLetters.Peek() == "EndQueue")
                {
                    Destroy(this.gameObject);
                }
                
                if (Input.GetKeyDown(ComboLetters.Peek()) && pressable)
                {
                    string temp = ComboLetters.Dequeue();
                    Debug.Log("Dequeue'd " + temp);
                    if(ComboLetters.Count == 1)
                    {
                        ComboLetters.Enqueue("EndQueue");
                    }
                }
            }catch(InvalidOperationException e)
            {
                Debug.Log(e);
                Destroy(this.gameObject);
            }
        }
	}
}
