using UnityEngine;
using System.Collections;

public class PlatManager : MonoBehaviour
{
    //Variables
    private GameObject[] platforms;
    private GameObject[] platforms2;
    private GameObject[] movPlatformsVer;
    private GameObject[] movPlatformsHor;
    //private GameObject[] trapPlatforms;
    private GameObject[] spikes;
    private GameObject[] conPlatformsLeft;
    private GameObject[] fallThroughPlatforms;
    private GameObject[] conPlatformsRight;
    private Color[] platColors;
    public GameObject player;
    BoxCollider2D playerCollider;
    Platformer playerScript;
    public Color color1;
    public Color color2;

    GameObject beatManager;
    private double beatTime;

    //Prevents other instances of PlatformManager, since the constructor is restricted
    protected PlatManager() { }
    //static instance of PlatformManager
    public static PlatManager instance = null;
    
    public int beatCounter;
    public int beatPerMove;

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
        movPlatformsVer = GameObject.FindGameObjectsWithTag("MovingVertical");
        movPlatformsHor = GameObject.FindGameObjectsWithTag("MovingHorizantal");
        //trapPlatforms = GameObject.FindGameObjectsWithTag("TrapDoor");
        fallThroughPlatforms = GameObject.FindGameObjectsWithTag("FallthroughPlatform");
        spikes = GameObject.FindGameObjectsWithTag("Spikes");
        conPlatformsLeft = GameObject.FindGameObjectsWithTag("ConveyorLeft");
        conPlatformsRight = GameObject.FindGameObjectsWithTag("ConveyorRight");
    }

    // Use this for initialization
    void Start()
    {
        platColors = new Color[6];
        platColors[0] = ConvertColor(255, 203, 244);
        platColors[1] = ConvertColor(23, 127, 117);

        beatManager = GameObject.Find("BeatManager");
        beatTime = beatManager.GetComponent<BeatMan>().BeatTime;
        beatCounter = 0;

        //Checks to see if the types of platforms can be found and then adds their behaviors accordingly
        if (platforms != null)
        {BeatMan.onBeat += platformBehavior; }
        if (conPlatformsLeft!= null)
        { BeatMan.onBeat += leftConveyorBehavior; }
        if (conPlatformsRight != null)
        { BeatMan.onBeat += rightConveyorBehavior; }
        if (movPlatformsHor != null)
        { BeatMan.onBeat += horPlatBehavior; }
        if (movPlatformsVer != null)
        { BeatMan.onBeat += vertPlatBehavior; }
        if (spikes != null)
        { BeatMan.onBeat += spikeBehavior; }
        if (fallThroughPlatforms != null)
        { BeatMan.onBeat += fallThroughBehavior; }
         BeatMan.onBeat += beatCount; 

    }

     //Update is called once per frame
    void Update()
    {
        //Call the update of every platform

    }

    // Changes the colors of the platforms
    void platformBehavior()
    {
        //Changes the colors of the first type of platform
        for (int i = 0; i < platforms.Length; i++)
        {
            if(platforms[i].GetComponent<BoxCollider2D>().IsTouching(playerCollider))
            {
                //Ensure the player is in the center of the square
                setPlayerPos(platforms[i]);
            }
            //Alternates the color between two possiblities
            if (platforms[i].GetComponent<Renderer>().material.color != platColors[1])
            { platforms[i].GetComponent<Renderer>().material.color = platColors[1]; }
            else if (platforms[i].GetComponent<Renderer>().material.color != platColors[0])
            { platforms[i].GetComponent<Renderer>().material.color = platColors[0];}
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
                    setPlayerPos(platforms2[i]);
                }
                //Alternates the color between two possiblities
                if (platforms2[i].GetComponent<Renderer>().material.color != platColors[0])
                { platforms2[i].GetComponent<Renderer>().material.color = platColors[0]; }
                else if (platforms2[i].GetComponent<Renderer>().material.color != platColors[1])
                { platforms2[i].GetComponent<Renderer>().material.color = platColors[1]; }
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
                setPlayerPos(spikes[i]);
            }
            //Changes Platform Color
            spikes[i].GetComponent<Renderer>().material.color = Color.red;
        }
    }
    void fallThroughBehavior()
    {
        Color color = Color.magenta;
        color.a = 0.1f;
        for (int i = 0; i < fallThroughPlatforms.Length; i++)
        {
            if (fallThroughPlatforms[i].GetComponent<BoxCollider2D>().IsTouching(playerCollider))
            {
                //Ensure the player is in the center of the square
                setPlayerPos(fallThroughPlatforms[i]);
            }
            //Changes Platform Color
            fallThroughPlatforms[i].GetComponent<Renderer>().material.color = color;
            
        }
    }
    void beatCount()
    {
        //Sets a counter for actions that only occur every other beat
        beatCounter++;
        if (beatCounter == 2)
            beatCounter = 0;
    }
    Color ConvertColor( int r,  int g,  int b)
    {
        return new Color(r/255.0f, g/255.0f, b/255.0f);
    }

//void openTrap()
//    {
//        for (int i = 0; i < trapPlatforms.Length; i++)
//        {
//            if (beatCounter == beatPerMove)
//            {
//                playerScript.Grounded = false;
//                playerScript.CanFall = true;
//            }
//        }
//    }



    void leftConveyorBehavior()
    {
        for (int i = 0; i < conPlatformsLeft.Length; i++)
        {
            //Changes Platform Color
            conPlatformsLeft[i].GetComponent<Renderer>().material.color = Color.yellow;
            if (conPlatformsLeft[i].GetComponent<BoxCollider2D>().IsTouching(playerCollider))
            {
                //Ensure the player is in the center of the square
                setPlayerPos(conPlatformsLeft[i]);

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
            //Changes Platform Color
            conPlatformsRight[i].GetComponent<Renderer>().material.color = Color.green;
            if (conPlatformsRight[i].GetComponent<BoxCollider2D>().IsTouching(playerCollider))
            {
                //Ensure the player is in the center of the square
                setPlayerPos(conPlatformsRight[i]);

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
            //Changes Platform Color
            movPlatformsVer[i].GetComponent<Renderer>().material.color = Color.cyan;

            //Sets this behavior to occur every other beat
            if (beatCounter == beatPerMove)
            {
                if (movPlatformsVer[i].GetComponent<BoxCollider2D>().IsTouching(playerCollider))
                {
                    //Ensure the player is in the center of the square
                    setPlayerPos(movPlatformsVer[i]);

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
            //Changes Platform Color
            movPlatformsHor[i].GetComponent<Renderer>().material.color = Color.blue;

            //Sets this behavior to occur every other beat
            if (beatCounter == beatPerMove)
            {
                if (movPlatformsHor[i].GetComponent<BoxCollider2D>().IsTouching(playerCollider))
                {
                    //Ensure the player is in the center of the square
                    setPlayerPos(movPlatformsHor[i]);

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

    void setPlayerPos( GameObject platform)
    {
        //Sets the player Lerpposition to be in the center and just above the platform they are currently touching
        //Issue that makes it so you cant move before on beat due to platform currently having priority
        if (platform.GetComponent<BoxCollider2D>().IsTouching(playerCollider))
        {
            playerScript.LerpDestination = new Vector2(platform.transform.position.x, platform.transform.position.y + player.GetComponent<BoxCollider2D>().size.y/2 + platform.GetComponent<BoxCollider2D>().bounds.size.y/2);
        }
    }
}
