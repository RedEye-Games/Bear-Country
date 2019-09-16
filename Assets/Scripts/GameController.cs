using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    private GameData gameData;

    public GameObject boardSpace;
    public GameObject boardEdge;

    private float boardPosZ;

    public List<GameObject> boardSpaceList;
    public List<string> validPathTags = new List<string>();
    public float boardScale = 10;
    public int boardSize = 9;
    public GameObject selectedTile;

    // tileDisbursementController
    private TileDisbursementController tileDisbursementController;

    // ScoreBoard
    private ScoreBoard scoreBoard;
    public GameObject endGameOverlay;

    private int score;

    // Scoring Systems
    public List<GameObject> tileSystemList;
    public GameObject tileSystem;
    readonly string tileSystemNamingString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private int tileSystemNamingInt = 0;

    // Placement Helpers
    public List<GameObject> tilesPlacedThisRound;
    public List<GameObject> specialTilesPlacedThisRound;

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

        // Locate ScoreBoard Script
        // ToDo: Trigger to EventManager
        GameObject scoreBoardObject = GameObject.FindWithTag("ScoreBoard");
        if (scoreBoardObject != null)
        {
            scoreBoard = scoreBoardObject.GetComponent<ScoreBoard>();
        }
        if (scoreBoardObject == null)
        {
            Debug.Log("Cannot find 'ScoreBoard' script");
        }

        // Locate TileDisbursementController Script
        tileDisbursementController = gameObject.GetComponent<TileDisbursementController>();

        Vector3 BoardCenter = new Vector3(boardScale / 2, 0, boardScale / 2 + boardPosZ);
        GenerateBoard(boardSize, boardSize, BoardCenter);
        score = 0;

        GameSettings gameSettings = new GameSettings();
        gameData = new GameData( gameSettings, DataHolder.sharedString );
        gameData.Begin( GameData.LaunchedFromValue.main_menu );

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
                        newBoardEdge.GetComponentInChildren<TileController>().isConfirmed = true;
                        PopulateTileSystems(newBoardEdge.transform.GetChild(0).gameObject, true);
                        
                    }
                    else if (x == 4)
                    {
                        SpriteRenderer tileSprite = newBoardEdge.GetComponentInChildren<SpriteRenderer>();
                        tileSprite.sprite = Resources.Load<Sprite>("Sprites/rStraight");
                        newBoardEdge.GetComponentInChildren<TileController>().northPath.gameObject.tag = "River";
                        newBoardEdge.GetComponentInChildren<TileController>().southPath.gameObject.tag = "River";
                        newBoardEdge.GetComponentInChildren<TileController>().isConfirmed = true;
                        PopulateTileSystems(newBoardEdge.transform.GetChild(0).gameObject, true);
                    }
                    else if (y == 2 || y == 6)
                    {
                        SpriteRenderer tileSprite = newBoardEdge.GetComponentInChildren<SpriteRenderer>();
                        tileSprite.sprite = Resources.Load<Sprite>("Sprites/rStraight");
                        newBoardEdge.GetComponentInChildren<TileController>().northPath.gameObject.tag = "River";
                        newBoardEdge.GetComponentInChildren<TileController>().southPath.gameObject.tag = "River";
                        newBoardEdge.GetComponentInChildren<TileController>().isConfirmed = true;
                        newBoardEdge.transform.Rotate(Vector3.up * 90);
                        PopulateTileSystems(newBoardEdge.transform.GetChild(0).gameObject, true);
                    }
                    else if (y == 4)
                    {
                        SpriteRenderer tileSprite = newBoardEdge.GetComponentInChildren<SpriteRenderer>();
                        tileSprite.sprite = Resources.Load<Sprite>("Sprites/tStraight"); 
                        newBoardEdge.GetComponentInChildren<TileController>().northPath.gameObject.tag = "Trail";
                        newBoardEdge.GetComponentInChildren<TileController>().southPath.gameObject.tag = "Trail";
                        newBoardEdge.GetComponentInChildren<TileController>().isConfirmed = true;
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
        // Debug.Log("The score is " + score + " points");
    }

    // Scoring
    // Adds tileSystem to List
    public void AddTileSystem(TileSystem tileSystemToAdd)
    {
        //tileSystemList.Add(tileSystemToAdd);
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

    public void SelectTile(GameObject tile)
    {
        if (selectedTile != null)
        {
            selectedTile.GetComponent<TileController>().StopBreathing();
        }
        selectedTile = tile;
        selectedTile.GetComponent<TileController>().isSelected = true;
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
        specialTilesPlacedThisRound.Clear();
        RemoveMissingObjects();

        gameData.GoToNextRound();
        
        GameObject[] allTiles = GameObject.FindGameObjectsWithTag("Tile");
        tileSystemList.Clear();
        // Wildlife Management
        foreach (var tile in allTiles)
        {
            GetComponentInParent<WildlifeController>().AdjustWildlife(tile);
        }
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

    public IEnumerator EndGame()
    {
        scoreBoard.GetComponent<ScoreBoard>().ScoreTiles();
        yield return new WaitUntil(() => !scoreBoard.isScoring);
        int highScore = scoreBoard.GetComponent<ScoreBoard>().totalScore;

        // ToDo: this should probably happen somewhere else instead of here
        gameData.SetScore(highScore);

        Debug.Log("Ended game. Saving.");
        scoreBoard.GetComponent<HighScoreSaver>().SaveScore(highScore);
        yield return new WaitUntil(() => !scoreBoard.GetComponent<HighScoreSaver>().isSaving);
        HighScoreData highScores = scoreBoard.GetComponent<HighScoreSaver>().highScoreData;
        endGameOverlay.SetActive(true);
        endGameOverlay.GetComponentInChildren<HighScoreController>().UpdateScores(highScores);

        AnalyticsWrapper.Report.GameFinish(gameData);
    }
}
