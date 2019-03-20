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
    private TileDisbursementController tileDisbursementController;
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
    public bool isConfirmed = false;
    int legalCheck = 0;
    public bool isPlaced = false;
    Vector3 spawnPoint;
    bool checkingPotential = false;

    // Tile Lineage
    public bool lineageBeingChecked = false;

    // Legality
    public bool isLegal = false;
    public bool checkingLegality;
    public string checkingLegalityDirection;

    // Special Tile Variables
    public bool isSpecial = false;

    // Mouse Drag Variables
    private Vector3 tilePositionStart;
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

        // Locate TileDisbursementController Script
        tileDisbursementController = gameControllerObject.GetComponent<TileDisbursementController>();

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

    // Update is called once per frame
    void Update()
    {
        frame++;
        if (checkingLegality)
        {
            checkingLegality = false;
            StartCoroutine(TileLegality());
        }

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
        tilePositionStart = gameObject.transform.position;
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y - 50, screenPoint.z));

        if (!isConfirmed)
        {
            // Highlight Compatible Board Spaces
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
                isPlaced = false;
                if (isSpecial)
                {
                    gameController.GetComponent<GameController>().specialTilesPlacedThisRound.Remove(gameObject);
                }
                else
                {
                    gameController.GetComponent<GameController>().tilesPlacedThisRound.Remove(gameObject);
                }
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
        StartCoroutine(DropTile());
        tileDisbursementController.ToggleButtons();
    }

    public void CheckLineage()
    {
        if (!gameObject.GetComponent<TileController>().HasLineage())
        {
            gameObject.GetComponent<TileController>().ResetToSpawn();
        }
    }

    // Check to make sure tile connects to confirmed tile
    public bool HasLineage()
    {
        lineageBeingChecked = true;
        bool hasLineage = false;
        foreach (var path in pathList)
        {
            if (path.GetComponent<PathController>().adjacentTile && !path.GetComponent<PathController>().isDeadEnd)
            {
                TileController adjacentTileController = path.GetComponent<PathController>().adjacentTile.GetComponent<TileController>();
                if (adjacentTileController.isConfirmed)
                {
                    lineageBeingChecked = false;
                    return true;
                }
                else
                {
                    if (!adjacentTileController.lineageBeingChecked)
                    {
                        hasLineage = adjacentTileController.HasLineage();
                    }
                }
            }
        }
        return hasLineage;
    }

    public IEnumerator TileLegality()
    {
        while (tileModifiers.rotating)
        {
            yield return null;
        }
        int startFrame = frame;
        //yield return new WaitUntil(() => frame >= startFrame + 1);
        StartCoroutine(TileLegalityCheck());
    }

    public IEnumerator TileLegalityCheck()
    {
        if (legalCheck < 4)
        {
            int pathCount = 0;
            foreach (var path in pathList)
            {
                while (path.GetComponent<PathController>().checkedDeadEnds == false)
                {
                    yield return null;
                }
                if (path.GetComponent<PathController>().isDeadEnd == false)
                {
                    isLegal = true;
                    legalCheck = 0;
                }
                else
                {
                    pathCount++;
                    Debug.Log(isLegal);
                }
            }
            while (pathCount < pathList.Count)
            {
                yield return null;
            }
            if (!isLegal)
            {
                if (checkingLegalityDirection == "CW")
                {
                    StartCoroutine(tileModifiers.RotateCW());
                }
                else 
                {
                    StartCoroutine(tileModifiers.RotateCCW());
                }

                legalCheck++;
                //StartCoroutine(TileLegality());
            }
        }
        else
        {
            ResetToSpawn();
            legalCheck = 0;
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
            gameController.GetComponent<GameController>().specialTilesPlacedThisRound.Remove(gameObject);
        }
        else
        {
            gameController.GetComponent<GameController>().tilesPlacedThisRound.Remove(gameObject);
        }
        isPlaced = false;
        transform.position = spawnPoint;
    }

    IEnumerator CheckPotential(int startFrame)
    {
        checkingPotential = true;
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
        checkingPotential = false;
    }

    private IEnumerator DropTile()
    {
        while (checkingPotential)
        {
            yield return null;
        }

        // Sends tiles back to spawn point

        if (isConfirmed == false)
        {
            if (isArmed && !isPlaced)
            {
                isArmed = false;

                Ray ray = new Ray(transform.position, Vector3.down);
                if (Physics.Raycast(ray, out RaycastHit boardCheck, Mathf.Infinity))
                {
                    if (boardCheck.collider.tag == "BoardSpace")
                    {
                        if (boardCheck.collider.GetComponentInParent<BoardSpace>().isHighlighted)
                        {
                            // All Checks Passed. Place the Tile
                            isPlaced = true;
                            if (isSpecial)
                            {
                                gameController.GetComponent<GameController>().specialTilesPlacedThisRound.Add(gameObject);
                            }
                            else
                            {
                                gameController.GetComponent<GameController>().tilesPlacedThisRound.Add(gameObject);
                            }
                            gameController.GetComponent<GameController>().selectedTile = gameObject;
                            Vector3 tilePosition = boardCheck.collider.transform.position;
                            tilePosition.y = 0;
                            transform.position = tilePosition;
                            boardCheck.collider.GetComponent<BoardSpace>().isOccupied = true;

                            // Rotate Tile if Illegal
                            checkingLegality = true;
                            StartCoroutine(TileLegality());
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

    public void ClearPotential()
    {
        checkingPotential = false;
        foreach (var boardSpace in gameController.boardSpaceList)
        {
            boardSpace.GetComponent<BoardSpace>().ClearPotential();
        }
    }

}