using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSystem : MonoBehaviour
{
    public char tileSystemName;
    public List<GameObject> containedTiles;
    public List<GameObject> connectedExits;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MergeSystem(GameObject mergingSystem)
    {

        foreach (var tile in mergingSystem.GetComponent<TileSystem>().containedTiles)
        {
            if (!containedTiles.Contains(tile))
            {
                AddToSystem(tile);
            }
        }
        if (gameObject != mergingSystem)
        {
            Destroy(mergingSystem);
            Debug.Log("Destroying.");
        }
        else
        {
            Debug.Log("Did not destroy " + mergingSystem.name);
        }
    }

    public void AddToSystem(GameObject tileToAdd)
    {
        containedTiles.Add(tileToAdd);
        tileToAdd.GetComponent<TileController>().tileSystem = gameObject;
        if (tileToAdd.GetComponent<TileController>().isExit)
        {
            connectedExits.Add(tileToAdd);
        }
    }
}
