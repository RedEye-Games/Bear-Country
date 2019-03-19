using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    // GameController
    private GameController gameController;

    private TileController parentTile;

    private string pathTag;
    public bool isDeadEnd = true;
    public bool isDoubledDeadEnd = false;
    public int scoreToAdd;
    public GameObject adjacentTile;
    public GameObject adjacentPath;
    List<string> validPathTags = new List<string>();

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
    }

private void OnTriggerExit(Collider other)
    {
        adjacentPath = null;
        adjacentTile = null;
        isDeadEnd = true;
        isDoubledDeadEnd = false;
        scoreToAdd = -1;
        if (parentTile.isLegal && parentTile.isPlaced && !parentTile.checkingLegality)
        {
            parentTile.CheckLineage();
        }
    }

}
