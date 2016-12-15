using UnityEngine;
using System.Collections;

public class EnemyMan : MonoBehaviour {

    //Variables
    private GameObject[] enemies;

    //Prevents other instances of EnemyManager, since the constructor is restricted
    protected EnemyMan() { }
    //static instance of EnemyManager
    public static EnemyMan instance = null;
    
    //Awake the object (Before Start)
    void Awake()
    {
        //Check if instance exists
        if (instance == null)
            //If not, assign this to it.
            instance = this;
        else if (instance != this)
            //If so (somehow), destroy this object.
            Destroy(gameObject);

        //Get List of enemies
       
    }

    // Use this for initialization
    void Start () {

        BeatMan.onBeat += UpdateEnemies;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }
	
    void UpdateEnemies()
    {
        foreach(GameObject enemy in enemies)
        {
			if((enemy!= null) && (enemy.GetComponent<ComboScript>()))
                enemy.GetComponent<ComboScript>().EnemyUpdate();
        }
    }
	// Update is called once per frame
	void Update () {
	    //Call the update of every enemy

	}
}
