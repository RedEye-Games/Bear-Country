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
                newTile.GetComponentInChildren<TileController>().northPath.gameObject.tag = tileChoice.northPath;
                newTile.GetComponentInChildren<TileController>().southPath.gameObject.tag = tileChoice.southPath;
                newTile.GetComponentInChildren<TileController>().eastPath.gameObject.tag = tileChoice.eastPath;
                newTile.GetComponentInChildren<TileController>().westPath.gameObject.tag = tileChoice.westPath;

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
        // Common Tiles
        TileOptions.Add(new Tile("rStraight", 1, "River", "River", "Path", "Path"));
        TileOptions.Add(new Tile("rFork", 1, "Path", "River", "River", "River"));
        TileOptions.Add(new Tile("rCorner", 1, "River", "Path", "Path", "River"));
        TileOptions.Add(new Tile("tStraight", 1, "Trail", "Trail", "Path", "Path"));
        TileOptions.Add(new Tile("tFork", 1, "Path", "Trail", "Trail", "Trail"));
        TileOptions.Add(new Tile("tCorner", 1, "Trail", "Path", "Path", "Trail"));

        // Rare Tiles
        TileOptions.Add(new Tile("rStraight_tStraight", 2, "River", "Trail", "Path", "Path"));
        TileOptions.Add(new Tile("rtCorner", 2, "Path", "River", "Trail", "Path"));
        TileOptions.Add(new Tile("tBridge_rStraight", 2, "River", "River", "Trail", "Trail"));

        // Special Tiles
        TileOptions.Add(new Tile("rStraight_tBroken", 3, "River", "River", "Trail", "Trail"));
        TileOptions.Add(new Tile("rCross_tStraight", 3, "River", "Trail", "River", "River"));
        TileOptions.Add(new Tile("rCross", 3, "River", "River", "River", "River"));
        TileOptions.Add(new Tile("tFork_rStraight", 3, "River", "Trail", "Trail", "Trail"));
        TileOptions.Add(new Tile("tCross", 3, "Trail", "Trail", "Trail", "Trail"));
        TileOptions.Add(new Tile("tCorner_rCorner", 3, "Trail", "River", "Trail", "River"));
    }
}
