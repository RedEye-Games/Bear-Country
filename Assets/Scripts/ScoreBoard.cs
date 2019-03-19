using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    private int systems = 0;
    public int exitPoints = 0;
    public int longestTrail = 0;
    public int longestRiver = 0;
    private int bonusSpaces = 0;
    private int deadEnds = 0;
    readonly int[] exitScoreTable = new int[] {0, 0, 4, 8, 12, 16, 20, 24, 28, 32, 36, 40, 45};
    public int totalScore = 0;

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
        CountTotalScore();
    }

    public void ScoreTiles()
    {
        GameObject[] tileSystems = GameObject.FindGameObjectsWithTag("TileSystem");
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        CountSystems(tileSystems);
        CountDeadEnds(tiles);
    }

    private void CountSystems(GameObject[] tileSystems)
    {
        systems = 0;
        exitPoints = 0;
        foreach (var tileSystem in tileSystems)
        {
            TileSystem tileSystemComponent = tileSystem.GetComponent<TileSystem>();
            if (tileSystemComponent.systemType == null && tileSystemComponent.containedTiles.Count() > 1)
            {
                exitPoints = exitPoints + exitScoreTable[tileSystemComponent.connectedExits.Count()];
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

    private void CountDeadEnds(GameObject[] tiles)
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

    private void CountTotalScore()
    {
        totalScore = exitPoints + longestRiver + longestTrail - deadEnds;
    }

}
