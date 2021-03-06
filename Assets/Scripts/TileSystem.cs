﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSystem : MonoBehaviour
{
    public string tileSystemName;
    public List<GameObject> containedTiles;
    public List<GameObject> connectedExits;
    public string systemType;

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
        if (mergingSystem.GetComponent<TileSystem>().systemType == systemType)
        {
            foreach (var tile in mergingSystem.GetComponent<TileSystem>().containedTiles)
            {
                if (!containedTiles.Contains(tile))
                {
                    AddToSystem(tile);
                }
            }
        }
    }

    public void AddToSystem(GameObject tileToAdd)
    {
        containedTiles.Add(tileToAdd);
        if (tileToAdd.GetComponent<TileController>().isExit)
        {
            connectedExits.Add(tileToAdd);
        }
    }
}
