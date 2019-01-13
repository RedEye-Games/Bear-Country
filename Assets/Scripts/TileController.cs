using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileController : MonoBehaviour
{
    // GameController
    private GameController gameController;

    // Tile Buttons
    public Button flipButton;
    public Button rotateCWButton;
    public Button rotateCCWButton;
    public Button confirmButton;

    // Tile Colliders
    public GameObject westPath;
    public GameObject eastPath;
    public GameObject northPath;
    public GameObject southPath;

    public List<GameObject> pathList = new List<GameObject>();

    // how to find canvas so you can fade it on and off
    public GameObject canvas;
    CanvasGroup theGroup;

    // Placement Vars
    bool isArmed = false;
    public bool isPlaced;
    bool isConfirmed = false;
    Vector3 theSquarePosition;

    // Mouse Drag vars
    private Vector3 screenPoint;
    private Vector3 offset;

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

        //Announcing the buttons
        flipButton.onClick.AddListener(Flip);
        rotateCWButton.onClick.AddListener(RotateCW);
        rotateCCWButton.onClick.AddListener(RotateCCW);
        confirmButton.onClick.AddListener(Confirm);

        // Canvas Group vars
        theGroup = canvas.GetComponent<CanvasGroup>();
        theGroup.alpha = 0;
        theGroup.interactable = false;

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
        isPlaced = false;
    }

    void OnMouseDrag()
    {
        if (isConfirmed == false)
        {
            ControlsDisable();
            Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
            transform.parent.gameObject.transform.position = cursorPosition;
        }
    }

    private void OnMouseUp()
    {
        if (isArmed)
        {
            transform.parent.gameObject.transform.position = new Vector3(theSquarePosition.x, transform.position.y, theSquarePosition.z);
            ControlsEnable();
            isArmed = false;
            isPlaced = true;

            foreach (GameObject path in pathList)
            {
                // Add up score.
                var scoreToAdd = path.GetComponent<PathController>().scoreToAdd;
                gameController.AddScore(scoreToAdd);
            }
        }
    }

    // Tile Controls Enable
    private void ControlsEnable()
    {
        theGroup.alpha = 1;
        theGroup.interactable = true;
    }
    // Tile Controls Disable
    private void ControlsDisable()
    {
        theGroup.alpha = 0;
        theGroup.interactable = false;
    }

    // Button functions
    void Flip()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, -transform.localScale.z);
    }

    void RotateCW()
    {
        transform.Rotate(0, 90, 0);
    }

    void RotateCCW()
    {
        transform.Rotate(0, -90, 0);
    }

    void Confirm()
    {
        isConfirmed = true;
        ControlsDisable();
    }

    // Update is called once per frame
    void Update()
    {
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
   
}
