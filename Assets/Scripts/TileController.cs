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
    bool isSelected = false;
    Vector3 theSquarePosition;
    Vector3 spawnPoint;

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

        // Find this tile's position
        theSquarePosition = gameObject.transform.position;
        // Record spawn point
        spawnPoint = gameObject.transform.position;

        // Locate Tile Adjuster Buttons
        flipButton = GameObject.Find("FlipButton").GetComponent<Button>();
        rotateCCWButton = GameObject.Find("RotateCCWButton").GetComponent<Button>();
        rotateCWButton = GameObject.Find("RotateCWButton").GetComponent<Button>();

        //Announcing the buttons
        flipButton.onClick.AddListener(Flip);
        rotateCWButton.onClick.AddListener(RotateCW);
        rotateCCWButton.onClick.AddListener(RotateCCW);

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
        if (isPlaced && !isConfirmed) 
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
            }
            isArmed = true;
            isPlaced = false;
            Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
            transform.parent.gameObject.transform.position = cursorPosition;
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
                    if (boardCheck.collider.name == "boardSpaceMain" || boardCheck.collider.name == "boardSpaceCollider01")
                    {
                        isPlaced = true;
                        transform.parent.gameObject.transform.position = new Vector3(theSquarePosition.x, transform.position.y, theSquarePosition.z);
                        gameController.GetComponent<TileDisbursementController>().UpdatePlaceCount(-1);
                    }
                    else
                    {
                        theSquarePosition = spawnPoint;
                        transform.parent.gameObject.transform.position = spawnPoint;
                    }
                }
                else
                {
                    theSquarePosition = spawnPoint;
                    transform.parent.gameObject.transform.position = spawnPoint;
                }
            }
        }
    }

    // Button functions
    void Flip()
    {
        if (isSelected && isPlaced)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, -transform.localScale.z);
        }
    }

    void RotateCW()
    {
        if (isSelected && isPlaced)
        {
            transform.Rotate(0, 90, 0);
        }
    }

    void RotateCCW()
    {
        if (isSelected && isPlaced)
        {
            transform.Rotate(0, -90, 0);
        }
    }

    public void ConfirmTile()
    {
        isConfirmed = true;
        ScoreTile();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject == thisTile)
                {
                    isSelected = true;
                }
                else
                {
                    isSelected = false;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BoardSpace"))
        {
            GameObject theSquare = other.gameObject;
            theSquarePosition = other.gameObject.transform.position;
            TileCollider tileCollider = theSquare.GetComponent<TileCollider>();
            if (tileCollider.isEmpty == true)
            {
                isArmed = true;
            }
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