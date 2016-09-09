using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EnemyComboScript : MonoBehaviour {

    public bool Selected = false;
    //[SerializeField]
    public GameObject GameControl;
    [SerializeField]
    GameObject TextGameObject;
    [SerializeField]
    Queue<string> ComboLetters;
	// Use this for initialization
	void Start () {

        GameControl = GameObject.FindGameObjectWithTag("GameController");
        //Puts in three standard buttons
        ComboLetters = new Queue<string>();
        ComboLetters.Enqueue("a");
        ComboLetters.Enqueue("b");
        ComboLetters.Enqueue("c");


        TextGameObject = gameObject.transform.Find("Enemy Letter").gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        if (GameControl.GetComponent<OnBeatRose>().Pressable)
        {
            Debug.Log("We could have pressed!");
        }
        TextGameObject.GetComponent<TextMesh>().text = ComboLetters.Peek();
        if (!Selected )
        {
            try
            {
                if(ComboLetters.Peek() == "EndQueue")
                {
                    Destroy(this.gameObject);
                }
                //Debug.Log("Is pressable? " + GameControl.GetComponent<OnBeatRose>().Pressable);
                if (Input.GetKeyDown(ComboLetters.Peek()) && GameControl.GetComponent<OnBeatRose>().Pressable)
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
