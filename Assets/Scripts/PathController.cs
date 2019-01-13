using System.Collections;
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

        scoreToAdd = -1;

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

        if (other.CompareTag(pathTag))
        {
            GameObject adjacentPath = other.gameObject;
            Debug.Log("Touched a " + pathTag);
            isDeadEnd = false;
            scoreToAdd = 1;
            //var adjacentPathName = other.gameObject.name;
            // Check to see if path is alive. Check to see if it's a riverhead.
            //gameController.AddScore(1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isDeadEnd = true;
        //gameController.AddScore(-1);
        scoreToAdd = -1;
    }

}
