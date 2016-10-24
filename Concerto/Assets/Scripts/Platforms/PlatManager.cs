using UnityEngine;
using System.Collections;

public class PlatManager : MonoBehaviour
{

    //Prevents other instances of PlatformManager, since the constructor is restricted
    protected PlatManager() { }
    //static instance of PlatformManager
    public static PlatManager instance = null;

    //Public Variables
    GameObject beatManager;
    public GameObject player;
    BoxCollider2D playerCollider;
    Platformer playerScript;

    public Color color1;
    public Color color2;
    public int beatCounter;
    public int beatPerMove;

    //Private Variables
    private GameObject[] platforms;
    private GameObject[] platforms2;
    private SpriteRenderer[] platformSpriteRens;
    //Get all relevent components
    private GameObject[] movPlatformsVer;
    private GameObject[] movPlatformsHor;
    private GameObject[] spikes;
    private GameObject[] conPlatformsLeft;
    private GameObject[] fallThroughPlatforms;
    private GameObject[] conPlatformsRight;
    private GameObject platToMoveTo;

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

        playerCollider = player.GetComponent<BoxCollider2D>();
        playerScript = player.GetComponent<Platformer>();

        //Get List of platforms
        platforms = GameObject.FindGameObjectsWithTag("Platform");
        platforms2 = GameObject.FindGameObjectsWithTag("Platform2");
        //Get the spriteRenderers of each platform that changed colors
        platformSpriteRens = new SpriteRenderer[platforms.Length + platforms2.Length];
        for(int i = 0; i < platforms.Length; i++)
        {
            platformSpriteRens[i] = platforms[i].GetComponent<SpriteRenderer>();
        }
        for (int i = platforms.Length; i < platformSpriteRens.Length; i++)
        {
            platformSpriteRens[i] = platforms2[i - platforms.Length].GetComponent<SpriteRenderer>();
        }
        movPlatformsVer = GameObject.FindGameObjectsWithTag("MovingVertical");
        movPlatformsHor = GameObject.FindGameObjectsWithTag("MovingHorizantal");
        fallThroughPlatforms = GameObject.FindGameObjectsWithTag("FallthroughPlatform");
        spikes = GameObject.FindGameObjectsWithTag("Spikes");
        conPlatformsLeft = GameObject.FindGameObjectsWithTag("ConveyorLeft");
        conPlatformsRight = GameObject.FindGameObjectsWithTag("ConveyorRight"); 
    }

    // Use this for initialization
    void Start()
    {
        beatManager = GameObject.Find("BeatManager");
        beatCounter = 0;

        //Checks to see if the types of platforms can be found and then adds their behaviors accordingly
        if (platforms != null)
        {
            BeatMan.onBeat += platformBehavior;
            //Changes Platform Color
            for (int i = 0; i < platforms.Length; i++)
            {
                platformSpriteRens[i].color = color1;
            }
            for (int i = platforms.Length; i < platformSpriteRens.Length; i++)
            {
                platformSpriteRens[i].color = color2;
            }
        }
        if (conPlatformsLeft!= null)
        {
            BeatMan.onBeat += leftConveyorBehavior;
            for(int i = 0; i < conPlatformsLeft.Length; i++)
                conPlatformsLeft[i].GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        if (conPlatformsRight != null)
        {
            BeatMan.onBeat += rightConveyorBehavior;
            for (int i = 0; i < conPlatformsRight.Length; i++)
                conPlatformsRight[i].GetComponent<SpriteRenderer>().color = Color.green;
        }
        if (movPlatformsHor != null)
        {
            BeatMan.onBeat += horPlatBehavior;
            for (int i = 0; i < movPlatformsHor.Length; i++)
                movPlatformsHor[i].GetComponent<SpriteRenderer>().color = Color.blue;
        }
        if (movPlatformsVer != null)
        {
            BeatMan.onBeat += vertPlatBehavior;
            for (int i = 0; i < movPlatformsVer.Length; i++)
                movPlatformsVer[i].GetComponent<SpriteRenderer>().color = Color.cyan;
        }
        if (spikes != null)
        {
            BeatMan.onBeat += spikeBehavior;
            for (int i = 0; i < spikes.Length; i++)
                spikes[i].GetComponent<SpriteRenderer>().color = Color.red;

        }
        if (fallThroughPlatforms != null)
        {
            BeatMan.onBeat += fallThroughBehavior;
            Color color = Color.magenta;
            color.a = 0.1f;
            //Changes Platform Color
            for (int i = 0; i < fallThroughPlatforms.Length; i++)
                fallThroughPlatforms[i].GetComponent<SpriteRenderer>().color = color;
        }
        //BeatMan.startBeat += setPlayerPos;
        BeatMan.onBeat += beatCount;
    }

    // Changes the colors of the platforms
    void platformBehavior()
    {

        //Alternates the color between two possiblities
        for (int i = 0; i < platformSpriteRens.Length; i++)
        {
            if (platformSpriteRens[i].color == color1)
                platformSpriteRens[i].color = color2;
            else
                platformSpriteRens[i].color = color1;
        }

        //Checks to see whether normal platform exists
        for (int i = 0; i < platforms.Length; i++)
        {
            if(platforms[i].GetComponent<BoxCollider2D>().IsTouching(playerCollider))
            {
                //Ensure the player is in the center of the square
                platToMoveTo = platforms[i];
            }
        }

        //Checks to see whether a second type of normal platform exists
        if (platforms2 != null)
        {
        //Changes the colors of the first type of platform
            for (int i = 0; i < platforms2.Length; i++)
            {
                if (platforms2[i].GetComponent<BoxCollider2D>().IsTouching(playerCollider))
                {
                    //Ensure the player is in the center of the square
                    platToMoveTo = platforms2[i];
                }
            }
        }
    }
    
    void spikeBehavior()
    {
        for (int i = 0; i < spikes.Length; i++)
        {
            if (spikes[i].GetComponent<BoxCollider2D>().IsTouching(playerCollider))
            {
                //Ensure the player is in the center of the square
                platToMoveTo = spikes[i];
            }

        }
    }
    void fallThroughBehavior()
    {
        for (int i = 0; i < fallThroughPlatforms.Length; i++)
        {
            if (fallThroughPlatforms[i].GetComponent<BoxCollider2D>().IsTouching(playerCollider))
                //Ensure the player is in the center of the square
                platToMoveTo = fallThroughPlatforms[i];
        }
    }

    void beatCount()
    {
        //Sets a counter for actions that only occur every other beat
        beatCounter++;
        if (beatCounter == 2)
            beatCounter = 0;
    }

    void leftConveyorBehavior()
    {
        for (int i = 0; i < conPlatformsLeft.Length; i++)
        {
            
            if (conPlatformsLeft[i].GetComponent<BoxCollider2D>().IsTouching(playerCollider))
            {
                //Ensure the player is in the center of the square
                platToMoveTo = conPlatformsLeft[i];

                //Checks to see if the player has already moved, and shoots them backward if not
                if (playerScript.HaveMoved == false)
                {
                    playerScript.LerpDestination = new Vector2(player.transform.position.x - playerScript.lerpDistance, playerScript.transform.position.y);
                    playerScript.HaveMoved = true;
                    //player.transform.position = Vector2.Lerp(player.transform.position, playerScript.LerpDestination, playerScript.lerpTime * Time.fixedDeltaTime);
                }
            }
        }
    }

    void rightConveyorBehavior()
    {
        for (int i = 0; i < conPlatformsRight.Length; i++)
        {
            if (conPlatformsRight[i].GetComponent<BoxCollider2D>().IsTouching(playerCollider))
            {
                //Ensure the player is in the center of the square
                platToMoveTo = conPlatformsRight[i];

                //Checks to see if the player has already moved, and shoots them forward if not
                if (playerScript.HaveMoved == false)
                {
                    playerScript.LerpDestination = new Vector2(player.transform.position.x + playerScript.lerpDistance, playerScript.transform.position.y);
                    playerScript.HaveMoved = true;
                   // player.transform.position = Vector2.Lerp(player.transform.position, playerScript.LerpDestination, playerScript.lerpTime * Time.fixedDeltaTime);
                }
            }
        }
    }
    
    void vertPlatBehavior()
    {
        for (int i = 0; i < movPlatformsVer.Length; i++)
        {
            //Sets this behavior to occur every other beat
            if (beatCounter == beatPerMove)
            {
                if (movPlatformsVer[i].GetComponent<BoxCollider2D>().IsTouching(playerCollider))
                {
                    //Ensure the player is in the center of the square
                    platToMoveTo = movPlatformsVer[i];

                    //Lerps the Player in the direction the platform is moving in when the platform moves
                    if (movPlatformsVer[i].GetComponent<PlatMover>().moveForward)
                    {
                        playerScript.LerpDestination = new Vector2(player.transform.position.x, player.transform.position.y + movPlatformsVer[i].GetComponent<PlatMover>().up);
                        playerScript.HaveMoved = true;
                      //  player.transform.position = Vector2.Lerp(player.transform.position, playerScript.LerpDestination, playerScript.lerpTime * Time.fixedDeltaTime);
                    }
                    else
                    {
                        playerScript.LerpDestination = new Vector2(player.transform.position.x, player.transform.position.y - movPlatformsVer[i].GetComponent<PlatMover>().up);
                        playerScript.HaveMoved = true;
                       // player.transform.position = Vector2.Lerp(player.transform.position, playerScript.LerpDestination, playerScript.lerpTime * Time.fixedDeltaTime);
                    }
                }
                //Lerps the Platform
                movPlatformsVer[i].GetComponent<PlatMover>().moveVer();
            }
        }
    }

    void horPlatBehavior()
    {
        for (int i = 0; i < movPlatformsHor.Length; i++)
        {
            //Sets this behavior to occur every other beat
            if (beatCounter == beatPerMove)
            {
                if (movPlatformsHor[i].GetComponent<BoxCollider2D>().IsTouching(playerCollider))
                {
                    //Ensure the player is in the center of the square
                    platToMoveTo = movPlatformsHor[i];

                    //Lerps the Player in the direction the platform is moving in when the platform moves
                    if (movPlatformsHor[i].GetComponent<PlatMover>().moveForward)
                    {
                        playerScript.LerpDestination = new Vector2(player.transform.position.x + movPlatformsHor[i].GetComponent<PlatMover>().right, player.transform.position.y);
                        playerScript.HaveMoved = true;
                       // player.transform.position = Vector2.Lerp(player.transform.position, playerScript.LerpDestination, playerScript.lerpTime * Time.fixedDeltaTime);
                    }
                    else
                    {
                        playerScript.LerpDestination = new Vector2(player.transform.position.x - movPlatformsHor[i].GetComponent<PlatMover>().right, player.transform.position.y);
                        playerScript.HaveMoved = true;
                       // player.transform.position = Vector2.Lerp(player.transform.position, playerScript.LerpDestination, playerScript.lerpTime * Time.fixedDeltaTime);
                    }
                }
                //Lerps the Platform
                movPlatformsHor[i].GetComponent<PlatMover>().moveHor();
            }
        }
    }

    void setPlayerPos()
    {
        //Sets the player Lerpposition to be in the center and just above the platform they are currently touching
        //Issue that makes it so you cant move before on beat due to platform currently having priority
        if (platToMoveTo != null && platToMoveTo.GetComponent<BoxCollider2D>().IsTouching(playerCollider))
        {
            playerScript.LerpDestination = new Vector2(platToMoveTo.transform.position.x, platToMoveTo.transform.position.y + player.GetComponent<BoxCollider2D>().size.y/2 + platToMoveTo.GetComponent<BoxCollider2D>().bounds.size.y/2);
            platToMoveTo = null;
        }
    }
}
