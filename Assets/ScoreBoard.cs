using System.Collections;
using System.Collections.Generic;
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
        
    }

    public void ScoreTiles()
    {
        GameObject[] tileSystems = GameObject.FindGameObjectsWithTag("TileSystem");

        foreach (var tileSystem in tileSystems)
        {
            Dictionary<string, int> tileSystemLengths = new Dictionary<string, int>();
            List<GameObject> tileList = tileSystem.GetComponent<TileSystem>().containedTiles;
            if (tileList.Count > 1)
            {
                foreach (var tile in tileList)
                {
                    List<string> addTo = new List<string>();
                    foreach (var path in tile.GetComponent<TileController>().pathList)
                    {
                        if (!path.GetComponent<PathController>().isDeadEnd && !addTo.Contains(path.tag))
                        {
                            addTo.Add(path.tag);
                        }
                    }

                    foreach (var pathTag in addTo)
                    {
                        if (tileSystemLengths.ContainsKey(pathTag))
                        {
                            tileSystemLengths[pathTag] = tileSystemLengths[pathTag] + 1;
                        }
                        else
                        {
                            tileSystemLengths.Add(pathTag, 1);
                        }
                    }
                }
            }

            // To Do: Abstract this for any number of key:value pairs
            if (tileSystemLengths.ContainsKey("River"))
            {
                if (tileSystemLengths["River"] > longestRiver)
                {
                    longestRiver = tileSystemLengths["River"];
                }
            }
            if (tileSystemLengths.ContainsKey("Trail"))
            {
                if (tileSystemLengths["Trail"] > longestTrail)
                {
                    longestTrail = tileSystemLengths["Trail"];
                }
            }
        }
    }


}
