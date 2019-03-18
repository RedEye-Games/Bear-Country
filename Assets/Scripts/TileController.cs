using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum TileEventName
{
    PickedUp,
    SuccessfullyPlaced,
    UnsuccessfullyPlaced,
    Rotated,
    Flipped
}

public class TileController : MonoBehaviour 
{ 

    public static event Action<TileEventName, GameObject> TileEvent;

    // GameController
    private GameController gameController;

    // TileModifiers
    private TileModifiers tileModifiers;

    // Tile Colliders
    public GameObject westPath;
    public GameObject eastPath;
    public GameObject northPath;
    public GameObject southPath;

    // Path Components
    public List<GameObject> pathList = new List<GameObject>();

    // Scoring Variables
    private int scoreToAdd;
    public List<GameObject> tileSystemList = new List<GameObject>();
    public bool isExit = false;

    // Tile Buttons
    Button rotateCWButton;
    Button rotateCCWButton;
    Button flipButton;
    public GameObject thisTile;

    // Placement Variables
    bool isArmed = false;
    bool isConfirmed = false;
    int legalCheck = 0;
    public bool isPlaced = false;
    Vector3 spawnPoint;
    bool checkingPotential = false;

    // Legality
    bool isLegal = false;
    public bool checkingLegality;
    public string checkingLegalityDirection;

    // Special Tile Variables
    public bool isSpecial = false;

    // Mouse Drag Variables
    private Vector3 screenPoint;
    private Vector3 offset;
    public float distance = 100;

    // Coroutine Setup
    public int frame;

    // Start is called before the first frame update
    void Start()
    {
        // Locate GameController Script
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }

        // Locate TileModifiers Script
        GameObject TileModifiersObject = GameObject.FindWithTag("TileModifiers");
        if (TileModifiersObject != null)
        {
            tileModifiers = TileModifiersObject.GetComponent<TileModifiers>();
        }
        if (tileModifiers == null)
        {
            Debug.Log("Cannot find 'TileModifiers' script");
        }

        // Record spawn point
        spawnPoint = gameObject.transform.position;



        // Set Placement
        isPlaced = false;

