using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildlifeController : MonoBehaviour
{

    public List<GameObject> tileSystemList;

    // GameController
    private GameController gameController;

    // TileDisbursementController
    private TileDisbursementController tileDisbursementController;

    // Checks
    private bool checkingSalmon;
    private bool checkingBears;
    private bool supportsBear;

    // Start is called before the first frame update
    void Start()
    {
        checkingSalmon = false;
        checkingBears = false;
        supportsBear = false;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdjustWildlife(GameObject tile)
    {
        StartCoroutine(BeginAdjustment(tile));
    }

    IEnumerator BeginAdjustment(GameObject tile)
    {
        // Wait until all systems are finished adjusting
        yield return new WaitUntil(() => !tileDisbursementController.disbursingTiles);
        foreach (var tileSystem in GameObject.FindGameObjectsWithTag("TileSystem"))
        {
            tileSystemList.Add(tileSystem);
        }

        checkingSalmon = true;
        CheckForSalmon(tile);
        yield return new WaitUntil(() => !checkingSalmon);
        tileSystemList.Clear();
    }

    void CheckForBears(GameObject tile)
    {
        // See if a tile is part of a trail system with at least one exit
        // If so, spawn bear

        // Get all tile systems
        foreach (var tileSystemVar in tileSystemList)
        {
            TileSystem tileSystem = tileSystemVar.GetComponent<TileSystem>();
            // Search for tile systems containing target tile
            if (tileSystem.containedTiles.Contains(tile))
            {
                if (tileSystem.systemType == "Trail")
                {
                    // Check to see if the tile already has a bear
                    if (!tile.GetComponent<TileController>().hasBear)
                    {
                        // Check to see if a trail system has at least one exit.
                        if (tileSystem.connectedExits.Count >= 1)
                        {
                            // If so, add a bear to tile
                            Debug.Log("Spawning Bear on tile " + tile.name);
                            SpawnBear(tile);
                        }
                    }
                }
            }
        }
        checkingBears = false;
    }

    void CheckForSalmon(GameObject tile)
    {
        // Get all tile systems
        foreach (var tileSystemVar in tileSystemList)
        {
            TileSystem tileSystem = tileSystemVar.GetComponent<TileSystem>();
            // Search for tile systems containing target tile
            if (tileSystem.containedTiles.Contains(tile))
            {
                if (tileSystem.systemType == "River")
                {
                    // Check to see if a river system already has salmon
                    if (!tileSystem.hasSalmon)
                    {
                        // Check to see if a river system has two exits.
                        if (tileSystem.connectedExits.Count > 1)
                        {
                            // If so, fill river with Salmon
                            SpawnSalmon(tileSystem);
                            CheckForBears(tile);
                        }
                    }
                    else
                    {
                        CheckForBears(tile);
                    }
                }
            }
        }
        checkingSalmon = false;
    }

    void SpawnSalmon(TileSystem tileSystem)
    {
        // Fill a river with Salmon
        tileSystem.hasSalmon = true;
        foreach (var tile in tileSystem.containedTiles)
        {
            //tile.GetComponentInChildren<SalmonController>().ShowSalmon();
        }
    }

    void SpawnBear(GameObject tile)
    {
        // Enable a bear on the tile.
        Debug.Log("Showing bear.");
        tile.GetComponentInChildren<BearController>().ShowBear();
    }
}
