using System.Linq;
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
    public int unplacedTiles = 0;
    public int remainingTiles = 28;

    // Vars for Special Tiles
    public GameObject specialTile;
    public Transform[] specialTileSpawnPoints;
    public bool specialTileTray = true;

    List<Tile> TileOptions = new List<Tile>();
    private List<Tile> tileChoices;

    // Start is called before the first frame update
    void Start()
    {
        PopulateTileOptions();
        DisburseTiles();
        DisburseSpecialTiles();
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
                GameObject newTile = Instantiate(Tile, tileSpawnPoints[i].position, tileSpawnPoints[i].rotation);
                unplacedTiles = unplacedTiles + 1;
                remainingTiles = remainingTiles - 1;
                SpriteRenderer tileSprite = newTile.GetComponentInChildren<SpriteRenderer>();

                if (i == tileSpawnPoints.Length - 1)
                {
                    tileChoices = TileOptions.Where(x => (x.rarity == 2)).ToList();
                } 
                else
                {
                    tileChoices = TileOptions.Where(x => (x.rarity == 1)).ToList();
                }
                var tileChoice = tileChoices[Random.Range(0, tileChoices.Count)];

                tileSprite.sprite = Resources.Load<Sprite>("Sprites/" + tileChoice.tileType);
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

    void DisburseSpecialTiles()
    {

        for (int i = 0; i < specialTileSpawnPoints.Length; i++)
        {
            GameObject newSpecialTile = Instantiate(specialTile, specialTileSpawnPoints[i].position, specialTileSpawnPoints[i].rotation);
            SpriteRenderer tileSprite = newSpecialTile.GetComponentInChildren<SpriteRenderer>();
            tileChoices = TileOptions.Where(x => (x.rarity == 3)).ToList();
            var tileChoice = tileChoices[i];
            tileSprite.sprite = Resources.Load<Sprite>("Sprites/" + tileChoice.tileType);
        }
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
        else
        {
            DisableButton();
        }
        //        Debug.Log(unplacedTiles);
    }

    void EnableButton() 
    {
        disburseTilesButton.GetComponentInChildren<Text>().text = "Confirm Placement";
        //isEnabled = true;
        disburseTilesButton.interactable = true;
        disburseTilesButton.onClick.AddListener(DisburseTiles);
    }

    void DisableButton()
    {
        //isEnabled = false;
        disburseTilesButton.interactable = false;
        disburseTilesButton.onClick.RemoveListener(DisburseTiles);
    }

    void DisableSpecialTileTray()
    {
        //isEnabled = false;
        specialTileTray = false;
    }

    void PopulateTileOptions() 
    {
        TileOptions.Add(new Tile("rStraight", 1));
        TileOptions.Add(new Tile("rCross", 2));
        TileOptions.Add(new Tile("rStraight_tBroken", 3));
        TileOptions.Add(new Tile("rStraight_tBroken", 3));
        TileOptions.Add(new Tile("rStraight_tBroken", 3));
        TileOptions.Add(new Tile("rStraight_tBroken", 3));
    }
}
