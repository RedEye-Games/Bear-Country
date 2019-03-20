using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    // GameController
    private GameController gameController;
    // TileModifiers
    private TileModifiers tileModifiers;

    private TileController parentTile;

    private string pathTag;
    public bool isDeadEnd = true;
    public bool isDoubledDeadEnd = false;
    public int scoreToAdd;
    public GameObject adjacentTile;
    public GameObject adjacentPath;
    List<string> validPathTags = new List<string>();
    public bool checkedDeadEnds;

    // Start is called before the first frame update
    void Start()
    {
        // Locate GameController Script
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
            validPathTags = gameController.validPathTags;
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }

        pathTag = gameObject.tag;

        // Locate Parent Tile Controller
        parentTile = gameObject.GetComponentInParent<TileController>();

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

    }

    // Update is called once per frame
    void Update()
    {
        if (parentTile.isPlaced)
        {
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        checkedDeadEnds = false;
        if (string.IsNullOrEmpty(other.gameObject.tag) == false && other.gameObject.tag != "Untagged" && other.gameObject.tag != "Path")
        {
            string tilePathTag = other.gameObject.tag;
            if (validPathTags.Contains(tilePathTag) == true)
            {
                adjacentPath = other.gameObject;
                adjacentTile = other.gameObject.transform.parent.gameObject;

                if (pathTag == adjacentPath.tag)
                {
                    isDeadEnd = false;
                    scoreToAdd = 1;
                }
                else if ("River" == adjacentPath.tag || "Trail" == adjacentPath.tag)
                {
                    isDoubledDeadEnd = true;
                }
            }
        }
        checkedDeadEnds = true;
    }

    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(BeginExit());
    }

    private IEnumerator BeginExit()
    {
        adjacentPath = null;
        adjacentTile = null;
        isDeadEnd = true;
        checkedDeadEnds = false;
        isDoubledDeadEnd = false;
        scoreToAdd = -1;
        if (parentTile.isLegal && parentTile.isPlaced && !tileModifiers.isRotating)
        {
            while (parentTile.checkingLegality)
            {
                Debug.Log("Did this a bunch.");
                yield return null;
            }
            Debug.Log("Got here.");
            parentTile.CheckLineage();
        }

    }

}
