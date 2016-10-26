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
    private SpriteRenderer[] platMoverRens;
    private BoxCollider2D[] platformCols;
    //Get all relevent components
    private GameObject[] movPlatformsVer;
    private BoxCollider2D[] movPlatformsVerCols;
    private GameObject[] movPlatformsHor;
    private BoxCollider2D[] movPlatformsHorCols;
    private GameObject[] spikes;
    private BoxCollider2D[] spikeCols;
    private GameObject[] conPlatformsLeft;
    private BoxCollider2D[] conPlatformsLeftCols;
    private GameObject[] fallThroughPlatforms;
    private BoxCollider2D[] fallThroughPlatformsCols;
    private GameObject[] conPlatformsRight;
    private BoxCollider2D[] conPlatformsRightCols;
    private PlatMover[] platMovers;
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

        

        //Get List of platforms
        platforms = GameObject.FindGameObjectsWithTag("Platform");
        platforms2 = GameObject.FindGameObjectsWithTag("Platform2");
        movPlatformsVer = GameObject.FindGameObjectsWithTag("MovingVertical");
        movPlatformsHor = GameObject.FindGameObjectsWithTag("MovingHorizantal");
        fallThroughPlatforms = GameObject.FindGameObjectsWithTag("FallthroughPlatform");
        spikes = GameObject.FindGameObjectsWithTag("Spikes");
        conPlatformsLeft = GameObject.FindGameObjectsWithTag("ConveyorLeft");
        conPlatformsRight = GameObject.FindGameObjectsWithTag("ConveyorRight");

        //Get the spriteRenderers of each platform that changed colors
        platformSpriteRens = new SpriteRenderer[platforms.Length + platforms2.Length];
        platMoverRens = new SpriteRenderer[movPlatformsVer.Length + movPlatformsHor.Length];

        //Get Colliders to check against the player Collider
        platformCols = new BoxCollider2D[platforms.Length + platforms2.Length];
        movPlatformsVerCols = new BoxCollider2D[movPlatformsVer.Length];
        movPlatformsHorCols = new BoxCollider2D[movPlatformsHor.Length];
        conPlatformsLeftCols = new BoxCollider2D[conPlatformsLeft.Length];
        conPlatformsRightCols = new BoxCollider2D[conPlatformsRight.Length];
        fallThroughPlatformsCols = new BoxCollider2D[fallThroughPlatforms.Length];
        spikeCols = new BoxCollider2D[spikes.Length];

        //Gets the Horizantal Platform Movers
        platMovers = new PlatMover[movPlatformsVer.Length + movPlatformsHor.Length];

        for (int i = 0; i < platforms.Length; i++)
        {
            platformSpriteRens[i] = platforms[i].GetComponent<SpriteRenderer>();
        }
        for (int i = platforms.Length; i < platformSpriteRens.Length; i++)
        {
            platformSpriteRens[i] = platforms2[i - platforms.Length].GetComponent<SpriteRenderer>();
        }
        for (int i = 0; i < movPlatformsVer.Length; i++)
        {
            platMoverRens[i] = movPlatformsVer[i].GetComponent<SpriteRenderer>();
        }
        for (int i = movPlatformsVer.Length; i < platMovers.Length; i++)
        {
            platMoverRens[i] = movPlatformsHor[i - movPlatformsVer.Length].GetComponent<SpriteRenderer>();
        }

        for (int i = 0; i < platforms.Length; i++)
        {
            platformCols[i] = platforms[i].GetComponent<BoxCollider2D>();
        }
        for (int i = platforms.Length; i < platformSpriteRens.Length; i++)
        {
            platformCols[i] = platforms2[i - platforms.Length].GetComponent<BoxCollider2D>();
        }
        for (int i = 0; i < movPlatformsVer.Length; i++)
        {
            movPlatformsVerCols[i] = movPlatformsVer[i].GetComponent<BoxCollider2D>();
        }
        for (int i = 0; i < movPlatformsHor.Length; i++)
        {
            movPlatformsHorCols[i] = movPlatformsHor[i].GetComponent<BoxCollider2D>();
        }
        for (int i = 0; i < spikes.Length; i++)
        {
            spikeCols[i] = spikes[i].GetComponent<BoxCollider2D>();
        }
        for (int i = 0; i < conPlatformsLeft.Length; i++)
        {
            conPlatformsLeftCols[i] = conPlatformsLeft[i].GetComponent<BoxCollider2D>();
        }
        for (int i = 0; i < conPlatformsRight.Length; i++)
        {
            conPlatformsRightCols[i] = conPlatformsRight[i].GetComponent<BoxCollider2D>();
        }
        for (int i = 0; i < fallThroughPlatforms.Length; i++)
        {
            fallThroughPlatformsCols[i] = fallThroughPlatforms[i].GetComponent<BoxCollider2D>();
        }
        for (int i = 0; i < platMovers.Length; i++)
        {
            if (i < movPlatformsVer.Length)
            {
                //Ensure the player is in the center of the square
                platMovers[i] = movPlatformsVer[i].GetComponent<PlatMover>();
            }
            else
            {
                if (platforms2 != null)
                    platMovers[i] = movPlatformsHor[i - movPlatformsVer.Length].GetComponent<PlatMover>();
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        playerCollider = player.GetComponent<BoxCollider2D>();
        playerScript = player.GetComponent<Platformer>();
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
            BeatMan.endBeat += leftConveyorBehavior;
            for(int i = 0; i < conPlatformsLeft.Length; i++)
                conPlatformsLeft[i].GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        if (conPlatformsRight != null)
        {
            BeatMan.endBeat += rightConveyorBehavior;
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
            //BeatMan.onBeat += spikeBehavior;
            for (int i = 0; i < spikes.Length; i++)
                spikes[i].GetComponent<SpriteRenderer>().color = Color.red;

        }
        if (fallThroughPlatforms != null)
        {
            //BeatMan.onBeat += fallThroughBehavior;
            Color color = Color.magenta;
            color.a = 0.1f;
            //Changes Platform Color
            for (int i = 0; i < fallThroughPlatforms.Length; i++)
                fallThroughPlatforms[i].GetComponent<SpriteRenderer>().color = color;
        }
        BeatMan.endBeat += setPlayerPos;
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
        for (int i = 0; i < platformCols.Length; i++)
        {
            if(platformCols[i].IsTouching(playerCollider))
            {
                if (i < platforms.Length)
                {
                    //Ensure the player is in the center of the square
                    platToMoveTo = platforms[i];
                }
                else
                {
                    if (platforms2 != null)
                        platToMoveTo = platforms2[i - platforms.Length];
                }
            }
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
            
            if (conPlatformsLeftCols[i].IsTouching(playerCollider))
            {
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
            if (conPlatformsRightCols[i].IsTouching(playerCollider))
            {
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
            if ((platMovers[i].up >0))
            {
                if (platMovers[i].moveForward)
                    platMoverRens[i].sprite = platMovers[i].sprite1;
                else
                    platMoverRens[i].sprite = platMovers[i].sprite2;
            }
            else
            {
                if (platMovers[i].moveForward)
                    platMoverRens[i].sprite = platMovers[i].sprite2;
                else
                    platMoverRens[i].sprite = platMovers[i].sprite1;
            }

            //Sets this behavior to occur every other beat
            if (beatCounter == beatPerMove)
            {
                if (movPlatformsVerCols[i].IsTouching(playerCollider))
                {
                    //Ensure the player is in the center of the square
                    platToMoveTo = movPlatformsVer[i];

                    //Lerps the Player in the direction the platform is moving in when the platform moves
                    if (platMovers[i].moveForward)
                    {
                        playerScript.LerpDestination = new Vector2(player.transform.position.x, player.transform.position.y + platMovers[i].up);
                        playerScript.HaveMoved = true;
                        //  player.transform.position = Vector2.Lerp(player.transform.position, playerScript.LerpDestination, playerScript.lerpTime * Time.fixedDeltaTime);
                    }
                    else
                    {
                        playerScript.LerpDestination = new Vector2(player.transform.position.x, player.transform.position.y - platMovers[i].up);
                        playerScript.HaveMoved = true;
                        // player.transform.position = Vector2.Lerp(player.transform.position, playerScript.LerpDestination, playerScript.lerpTime * Time.fixedDeltaTime);
                    }
                }
                //Lerps the Platform
                platMovers[i].moveVer();
            }
        }
    }

    void horPlatBehavior()
    {
        for (int i = 0; i < movPlatformsHor.Length; i++)
        {
            if ((platMovers[i].right > 0))
            {
                if (platMovers[i + movPlatformsVer.Length].moveForward)
                    platMoverRens[i + movPlatformsVer.Length].sprite = platMovers[i + movPlatformsVer.Length].sprite1;
                else
                    platMoverRens[i + movPlatformsVer.Length].sprite = platMovers[i + movPlatformsVer.Length].sprite2;
            }
            else
            {
                if (platMovers[i + movPlatformsVer.Length].moveForward)
                    platMoverRens[i + movPlatformsVer.Length].sprite = platMovers[i + movPlatformsVer.Length].sprite2;
                else
                    platMoverRens[i + movPlatformsVer.Length].sprite = platMovers[i + movPlatformsVer.Length].sprite1;
            }
            //Sets this behavior to occur every other beat
            if (beatCounter == beatPerMove)
            {
                if (movPlatformsHorCols[i].IsTouching(playerCollider))
                {
                    //Ensure the player is in the center of the square
                    platToMoveTo = movPlatformsHor[i];

                    //Lerps the Player in the direction the platform is moving in when the platform moves
                    if (platMovers[i + movPlatformsVer.Length].moveForward)
                    {
                        playerScript.LerpDestination = new Vector2(player.transform.position.x + platMovers[i + movPlatformsVer.Length].right, player.transform.position.y);
                        playerScript.HaveMoved = true;
                       // player.transform.position = Vector2.Lerp(player.transform.position, playerScript.LerpDestination, playerScript.lerpTime * Time.fixedDeltaTime);
                    }
                    else
                    {
                        playerScript.LerpDestination = new Vector2(player.transform.position.x - platMovers[i + movPlatformsVer.Length].right, player.transform.position.y);
                        playerScript.HaveMoved = true;
                       // player.transform.position = Vector2.Lerp(player.transform.position, playerScript.LerpDestination, playerScript.lerpTime * Time.fixedDeltaTime);
                    }
                }
                //Lerps the Platform
                platMovers[i + movPlatformsVer.Length].moveHor();
            }
        }
    }

    void setPlayerPos()
    {
        //Sets the player Lerpposition to be in the center and just above the platform they are currently touching
        if (platToMoveTo != null && platToMoveTo.GetComponent<BoxCollider2D>().IsTouching(playerCollider))
        {
            playerScript.LerpDestination = new Vector2(platToMoveTo.transform.position.x, platToMoveTo.transform.position.y + playerCollider.size.y/2 + platToMoveTo.GetComponent<BoxCollider2D>().bounds.size.y/2);
            platToMoveTo = null;
        }
    }
}
