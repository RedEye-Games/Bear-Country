using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileController : MonoBehaviour
{

    // GameController
    private GameController gameController;

    // Tile Colliders
    public GameObject westPath;
    public GameObject eastPath;
    public GameObject northPath;
    public GameObject southPath;

    // Path Components
    public List<GameObject> pathList = new List<GameObject>();

    // Scoring Variables
    private int scoreToAdd;

    // Tile Buttons
    Button rotateCWButton;
    Button rotateCCWButton;
    Button flipButton;
    public GameObject thisTile;

    // Placement Variables
    bool isArmed = false;
    bool isConfirmed = false;
    public bool isPlaced = false;
    Vector3 spawnPoint;

    // Special Tile Variables
    public bool isSpecial = false;

    // Mouse Drag Variables
    private Vector3 screenPoint;
    private Vector3 offset;
    public float distance = 100;

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

        // Record spawn point
        spawnPoint = gameObject.transform.position;

        // Populate Paths
        pathList.Add(westPath);
        pathList.Add(eastPath);
        pathList.Add(southPath);
        pathList.Add(northPath);

        // Set Placement
        isPlaced = false;
    }

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        if (isPlaced && !isConfirmed && !isSpecial) 
        {
            gameController.GetComponent<TileDisbursementController>().UpdatePlaceCount(1);
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
            }
            isArmed = true;
            Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
            Vector3 tilePosition = cursorPosition;
            tilePosition.y = 5;
            transform.position = tilePosition;

            // Highlight Compatible Board Spaces
            CheckPotential();
        }
    }

    void OnMouseUp()
    {
        // Sends tiles back to spawn point
       
        if (isConfirmed == false)
        {
            if (isArmed && !isPlaced)
            {
                isArmed = false;
                RaycastHit boardCheck;
                Ray ray = new Ray(transform.position, -transform.up);
                if (Physics.Raycast(ray, out boardCheck, Mathf.Infinity))
                {
                    if (boardCheck.collider.tag == "BoardSpace")
                    {
                        if (boardCheck.collider.GetComponentInParent<BoardSpace>().isHighlighted)
                        {
                            isPlaced = true;
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
                        }
                        else 
                        {
                            ResetToSpawn();
                        }
                    }
                    else
                    {
                        ResetToSpawn();
                    }
                }
                else
                {
                    ResetToSpawn();
                }
            }
        }

        ClearPotential();
    }

    public void ConfirmTile()
    {
        isConfirmed = true;
        ScoreTile();
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

    }

    public void CheckPotential()
    {
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
        }

    }

    public void ClearPotential()
    {
        foreach (var boardSpace in gameController.boardSpaceList)
        {
            boardSpace.GetComponent<BoardSpace>().ClearPotential();
        }
    }

    public void UpdateScore(int score) 
    {

    }

    public void ScoreTile() 
    {
        foreach (GameObject path in pathList)
        {
            // Add up score.
            scoreToAdd = path.GetComponent<PathController>().scoreToAdd;
            gameController.AddScore(scoreToAdd);
            Debug.Log(path.name + " has a score of " + scoreToAdd);
        }
    }
}