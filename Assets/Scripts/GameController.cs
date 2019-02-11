using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// quick scene reset for debugging
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    private float boardPosZ;
    public GameObject boardSpace;
    public List<GameObject> boardSpaceList;
    public List<string> validPathTags = new List<string>();
    public GameObject boardEdge;
    public float boardScale = 10;
    public int boardSize = 9;
    public GameObject selectedTile;

    private int score;

    // Scoring Systems
    public List<TileSystem> tileSystemList;
    public TileSystem tileSystem;
    readonly string tileSystemNamingString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private int tileSystemNamingInt = 0;


    // Start is called before the first frame update
    void Start()
    {
        // Locate Board
        GameObject gameBoard = GameObject.FindWithTag("Board");
        if (gameBoard != null)
        {
            boardPosZ = gameBoard.transform.position.z;
        }
        if (gameBoard == null)
        {
            Debug.Log("Cannot find 'GameController' script");
            boardPosZ = 0;
        }

        Vector3 BoardCenter = new Vector3(boardScale / 2, 0, boardScale / 2 + boardPosZ);
        GenerateBoard(boardSize, boardSize, BoardCenter);
        score = 0;

        // Define Valid Path Tags
        validPathTags.Add("Trail");
        validPathTags.Add("River");
    }

    void GenerateBoard(int sizeX, int sizeZ, Vector3 BoardCenter)
    {
        Vector3 spawnPosition = new Vector3();
        spawnPosition.x = BoardCenter.x - (sizeX * boardScale / 2);
        spawnPosition.z = BoardCenter.z - (sizeZ * boardScale / 2);
        Quaternion spawnRotation = new Quaternion();
        for (int y = 0; y < sizeZ; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                if (x == 0 || x == boardSize - 1 || y == 0 || y == boardSize - 1)
                {
                    GameObject newBoardEdge = Instantiate(boardEdge, spawnPosition, spawnRotation);
                    if (x == 2 || x == 6)
                    {
                        SpriteRenderer tileSprite = newBoardEdge.GetComponentInChildren<SpriteRenderer>();
                        tileSprite.sprite = Resources.Load<Sprite>("Sprites/tStraight");
                        newBoardEdge.GetComponentInChildren<TileController>().northPath.gameObject.tag = "Trail";
                        newBoardEdge.GetComponentInChildren<TileController>().southPath.gameObject.tag = "Trail";
                        newBoardEdge.GetComponentInChildren<TileController>().tileSystem = Instantiate(tileSystem);
                        newBoardEdge.GetComponentInChildren<TileController>().tileSystem.tileSystemName = tileSystemNamingString[tileSystemNamingInt];
                        tileSystemNamingInt++;
                    }
                    else if (x == 4)
                    {
                        SpriteRenderer tileSprite = newBoardEdge.GetComponentInChildren<SpriteRenderer>();
                        tileSprite.sprite = Resources.Load<Sprite>("Sprites/rStraight");
                        newBoardEdge.GetComponentInChildren<TileController>().northPath.gameObject.tag = "River";
                        newBoardEdge.GetComponentInChildren<TileController>().southPath.gameObject.tag = "River";
                    }
                    else if (y == 2 || y == 6)
                    {
                        SpriteRenderer tileSprite = newBoardEdge.GetComponentInChildren<SpriteRenderer>();
                        tileSprite.sprite = Resources.Load<Sprite>("Sprites/rStraight");
                        newBoardEdge.GetComponentInChildren<TileController>().northPath.gameObject.tag = "River";
                        newBoardEdge.GetComponentInChildren<TileController>().southPath.gameObject.tag = "River";
                        newBoardEdge.transform.Rotate(Vector3.up * 90);
                    }
                    else if (y == 4)
                    {
                        SpriteRenderer tileSprite = newBoardEdge.GetComponentInChildren<SpriteRenderer>();
                        tileSprite.sprite = Resources.Load<Sprite>("Sprites/tStraight"); 
                        newBoardEdge.GetComponentInChildren<TileController>().northPath.gameObject.tag = "Trail";
                        newBoardEdge.GetComponentInChildren<TileController>().southPath.gameObject.tag = "Trail";
                        newBoardEdge.transform.Rotate(Vector3.up * 90);
                    }
                    else
                    {
                        SpriteRenderer tileSprite = newBoardEdge.GetComponentInChildren<SpriteRenderer>();
                        tileSprite.sprite = Resources.Load<Sprite>("Sprites/");
                    }
                }
                else
                {
                    GameObject newBoardSpace = Instantiate(boardSpace, spawnPosition, spawnRotation);
                    boardSpaceList.Add(newBoardSpace);
                }
                spawnPosition.x = spawnPosition.x + boardScale;
            }
            spawnPosition.x = BoardCenter.x - (sizeX * boardScale / 2);
            spawnPosition.z = spawnPosition.z + boardScale;
        }
    }
    // Update is called once per frame

    void Update()
    {
       // quick scene reset for debugging
       if (Input.GetKeyDown("backspace"))
        {
            SceneManager.LoadScene(1);
        }
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
//        Debug.Log("The score is " + score + " points");
    }

    // Scoring
    // Adds tileSystem to List
    public void AddTileSystem(TileSystem tileSystemToAdd)
    {
        tileSystemList.Add(tileSystemToAdd);
    }

}
