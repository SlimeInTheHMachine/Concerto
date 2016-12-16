using UnityEngine;
using System.Collections.Generic;
public enum Hits { high, med, low};
public class Combo2 : MonoBehaviour {
    
    public Stack<Hits> neededHits;// = { Hits.low, Hits.low, Hits.low };
    Stack<Hits> basicHits;
    GameObject childLetter;
    [SerializeField]
    Hits hit1, hit2, hit3;
    void Start()
    {
        neededHits = new Stack<Hits>();
        neededHits.Push(hit1);
        neededHits.Push(hit2);
        neededHits.Push(hit3);
        childLetter = transform.GetChild(0).gameObject;
        //Debug.Log(childLetter.name);
        basicHits = neededHits;
    }

    void Update()
    {
        if (neededHits.Count <= 0)
        {
            Destroy(gameObject, 2f);
            //get tossed
            if (PlatManager.instance.player.transform.position.x < transform.position.x)
            {
                // PlatManager.instance.player.GetComponent<Platformer>().GenerateRight();
                gameObject.AddComponent<Rigidbody2D>();
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 1) * 25, ForceMode2D.Impulse);
            }
            else
            {
                // PlatManager.instance.player.GetComponent<Platformer>().GenerateLeft();
                gameObject.AddComponent<Rigidbody2D>();
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1, 1) * 25, ForceMode2D.Impulse);
            }
            Destroy(this);
            PlatManager.instance.player.GetComponent<Platformer>().score++;
        }
        else
        //These show what the next hit required is.
        {
            if (neededHits.Peek() == Hits.high)
            {
                childLetter.GetComponent<TextMesh>().text = "A";
            }
            if (neededHits.Peek() == Hits.med)
            {
                childLetter.GetComponent<TextMesh>().text = "S";
            }
            if (neededHits.Peek() == Hits.low)
            {
                childLetter.GetComponent<TextMesh>().text = "D";
            }
        }
    }

    void ResetStack()
    {
        Debug.Log("Reset");
        neededHits = new Stack<Hits>();
        neededHits.Push(hit1);
        neededHits.Push(hit2);
        neededHits.Push(hit3);
    }


    //These will be important in Platformer, hush
    public void HitLow(Hits hit)
    {
        if(neededHits.Peek() == hit)
        {
            neededHits.Pop();
            PlatManager.instance.player.GetComponent<Platformer>().GenerateRight(-0.4f);

        }
        else
        {
            ResetStack();
        }
    }

    public void HitMed(Hits hit)
    {
        if (neededHits.Peek() == hit)
        {
            neededHits.Pop();
            PlatManager.instance.player.GetComponent<Platformer>().GenerateRight(0);
        }
        else
        {
            ResetStack();
        }
    }

    public void HitHigh(Hits hit)
    {
        if (neededHits.Peek() == hit)
        {
            neededHits.Pop();
            PlatManager.instance.player.GetComponent<Platformer>().GenerateRight(0.4f);
        }
        else
        {
            ResetStack();
        }
    }
}
