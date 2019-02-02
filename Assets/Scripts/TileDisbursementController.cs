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
    public GameObject[] specialTiles;
    public Transform[] specialTileSpawnPoints;
    public GameObject specialTileTray;
    public bool specialTilePlacedThisRound = false;

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

                // Reset Special Tile
                specialTilePlacedThisRound = false;
                EnableSpecialTileTray();
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
            newSpecialTile.GetComponentInChildren<TileController>().isSpecial = true;
            newSpecialTile.tag = "SpecialTile";
            SpriteRenderer tileSprite = newSpecialTile.GetComponentInChildren<SpriteRenderer>();
            tileChoices = TileOptions.Where(x => (x.rarity == 3)).ToList();
            var tileChoice = tileChoices[i];
            tileSprite.sprite = Resources.Load<Sprite>("Sprites/" + tileChoice.tileType);
        }
        specialTiles = GameObject.FindGameObjectsWithTag("SpecialTile");
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdatePlaceCount(int count, bool isSpecial = false)
    {
        if (isSpecial == true) 
        {
            if (count == -1)
            {
                specialTilePlacedThisRound = true;
                DisableSpecialTileTray();
            }
            else
            {
                specialTilePlacedThisRound = false;
                EnableSpecialTileTray();
            }
        }
        else
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
        }
        //        Debug.Log(unplacedTiles);
    }

    void EnableSpecialTiles()
    {
        for (int i = 0; i < specialTiles.Length; i++)
        {
            specialTiles[i].SetActive(true);
        }
    }

    void DisableSpecialTiles()
    {
        for (int i = 0; i < specialTiles.Length; i++)
        {
            if (specialTiles[i].GetComponentInChildren<TileController>().isPlaced == false)
            {
                specialTiles[i].SetActive(false);
                Debug.Log("Inactive.");
            }
        }
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
        specialTileTray.SetActive(false);
        DisableSpecialTiles();
    }

    void EnableSpecialTileTray()
    {
        //isEnabled = false;
        specialTileTray.SetActive(true);
        EnableSpecialTiles();
    }

    void PopulateTileOptions() 
    {
        TileOptions.Add(new Tile("rStraight", 1));
        TileOptions.Add(new Tile("rCross", 2));
        TileOptions.Add(new Tile("rStraight_tBroken", 3));
        TileOptions.Add(new Tile("rStraight_tBroken", 3));
        TileOptions.Add(new Tile("rStraight_tBroken", 3));
        TileOptions.Add(new Tile("rStraight_tBroken", 3));
        TileOptions.Add(new Tile("rStraight_tBroken", 3));
        TileOptions.Add(new Tile("rStraight_tBroken", 3));
    }
}
