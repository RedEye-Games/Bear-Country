using System.Linq;
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

    // tileDisbursementController
    private TileDisbursementController tileDisbursementController;

    private int score;

    // Scoring Systems
    public List<TileSystem> tileSystemList;
    public GameObject tileSystem;
    readonly string tileSystemNamingString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private int tileSystemNamingInt = 0;

    // Placement Helpers
    public List<GameObject> tilesPlacedThisRound;

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

        // Locate TileDisbursementController Script
        tileDisbursementController = gameObject.GetComponent<TileDisbursementController>();

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
                        PopulateTileSystems(newBoardEdge.transform.GetChild(0).gameObject, true);
                        
                    }
                    else if (x == 4)
                    {
                        SpriteRenderer tileSprite = newBoardEdge.GetComponentInChildren<SpriteRenderer>();
                        tileSprite.sprite = Resources.Load<Sprite>("Sprites/rStraight");
                        newBoardEdge.GetComponentInChildren<TileController>().northPath.gameObject.tag = "River";
                        newBoardEdge.GetComponentInChildren<TileController>().southPath.gameObject.tag = "River";
                        PopulateTileSystems(newBoardEdge.transform.GetChild(0).gameObject, true);
                    }
                    else if (y == 2 || y == 6)
                    {
                        SpriteRenderer tileSprite = newBoardEdge.GetComponentInChildren<SpriteRenderer>();
                        tileSprite.sprite = Resources.Load<Sprite>("Sprites/rStraight");
                        newBoardEdge.GetComponentInChildren<TileController>().northPath.gameObject.tag = "River";
                        newBoardEdge.GetComponentInChildren<TileController>().southPath.gameObject.tag = "River";
                        newBoardEdge.transform.Rotate(Vector3.up * 90);
                        PopulateTileSystems(newBoardEdge.transform.GetChild(0).gameObject, true);
                    }
                    else if (y == 4)
                    {
                        SpriteRenderer tileSprite = newBoardEdge.GetComponentInChildren<SpriteRenderer>();
                        tileSprite.sprite = Resources.Load<Sprite>("Sprites/tStraight"); 
                        newBoardEdge.GetComponentInChildren<TileController>().northPath.gameObject.tag = "Trail";
                        newBoardEdge.GetComponentInChildren<TileController>().southPath.gameObject.tag = "Trail";
                        newBoardEdge.transform.Rotate(Vector3.up * 90);
                        PopulateTileSystems(newBoardEdge.transform.GetChild(0).gameObject, true);
                    }
                    else
                    {
                        SpriteRenderer tileSprite = newBoardEdge.GetComponentInChildren<SpriteRenderer>();
                        tileSprite.sprite = Resources.Load<Sprite>("Sprites/neutral");
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
        RemoveMissingObjects();
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

    public void AttachTileSystem(GameObject tile, string systemType = "Tile", bool isEdge = false)
    {
        tile.GetComponentInChildren<TileController>().isExit |= isEdge;
        GameObject newTileSystem = Instantiate(tileSystem);
        string newTileSystemName = GenerateTileName();
        newTileSystem.name = newTileSystemName;
        newTileSystem.GetComponent<TileSystem>().systemType = systemType;
        newTileSystem.GetComponent<TileSystem>().tileSystemName = newTileSystemName;
        newTileSystem.GetComponent<TileSystem>().AddToSystem(tile.GetComponentInChildren<TileController>().gameObject);
        tile.GetComponent<TileController>().tileSystemList.Add(newTileSystem);
    }

    private string GenerateTileName()
    {
        string newTileName = "";
        for (int i = 0; i < 5; i++)
        {
            char newChar = tileSystemNamingString[tileSystemNamingInt];
            newTileName = newTileName + newChar;
            tileSystemNamingInt = Random.Range(0, tileSystemNamingString.Length);
        }
        return "Tile System (" + newTileName + ")";
    }

    public void PopulateTileSystems(GameObject tile, bool isEdge = false)
    {
        // Populate Paths
        tile.GetComponent<TileController>().PopulatePaths();

        // Add a generic tile system
        AttachTileSystem(tile, default, isEdge);

        // Add a unique system for each path type contained in the tile
        List<string> SystemList = new List<string>();
        foreach (GameObject path in tile.GetComponent<TileController>().pathList)
        {
            if (path.tag != "Path")
            {
                if (!SystemList.Contains<string>(path.tag))
                {
                    SystemList.Add(path.tag);
                    AttachTileSystem(tile, path.tag);
                }
            }
        }
    }

    public void EndRound() 
    {
        tilesPlacedThisRound.Clear();
        RemoveMissingObjects();
    }

    public void RemoveMissingObjects()
    {
        GameObject[] allTiles = GameObject.FindGameObjectsWithTag("Tile");
        GameObject[] allBoardEdges = GameObject.FindGameObjectsWithTag("BoardEdge");
        allTiles = allTiles.Concat(allBoardEdges).ToArray();
        foreach (var tile in allTiles)
        {
            List<GameObject> tileSystems = tile.GetComponent<TileController>().tileSystemList;
            for (var i = tileSystems.Count() - 1; i > -1; i--)
            {
                if (tileSystems[i] == null)
                {
                    tileSystems.RemoveAt(i);
                }
            }
        }

    }

}
