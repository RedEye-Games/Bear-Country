using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileDisbursementController : MonoBehaviour
{

    public GameObject Tile;
    public Button disburseTilesButton;
    public Transform[] tileSpawnPoints;
    public GameObject[] tiles;
    private bool isEnabled = true;
    public int unplacedTiles = 0;
    public int remainingTiles = 28;

    // Start is called before the first frame update
    void Start()
    {
        DisburseTiles();
    }

    void DisburseTiles()
    {
        // Check to see if all tiles are placed.
        // Check for tiles on board. Confirm them.

        foreach (var tile in tiles)
        {
            if (tile.GetComponent<TileController>().isPlaced)
            {
                tile.GetComponent<TileController>().ConfirmTile();
            }
        }

        for (int i = 0; i < tileSpawnPoints.Length; i++)
        {
            if (remainingTiles > 0)
            {
                Instantiate(Tile, tileSpawnPoints[i].position, tileSpawnPoints[i].rotation);
                unplacedTiles = unplacedTiles + 1;
                remainingTiles = remainingTiles - 1;
            }
        }
        tiles = GameObject.FindGameObjectsWithTag("Tile");
        if (remainingTiles > 0)
        {
            disburseTilesButton.GetComponentInChildren<Text>().text = "Place all tiles.";
        }
        else
        {
            disburseTilesButton.GetComponentInChildren<Text>().text = "Place last tiles!";
        }
        DisableButton();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdatePlaceCount(int count) 
    {
        unplacedTiles = unplacedTiles + count;
        if (unplacedTiles == 0)
        {
            EnableButton();
        }
//        Debug.Log(unplacedTiles);
    }

    void EnableButton() 
    {
        disburseTilesButton.GetComponentInChildren<Text>().text = "Confirm Placement";
        isEnabled = true;
        disburseTilesButton.onClick.AddListener(DisburseTiles);
    }

    void DisableButton()
    {
        isEnabled = false;
        disburseTilesButton.onClick.RemoveListener(DisburseTiles);
    }
}
