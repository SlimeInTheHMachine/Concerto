using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EnemyComboScript : MonoBehaviour {

    public bool Selected = false;
    [SerializeField]
    GameObject TextGameObject;
    [SerializeField]
    Queue<string> ComboLetters;
	// Use this for initialization
	void Start () {

        //Puts in three standard buttons
        ComboLetters = new Queue<string>();
        ComboLetters.Enqueue("a");
        ComboLetters.Enqueue("b");
        ComboLetters.Enqueue("c");


        TextGameObject = gameObject.transform.Find("Enemy Letter").gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        TextGameObject.GetComponent<TextMesh>().text = ComboLetters.Peek();
        if (!Selected)
        {
            try
            {
                if(ComboLetters.Peek() == "EndQueue")
                {
                    Destroy(this.gameObject);
                }
                if (Input.GetKeyDown(ComboLetters.Peek()))
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
