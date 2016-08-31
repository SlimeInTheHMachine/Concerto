using UnityEngine;
using System.Collections.Generic;

public class EnemyComboScript : MonoBehaviour {

    public bool Selected = false;
    [SerializeField]
    GameObject TextGameObject;
    Queue<string> ComboLetters;
	// Use this for initialization
	void Start () {
        ComboLetters = new Queue<string>();
        ComboLetters.Enqueue("space");
        ComboLetters.Enqueue("b");
        ComboLetters.Enqueue("c");
    }
	
	// Update is called once per frame
	void Update () {
        TextGameObject.GetComponent<TextMesh>().text = ComboLetters.Peek();
        if (Selected)
        {
            if (Input.GetKeyDown(ComboLetters.Peek()) && ComboLetters.Count != 0 )
            {
                string temp = ComboLetters.Dequeue();
                Debug.Log("Dequeue'd " + temp);
            }
        }
	}
}
