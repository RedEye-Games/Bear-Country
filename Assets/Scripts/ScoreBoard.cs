using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public int systems = 0;
    public int longestTrail = 0;
    public int longestRiver = 0;
    public int bonusSpaces = 0;
    public int deadEnds = 0;

    // GameController
    private GameController gameController;

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
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] tileSystems = GameObject.FindGameObjectsWithTag("TileSystem");
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        // ToDo: Remove from Update
        CountSystems(tileSystems);
    }

    public void ScoreTiles()
    {
        GameObject[] tileSystems = GameObject.FindGameObjectsWithTag("TileSystem");
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        CountSystems(tileSystems);
        CalculateDeadEnds(tiles);
    }

    private void CountSystems(GameObject[] tileSystems)
    {
        systems = 0;
        foreach (var tileSystem in tileSystems)
        {
            TileSystem tileSystemComponent = tileSystem.GetComponent<TileSystem>();
            if (tileSystemComponent.systemType == null && tileSystemComponent.containedTiles.Count() > 1)
            {
                systems++;
            }
            else
            {
                int systemCount = tileSystemComponent.containedTiles.Count();
                string systemType = tileSystemComponent.systemType;
                if (tileSystemComponent.connectedExits.Any())
                {
                    if (systemType == "River")
                    {
                        if (systemCount > longestRiver)
                        {
                            longestRiver = systemCount;
                        }
                    }
                    else if (systemType == "Trail")
                    {
                        if (systemCount > longestTrail)
                        {
                            longestTrail = systemCount;
                        }
                    }
                }
            }
        }
    }

    private void CalculateDeadEnds(GameObject[] tiles)
    {
        deadEnds = 0;
        int deadEndDoubles = 0;
        // Count Dead Ends
        foreach (var tile in tiles)
        {
            List<GameObject> tilePaths = tile.GetComponent<TileController>().pathList;
            foreach (var path in tilePaths)
            {
                if (path.GetComponent<PathController>().isDeadEnd && tile.GetComponent<TileController>().isPlaced && path.tag != "Path")
                {
                    deadEnds++;

                    // Determine which dead ends are overlapping
                    if (path.GetComponent<PathController>().isDoubledDeadEnd)
                    {
                        deadEndDoubles++;
                    }
                }
            }
        }
        deadEnds = deadEnds - (deadEndDoubles / 2);

    }

}
