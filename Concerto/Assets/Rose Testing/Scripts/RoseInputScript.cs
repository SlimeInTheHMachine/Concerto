using UnityEngine;
using System.Collections;

public class RoseInputScript : MonoBehaviour {
    private attackInputs attackInput;
    public GameObject currentEnemy;
    // Use this for initialization
    void Start() {
        attackInput = attackInputs.None;
    }

    // Update is called once per frame
    void Update() {
        //Attack
        //Raycast to enemy
        if (Input.GetButtonDown("AButton") || Input.GetButtonDown("BButton") || Input.GetButtonDown("XButton")
            || Input.GetButtonDown("YButton") || (Input.GetButtonDown("Submit")))
        {
            Debug.Log("Input");
            //currentEnemy = rayHit.transform.gameObject.GetComponent<BlaneComboScript>();
            CombatInput();
            attackInput = attackInputs.None;
        }
    }

    void CombatInput()
    {
        //See if there is combat input
        //If multiple inputs, Garbage Input
        if (Input.GetButtonDown("AButton"))
        {
            if (attackInput == attackInputs.None)
                attackInput = attackInputs.A;
            else
                attackInput = attackInputs.Garbage;
        }
        if (Input.GetButtonDown("BButton"))
        {
            if (attackInput == attackInputs.None)
                attackInput = attackInputs.B;
            else
                attackInput = attackInputs.Garbage;
        }
        if (Input.GetButtonDown("XButton"))
        {
            if (attackInput == attackInputs.None)
                attackInput = attackInputs.X;
            else
                attackInput = attackInputs.Garbage;
        }
        if (Input.GetButtonDown("YButton"))
        {
            if (attackInput == attackInputs.None)
                attackInput = attackInputs.Y;
            else
                attackInput = attackInputs.Garbage;
        }
        if (Input.GetButtonDown("Submit"))
        {
            if (attackInput == attackInputs.None)
                attackInput = attackInputs.Garbage;
        }
        if (currentEnemy.GetComponent<BlaneComboScript>().checkInput(attackInput))
        {
            //clash code here
            //Clash();
            //currentEnemy = null;
            Debug.Log("Correct Input");
        }
    }

}
