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
        platColors[0] = color1;
        platColors[1] = color2;

        beatManager = GameObject.Find("BeatManager");
        beatTime = beatManager.GetComponent<BeatMan>().BeatTime;
        beatCounter = 0;
        if (platforms != null)
        {BeatMan.onBeat += changeColors; }
        if (conPlatformsLeft!= null)
        { BeatMan.onBeat += movePlayerLeft; }
        if (conPlatformsRight != null)
        { BeatMan.onBeat += movePlayerRight; }
        if (movPlatformsHor != null)
        { BeatMan.onBeat += movePlatHor; }
        if (movPlatformsVer != null)
        { BeatMan.onBeat += movePlatVert; }
        if (spikes != null)
        { BeatMan.onBeat += colorSpikes; }
        if (fallThroughPlatforms != null)
        { BeatMan.onBeat += colorFallThrough; }
         BeatMan.onBeat += beatCount; 

    }

     //Update is called once per frame
    void Update()
    {
        //Call the update of every platform

    }
    void changeColors()
    {
        
        for (int i = 0; i < platforms.Length; i++)
        {
            if(platforms[i].GetComponent<BoxCollider2D>().IsTouching(playerCollider))
            {
                setPlayerPos(platforms[i]);
            }
            if (platforms[i].GetComponent<Renderer>().material.color != platColors[1])
            { platforms[i].GetComponent<Renderer>().material.color = platColors[1]; }
            else if (platforms[i].GetComponent<Renderer>().material.color != platColors[0])
            { platforms[i].GetComponent<Renderer>().material.color = platColors[0];}
        }
        if (platforms2 != null)
        {
            for (int i = 0; i < platforms2.Length; i++)
            {
                if (platforms2[i].GetComponent<BoxCollider2D>().IsTouching(playerCollider))
                {
                    setPlayerPos(platforms2[i]);
                }
                if (platforms2[i].GetComponent<Renderer>().material.color != platColors[0])
                { platforms2[i].GetComponent<Renderer>().material.color = platColors[0]; }
                else if (platforms2[i].GetComponent<Renderer>().material.color != platColors[1])
                { platforms2[i].GetComponent<Renderer>().material.color = platColors[1]; }
            }
        }
    }


    void colorSpikes()
    {
        for (int i = 0; i < spikes.Length; i++)
        {
            if (spikes[i].GetComponent<BoxCollider2D>().IsTouching(playerCollider))
            {
                setPlayerPos(spikes[i]);
            }
            spikes[i].GetComponent<Renderer>().material.color = Color.red;
        }
    }
    void colorFallThrough()
    {
        Color color = Color.magenta;
        color.a = 0.1f;
        for (int i = 0; i < fallThroughPlatforms.Length; i++)
        {
            if (fallThroughPlatforms[i].GetComponent<BoxCollider2D>().IsTouching(playerCollider))
            {
                setPlayerPos(fallThroughPlatforms[i]);
            }
            fallThroughPlatforms[i].GetComponent<Renderer>().material.color = color;
            
        }
    }
    void beatCount()
    {
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



    void movePlayerLeft()
    {
        for (int i = 0; i < conPlatformsLeft.Length; i++)
        {
            conPlatformsLeft[i].GetComponent<Renderer>().material.color = Color.yellow;
            if (conPlatformsLeft[i].GetComponent<BoxCollider2D>().IsTouching(playerCollider))
            {
                setPlayerPos(conPlatformsLeft[i]);
                if (playerScript.HaveMoved == false)
                {
                    playerScript.LerpDestination = new Vector2(player.transform.position.x - playerScript.lerpDistance, playerScript.transform.position.y);
                    playerScript.HaveMoved = true;
                    player.transform.position = Vector2.Lerp(player.transform.position, playerScript.LerpDestination, playerScript.lerpTime * Time.fixedDeltaTime);
                }
            }
        }
    }

    void movePlayerRight()
    {
        for (int i = 0; i < conPlatformsRight.Length; i++)
        {
            conPlatformsRight[i].GetComponent<Renderer>().material.color = Color.green;
            if (conPlatformsRight[i].GetComponent<BoxCollider2D>().IsTouching(playerCollider))
            {
                setPlayerPos(conPlatformsRight[i]);
                if (playerScript.HaveMoved == false)
                {
                    playerScript.LerpDestination = new Vector2(player.transform.position.x + playerScript.lerpDistance, playerScript.transform.position.y);
                    playerScript.HaveMoved = true;
                    player.transform.position = Vector2.Lerp(player.transform.position, playerScript.LerpDestination, playerScript.lerpTime * Time.fixedDeltaTime);
                }
            }
        }
    }
    
    void movePlatVert()
    {
        for (int i = 0; i < movPlatformsVer.Length; i++)
        {
            movPlatformsVer[i].GetComponent<Renderer>().material.color = Color.cyan;
            if (beatCounter == beatPerMove)
            {
                if (movPlatformsVer[i].GetComponent<BoxCollider2D>().IsTouching(playerCollider))
                {
                    setPlayerPos(movPlatformsVer[i]);
                    if (movPlatformsVer[i].GetComponent<PlatMoverVertical>().moveForward)
                    {
                        //player.transform.SetParent(movPlatformsVer[i].transform);
                        playerScript.LerpDestination = new Vector2(player.transform.position.x, player.transform.position.y + movPlatformsVer[i].GetComponent<PlatMoverVertical>().up);
                        playerScript.HaveMoved = true;
                        player.transform.position = Vector2.Lerp(player.transform.position, playerScript.LerpDestination, playerScript.lerpTime * Time.fixedDeltaTime);
                    }
                    else
                    {
                        playerScript.LerpDestination = new Vector2(player.transform.position.x, player.transform.position.y - movPlatformsVer[i].GetComponent<PlatMoverVertical>().up);
                        playerScript.HaveMoved = true;
                        player.transform.position = Vector2.Lerp(player.transform.position, playerScript.LerpDestination, playerScript.lerpTime * Time.fixedDeltaTime);
                    }
                }
                movPlatformsVer[i].GetComponent<PlatMoverVertical>().move();
            }
        }
    }

    void movePlatHor()
    {
        for (int i = 0; i < movPlatformsHor.Length; i++)
        {
            movPlatformsHor[i].GetComponent<Renderer>().material.color = Color.blue;
            if (beatCounter == beatPerMove)
            {
                if (movPlatformsHor[i].GetComponent<BoxCollider2D>().IsTouching(playerCollider))
                {
                    setPlayerPos(movPlatformsHor[i]);
                    if (movPlatformsHor[i].GetComponent<PlatMoverHorizantal>().moveForward)
                    {
                        playerScript.LerpDestination = new Vector2(player.transform.position.x + movPlatformsHor[i].GetComponent<PlatMoverHorizantal>().right, player.transform.position.y);
                        playerScript.HaveMoved = true;
                        player.transform.position = Vector2.Lerp(player.transform.position, playerScript.LerpDestination, playerScript.lerpTime * Time.fixedDeltaTime);
                    }
                    else
                    {
                        playerScript.LerpDestination = new Vector2(player.transform.position.x - movPlatformsHor[i].GetComponent<PlatMoverHorizantal>().right, player.transform.position.y);
                        playerScript.HaveMoved = true;
                        player.transform.position = Vector2.Lerp(player.transform.position, playerScript.LerpDestination, playerScript.lerpTime * Time.fixedDeltaTime);
                    }
                    }
                movPlatformsHor[i].GetComponent<PlatMoverHorizantal>().move();
            }
        }
    }

    void setPlayerPos( GameObject platform)
    {
        if (platform.GetComponent<BoxCollider2D>().IsTouching(playerCollider))
        {
            player.transform.position = new Vector2(platform.transform.position.x + 0.086f, platform.transform.position.y + 0.889f);
        }
    }
}
