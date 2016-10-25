using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.IO;


public class IO : MonoBehaviour {

    public string levelFilePath;
    public GameObject Q;
    public GameObject W;
    public GameObject Z;
    public GameObject X;
    public GameObject N;
    public GameObject M;
    public GameObject S;
    public GameObject F;
    public GameObject C;
    public GameObject P;
    public GameObject E;
    public GameObject D;

    string defaultFilePath = "Assets/Scripts/LevelIO/levels/levelTest.txt";
    bool comment = false;
    bool sizeNext = false;
    bool levelNext = false;
    int lineNum;
    int width, height;
    char[,] tileChars;
    GameObject[,] tiles;
    

	// Use this for initialization
	void Awake () {
	    if(levelFilePath == null || !File.Exists(levelFilePath))
        {
            levelFilePath = defaultFilePath;
        }
        readTextFile(levelFilePath);
        parseData();
	}
	

    //read in
    void readTextFile(string filePath)
    {
        StreamReader inputStream = new StreamReader(filePath);
        while(!inputStream.EndOfStream)
        {
            string inputLine = inputStream.ReadLine();
            if (inputLine == "#--")
                comment = true;
            else if (inputLine == "Size:")
                sizeNext = true;
            else if (inputLine == "Level:")
            {
                levelNext = true;
                lineNum = 0;
            }
            else if (!comment)
            {
                char[] inputChars = new char[inputLine.Length];
                StringReader sr = new StringReader(inputLine);
                sr.Read(inputChars, 0, inputLine.Length);
                if (sizeNext)
                {
                    bool heightYet = false;
                    string wid = "";
                    string hi = "";
                    for(int i = 0; i < inputChars.Length; i++)
                    {
                        if (inputChars[i] == 'x')
                            heightYet = true;
                        else
                        {
                            if (!heightYet)
                                wid += inputChars[i];
                            else
                                hi += inputChars[i];
                        }
                    }
                    width = Int32.Parse(wid);
                    height = Int32.Parse(hi);
                    tileChars = new char[width, height];
                    sizeNext = false;
                }
                else if(levelNext && lineNum < height)
                {
                    
                    char[] nonSpaces = new char[width];
                    int j = 0;
                    for (int i = 0; i < inputChars.Length; i++)
                    {
                        if (inputChars[i] != ' ')
                        {
                            nonSpaces[j] = inputChars[i];
                            j++;
                        }
                    }
                    for (int i = 0; i < nonSpaces.Length; i++)
                    {
                        Debug.Log(nonSpaces[i]);
                        //Handling
                        tileChars[i, lineNum] = nonSpaces[i];
                    }
                    lineNum++;
                    
                }

            }
            else if (inputLine == "--#")
                comment = false;
        }

        inputStream.Close();
    }

    void parseData()
    {
        tiles = new GameObject[width, height];
        for(int i = 0; i > -height; i--)
        {
            for(int k = 0; k < width; k++)
            {
                switch(tileChars[k, -i])
                {
                    //Platform
                    case 'Q':
                        Instantiate(Q, new Vector3(k, i, 0f), Quaternion.identity);
                        break;
                    //Platform2
                    case 'W':
                        Instantiate(W, new Vector3(k, i, 0f), Quaternion.identity);
                        break;
                    //MovingHorizontal
                    case 'Z':
                        Instantiate(Z, new Vector3(k, i, 0f), Quaternion.identity);
                        break;
                    //MovingVertical
                    case 'X':
                        Instantiate(X, new Vector3(k, i, 0f), Quaternion.identity);
                        break;
                    //ConveyorLeft
                    case 'N':
                        Instantiate(N, new Vector3(k, i, 0f), Quaternion.identity);
                        break;
                    //ConveyorRight
                    case 'M':
                        Instantiate(M, new Vector3(k, i, 0f), Quaternion.identity);
                        break;
                    //Spikes
                    case 'S':
                        Instantiate(S, new Vector3(k, i, 0f), Quaternion.identity);
                        break;
                    //FallthroughPlatform
                    case 'F':
                        Instantiate(F, new Vector3(k, i, 0f), Quaternion.identity);
                        break;
                    //Checkpoint
                    case 'C':
                        Instantiate(C, new Vector3(k, i, 0f), Quaternion.identity);
                        break;
                    //Player
                    case 'P':
                        Instantiate(P, new Vector3(k, i, 0f), Quaternion.identity);
                        break;
                    //Enemy
                    case 'E':
                        Instantiate(E, new Vector3(k, i, 0f), Quaternion.identity);
                        break;
                    //Finish
                    case 'D':
                        Instantiate(D, new Vector3(k, i, 0f), Quaternion.identity);
                        break;
                    //Blank
                    case 'B':
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