        // Setting Legality
        checkingLegality = false;
        checkingLegalityDirection = "CW";
    }

    public void PopulatePaths()
    { 
        pathList.Add(westPath);
        pathList.Add(eastPath);
        pathList.Add(southPath);
        pathList.Add(northPath);
    }

    void OnMouseDown()
    {
        // Check to make sure it's not attached to only a tile placed this round
        // ... to do

        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        if (isPlaced && !isConfirmed && !isSpecial) 
        {
            gameController.GetComponent<TileDisbursementController>().UpdatePlaceCount(1);
        }
        if (!isConfirmed)
        {
            // Highlight Compatible Board Spaces
            checkingPotential = true;
            StartCoroutine(CheckPotential(frame));

            TileEvent(TileEventName.PickedUp, gameObject);

            // Clear Legality
            ClearLegality();
        }
    }

    void OnMouseDrag()
    {
        if (isConfirmed == false)
        {
            if (isPlaced)
            {
                gameController.AddScore(4);
                isPlaced = false;
                gameController.GetComponent<GameController>().tilesPlacedThisRound.Remove(gameObject);
            }
            isArmed = true;
            Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
            Vector3 tilePosition = cursorPosition;
            tilePosition.y = 5;
            transform.position = tilePosition;

        }
    }

    void OnMouseUp()
    {
        // Sends tiles back to spawn point
        checkingPotential = false;

        if (isConfirmed == false)
        {
            if (isArmed && !isPlaced)
            {
                isArmed = false;

                Ray ray = new Ray(transform.position, -transform.up);
                if (Physics.Raycast(ray, out RaycastHit boardCheck, Mathf.Infinity))
                {
                    if (boardCheck.collider.tag == "BoardSpace")
                    {
                        if (boardCheck.collider.GetComponentInParent<BoardSpace>().isHighlighted)
                        {
                            // All Checks Passed. Place the Tile
                            isPlaced = true;
                            gameController.GetComponent<GameController>().tilesPlacedThisRound.Add(gameObject);
                            gameController.GetComponent<GameController>().selectedTile = gameObject;
                            Vector3 tilePosition = boardCheck.collider.transform.position;
                            tilePosition.y = 0;
                            transform.position = tilePosition;
                            boardCheck.collider.GetComponent<BoardSpace>().isOccupied = true;

                            if (isSpecial)
                            {
                                gameController.GetComponent<TileDisbursementController>().UpdatePlaceCount(-1, true);
                            }
                            else
                            {
                                gameController.GetComponent<TileDisbursementController>().UpdatePlaceCount(-1);
                            }

                            // Rotate Tile if Illegal
                            checkingLegality = true;
                            StartCoroutine(TileLegality(frame));
                            TileEvent(TileEventName.SuccessfullyPlaced, gameObject);
                        }
                        else 
                        {
                            TileEvent(TileEventName.UnsuccessfullyPlaced, gameObject);
                            ResetToSpawn();
                        }
                    }
                    else
                    {
                        TileEvent(TileEventName.UnsuccessfullyPlaced, gameObject);
                        ResetToSpawn();
                    }
                }
                else
                {
                    TileEvent(TileEventName.UnsuccessfullyPlaced, gameObject);
                    ResetToSpawn();
                }
            }
        }

        ClearPotential();
    }

    public IEnumerator TileLegality(int startFrame)
    {
        yield return new WaitUntil(() => frame >= startFrame + 5);
        TileLegalityCheck();
    }

    public void TileLegalityCheck()
    {
        if (legalCheck < 4)
        {
            foreach (var path in pathList)
            {
                if (path.GetComponent<PathController>().isDeadEnd == false)
                {
                    isLegal = true;
                    legalCheck = 0;
                }
            }
            if (!isLegal)
            {
                if (checkingLegalityDirection == "CW")
                {
                    tileModifiers.RotateCW();
                }
                else 
                {
                    tileModifiers.RotateCCW();
                }

                legalCheck++;
                StartCoroutine(TileLegality(frame));
            }
        }
        else
        {
            ResetToSpawn();
            legalCheck = 0;
            Debug.Log("No legal moves.");
            // And perhaps end round?
        }
    }

    public void ClearLegality()
    {
        foreach (var path in pathList)
        {
            isLegal = false;
        }
        legalCheck = 0;
    }

    public void ConfirmTile()
    {
        isConfirmed = true;
        // Connect Systems for Scoring
        ScoreTile();
    }

    public void ScoreTile()
    {
        List<GameObject> flaggedForDeletion = new List<GameObject>();
        List<GameObject> flaggedForAddition = new List<GameObject>();
        foreach (var path in pathList)
        {
            if (!path.GetComponent<PathController>().isDeadEnd)
            {
                List<GameObject> adjacentTileSystems = path.GetComponent<PathController>().adjacentTile.GetComponent<TileController>().tileSystemList;
                if (adjacentTileSystems != null)
                {
                    foreach (var system in adjacentTileSystems)
                    {
                        if (system != null)
                        {
                            if (path.tag == system.GetComponent<TileSystem>().systemType)
                            {
                                foreach (var internalSystem in tileSystemList)
                                {
                                    if (internalSystem.GetComponent<TileSystem>().systemType == path.tag)
                                    {
                                        internalSystem.GetComponent<TileSystem>().MergeSystem(system);
                                        flaggedForDeletion.Add(system);
                                    }
                                }
                            }
                            else if (system.GetComponent<TileSystem>().systemType == null)
                            {
                                // ToDo: DRY this up.
                                foreach (var internalSystem in tileSystemList)
                                {
                                    if (internalSystem.GetComponent<TileSystem>().systemType == null)
                                    {
                                        internalSystem.GetComponent<TileSystem>().MergeSystem(system);
                                        flaggedForDeletion.Add(system);
                                    }
                                }
                            }
                        }
                    }
                }

            }
        }
        foreach (var system in flaggedForDeletion)
        {
            if (!tileSystemList.Contains(system))
            {
                Destroy(system);
            }
        }
        foreach (var system in tileSystemList)
        {
            foreach (var tile in system.GetComponent<TileSystem>().containedTiles)
            {
                if (!tile.GetComponent<TileController>().tileSystemList.Contains(system))
                {
                    tile.GetComponent<TileController>().tileSystemList.Add(system);
                }
            }
        }
    }

    private void ResetToSpawn()
    {
        if (isSpecial)
        {
            gameController.GetComponent<TileDisbursementController>().UpdatePlaceCount(1, true);
        }

        transform.position = spawnPoint;
    }

    // Update is called once per frame
    void Update()
    {
        frame++;
        if (checkingLegality)
        {
            checkingLegality = false;
            StartCoroutine(TileLegality(frame));
        }
    }

    IEnumerator CheckPotential(int startFrame)
    {
        yield return new WaitUntil(() => frame >= startFrame + 5);
        foreach (var boardSpace in gameController.boardSpaceList)
        {
            BoardSpace boardSpaceToBeChecked = boardSpace.GetComponent<BoardSpace>();
            foreach (GameObject path in pathList)
            {
                if (path.tag != "Path")
                {
                    boardSpaceToBeChecked.CheckPotential(path.tag);
                }
            }
            if (boardSpaceToBeChecked.hasPotential)
            {
                if (checkingPotential)
                {
                    boardSpaceToBeChecked.Highlight(true);
                }
            }
        }
    }

    public void ClearPotential()
    {
        checkingPotential = false;
        foreach (var boardSpace in gameController.boardSpaceList)
        {
            boardSpace.GetComponent<BoardSpace>().ClearPotential();
        }
    }

    public void UpdateScore(int score) 
    {

    }

    public void AddToTileSystem() 
    {
        foreach (GameObject path in pathList)
        {
            if (!path.GetComponent<PathController>().isDeadEnd)
            {
                //string tileSystem = path.transform.parent.GetComponent<TileController>().tileSystem;
            }
        }
    }
}