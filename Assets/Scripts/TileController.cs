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
    public bool isFlipped = false;

    // Selection
    public bool isSelected;
    SpriteRenderer[] tileSprites;
    IEnumerator breathing;

    // Tile Lineage
    public bool lineageBeingChecked = false;
    public bool hasLineage;

    // Legality
    public bool isLegal = false;
    public bool checkingLegality;
    public string checkingLegalityDirection;

    // Special Tile Variables
    public bool isSpecial = false;

    // Drag Variables
    private Vector3 tilePositionStart;
    private Vector3 screenPoint;
    private Vector3 offset;
    public float distance = 125;
    private bool isLifted;
    private bool slowMovement;

    // Coroutine Setup
    public int frame;
    private IEnumerator coroutine;

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

        // Drag Variables
        isLifted = false;
        slowMovement = true;

        // Set Placement
        isPlaced = false;

        isSelected = false;

        tileSprites = gameObject.GetComponentsInChildren<SpriteRenderer>();

        // Setting Legality
        checkingLegality = false;
        checkingLegalityDirection = "CW";


    }

    // Update is called once per frame
    void Update()
    {
        frame++;
        if (isLifted)
        {
            if (slowMovement)
            {
                transform.position = Vector3.Lerp(transform.position, offset, 15 * Time.deltaTime);
            }
            else
            {
                transform.position = offset;
            }

        }
        if (isSelected)
        {
            tileSprites = gameObject.GetComponentsInChildren<SpriteRenderer>();
            foreach (var sprite in tileSprites)
            {
                if (sprite.gameObject.name == "PlacedTile")
                {
                    sprite.color = new Color(1, 1, 1, 0);
                }
                if (sprite.gameObject.name == "Highlight")
                {
                    sprite.color = new Color(1, 1, 1, 0.5f);
                }
            }
        }
        if (isPlaced && !isSelected && !isConfirmed)
        {
            tileSprites = gameObject.GetComponentsInChildren<SpriteRenderer>();
            foreach (var sprite in tileSprites)
            {
                if (sprite.gameObject.name == "PlacedTile")
                {
                    sprite.color = new Color(1, 1, 1, 1);
                }
            }
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
        if (!isConfirmed)
        {
            coroutine = BeginLift();
            StartCoroutine(coroutine);
        }
    }

    void OnMouseDrag()
    {
        if (isConfirmed == false && isLifted == true)
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
            offset = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y+distance, screenPoint.z + 145));
        }
    }

    void OnMouseUp()
    {
        isLifted = false;
        StartCoroutine(DropTile());
        tileDisbursementController.ToggleButtons();
        StopCoroutine(coroutine);
    }

    public IEnumerator BeginLift()
    {
        slowMovement = true;

        gameController.GetComponent<GameController>().SelectTile(gameObject);
        breathing = StartBreathing();
        StartCoroutine(breathing);
        TileEvent(TileEventName.PickedUp, gameObject);

        yield return new WaitForSeconds(.10f);       
         
        // Clear Legality
        ClearLegality();

        // Highlight Compatible Board Spaces
        StartCoroutine(CheckPotential(frame));

        isLifted = true;

        yield return new WaitForSeconds(.25f);
        slowMovement = false;

    }

    public IEnumerator StartBreathing()
    {


        // Track how many seconds we've been fading.
        int duration = 1;
        float t = 0;

        float startOpacity = 0.5f;
        float endOpacity = 1.0f;
        while (isSelected)
        {
            while (t < duration)
            {
                // Step the fade forward one frame.
                t += Time.deltaTime;
                // Turn the time into an interpolation factor between 0 and 1.
                float blend = Mathf.Clamp01(t / duration);
                foreach (var sprite in tileSprites)
                {
                    if (sprite.gameObject.name == "Highlight")
                    {
                        // Blend to the corresponding opacity between start & target.
                        sprite.color = new Color(1, 1, 1, Mathf.Lerp(startOpacity, endOpacity, blend));
                    }
                }

                // Wait one frame, and repeat.
                yield return null;
            }
            if (t >= duration)
            {
                t = 0;
                float swapOpacity = startOpacity;
                startOpacity = endOpacity;
                endOpacity = swapOpacity;
            }
        }

    }

    public void StopBreathing()
    {
        StopCoroutine(breathing);
        foreach (var sprite in tileSprites)
        {
            if (sprite.gameObject.name == "Highlight")
            {
                sprite.color = new Color(1, 1, 1, 0);
            }
        }
        isSelected = false;
    }

    // Explicit predicate delegate.
    private static bool FindLiveEnds(GameObject path)
    {

        if (path.GetComponent<PathController>().isDeadEnd == false)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public IEnumerator CheckLineage()
    {

        yield return new WaitUntil(() => !tileModifiers.isRotating);

        lineageBeingChecked = true;
        hasLineage = false;

        // First Determine if two adjacent tiles have only each other as living ends. If so, reset both to spawn.
        List<GameObject> livePaths = pathList.FindAll(FindLiveEnds);
        if (livePaths.Count == 1 && !isConfirmed)
        {
            List<GameObject> adjacentLivePaths = livePaths[0].GetComponent<PathController>().adjacentTile.GetComponentInParent<TileController>().pathList.FindAll(FindLiveEnds);
            if (adjacentLivePaths.Count == 1)
            {
                // Make sure other tile is not an edge.
                if (!adjacentLivePaths[0].GetComponentInParent<TileController>().isConfirmed)
                {
                    adjacentLivePaths[0].GetComponentInParent<TileController>().ResetToSpawn();
                    ResetToSpawn();
                }
            }
        }

        foreach (var path in pathList)
        {
            if (path.GetComponent<PathController>().adjacentTile && !path.GetComponent<PathController>().isDeadEnd)
            {
                TileController adjacentTileController = path.GetComponent<PathController>().adjacentTile.GetComponent<TileController>();
                if (adjacentTileController.isConfirmed)
                {
                    lineageBeingChecked = false;
                    hasLineage = true;
                }
                else
                {
                    if (!adjacentTileController.lineageBeingChecked)
                    {
                        adjacentTileController.lineageBeingChecked = true;
                        IEnumerator adjacentCheck = adjacentTileController.CheckLineage();
                        StartCoroutine(adjacentCheck);
                    }
                    yield return new WaitUntil(() => !adjacentTileController.lineageBeingChecked);
                    if (adjacentTileController.hasLineage)
                    {
                        hasLineage = true;
                    }
                }
            }
        }

        if (!hasLineage && isLegal)
        {
            gameObject.GetComponent<TileController>().ResetToSpawn();
        }
        lineageBeingChecked = false;
    }

    public IEnumerator TileLegality()
    {
        checkingLegality = true;
        while (tileModifiers.isRotating)
        {
            yield return null;
        }
        int startFrame = frame;
        yield return new WaitUntil(() => frame >= startFrame + 5);
        StartCoroutine(TileLegalityCheck());
    }

    public IEnumerator TileLegalityCheck()
    {
        if (legalCheck < 4)
        {
            int pathCount = 0;
            foreach (var path in pathList)
            {
                if (path.GetComponent<PathController>().isDeadEnd == false)
                {
                    isLegal = true;
                    legalCheck = 0;
                }
                else
                {
                    pathCount++;
                }
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
            }
            else
            {
                checkingLegality = false;
            }
        }
        else
        {
            ResetToSpawn();
            legalCheck = 0;
        }
        yield return null;
    }

    public void ClearLegality()
    {
        isLegal = false;
        legalCheck = 0;
    }

    public void ConfirmTile()
    {
        isConfirmed = true;
        isSelected = false;
        StopBreathing();
        foreach (var tileSprite in tileSprites)
        {
            if (tileSprite.gameObject.name == "Tile Sprite")
            {
                tileSprite.color = new Color(1, 1, 1, 1);
            }
            if (tileSprite.gameObject.name == "PlacedTile")
            {
                tileSprite.color = new Color(1, 1, 1, 0);
            }
        }
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
        hasLineage = false;
        lineageBeingChecked = false;
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
                            //gameController.GetComponent<GameController>().SelectTile(gameObject);
                            Vector3 tilePosition = boardCheck.collider.transform.position;
                            tilePosition.y = 0;
                            transform.position = tilePosition;
                            boardCheck.collider.GetComponent<BoardSpace>().isOccupied = true;

                            // Rotate Tile if Illegal
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