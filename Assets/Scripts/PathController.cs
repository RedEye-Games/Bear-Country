﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    private TileController parentTile;

    private string pathTag;
    private bool isDeadEnd = true;
    public int scoreToAdd;

    // Start is called before the first frame update
    void Start()
    {
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
        if (other.gameObject.tag != "Untagged" && other.CompareTag(pathTag))
        {
            GameObject adjacentPath = other.gameObject;

            //var adjacentPathName = other.gameObject.name;
            if (pathTag == gameObject.tag)
            {
                isDeadEnd = false;
                scoreToAdd = 1;
            }
            // To Do: Check to see if path is alive. Check to see if it's a riverhead.
            // Then create and store unique river/trail identifier.
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isDeadEnd = true;
        scoreToAdd = -1;
    }

}
