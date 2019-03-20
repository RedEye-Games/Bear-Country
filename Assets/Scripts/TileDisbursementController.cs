using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileDisbursementController : MonoBehaviour
{
    // GameController
    private GameController gameController;

    // ScoreBoard
    private ScoreBoard scoreBoard;

    public GameObject Tile;
    public Button disburseTilesButton;
    public Transform[] tileSpawnPoints;
    public GameObject[] tiles;
    public int unplacedTiles = 0;
    public int remainingTiles = 28;

    // Vars for Special Tiles
    public GameObject SpecialTile;
    public GameObject[] specialTiles;
    public Transform[] specialTileSpawnPoints;
    public GameObject specialTileTray;
    public bool specialTilePlacedThisRound = false;
    public int remainingSpecialTiles = 3;

    List<Tile> TileOptions = new List<Tile>();
    private List<Tile> tileChoices;

    private bool disbursingTiles = false;

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

        // Locate ScoreBoard Script
        // ToDo: Trigger to EventManager
        GameObject scoreBoardObject = GameObject.FindWithTag("ScoreBoard");
        if (scoreBoardObject != null)
        {
            scoreBoard = scoreBoardObject.GetComponent<ScoreBoard>();
        }
        if (scoreBoardObject == null)
        {
            Debug.Log("Cannot find 'ScoreBoard' script");
        }

        PopulateTileOptions();
        DisburseTiles();
        DisburseSpecialTiles();
    }

    void DisburseTiles()
    {
        DisableButton();
        disbursingTiles = true;
        // Check to see if all tiles are placed.
        // Check for tiles on board. Confirm them.

        // Check to see if any special tiles remain

        if (remainingSpecialTiles != 1)
        {
            if (gameController.specialTilesPlacedThisRound.Any())
            {
                remainingSpecialTiles--;
            }
            // Reset Special Tile
            EnableSpecialTileTray();
        }

        foreach (var tile in gameController.GetComponent<GameController>().tilesPlacedThisRound)
        {
            tile.GetComponent<TileController>().ConfirmTile();
        }
        foreach (var tile in gameController.GetComponent<GameController>().specialTilesPlacedThisRound)
        {
            tile.GetComponent<TileController>().ConfirmTile();
        }

        // Round Cleanup
        // ToDo: Move to more appropriate class.
        scoreBoard.GetComponent<ScoreBoard>().ScoreTiles();
        gameController.EndRound();

        // Begin Dispersal
        for (int i = 0; i < tileSpawnPoints.Length; i++)
        {
            if (remainingTiles > 0)
            {
                GameObject newTile = Instantiate(Tile, tileSpawnPoints[i].position, tileSpawnPoints[i].rotation);
                unplacedTiles = unplacedTiles + 1;
                remainingTiles = remainingTiles - 1;
                SpriteRenderer[] tileSprites = newTile.GetComponentsInChildren<SpriteRenderer>();

                if (i == tileSpawnPoints.Length - 1)
                {
                    tileChoices = TileOptions.Where(x => (x.rarity == 2)).ToList();
                } 
                else
                {
                    tileChoices = TileOptions.Where(x => (x.rarity == 1)).ToList();
                }
                var tileChoice = tileChoices[Random.Range(0, tileChoices.Count)];
                foreach (var tileSprite in tileSprites)
                {
                    tileSprite.sprite = Resources.Load<Sprite>("Sprites/" + tileChoice.tileType);
                }
                newTile.GetComponentInChildren<TileController>().northPath.gameObject.tag = tileChoice.northPath;
                newTile.GetComponentInChildren<TileController>().southPath.gameObject.tag = tileChoice.southPath;
                newTile.GetComponentInChildren<TileController>().eastPath.gameObject.tag = tileChoice.eastPath;
                newTile.GetComponentInChildren<TileController>().westPath.gameObject.tag = tileChoice.westPath;
                gameController.PopulateTileSystems(newTile);
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
        disbursingTiles = false;
    }

    void DisburseSpecialTiles()
    {

        for (int i = 0; i < specialTileSpawnPoints.Length; i++)
        {
            GameObject newSpecialTile = Instantiate(SpecialTile, specialTileSpawnPoints[i].position, specialTileSpawnPoints[i].rotation);
            newSpecialTile.GetComponentInChildren<TileController>().isSpecial = true;
            newSpecialTile.tag = "SpecialTile";
            SpriteRenderer[] tileSprites = newSpecialTile.GetComponentsInChildren<SpriteRenderer>();
            tileChoices = TileOptions.Where(x => (x.rarity == 3)).ToList();
            var tileChoice = tileChoices[i];
            foreach (var tileSprite in tileSprites)
            {
                tileSprite.sprite = Resources.Load<Sprite>("Sprites/" + tileChoice.tileType);
            }
            newSpecialTile.GetComponentInChildren<TileController>().northPath.gameObject.tag = tileChoice.northPath;
            newSpecialTile.GetComponentInChildren<TileController>().southPath.gameObject.tag = tileChoice.southPath;
            newSpecialTile.GetComponentInChildren<TileController>().eastPath.gameObject.tag = tileChoice.eastPath;
            newSpecialTile.GetComponentInChildren<TileController>().westPath.gameObject.tag = tileChoice.westPath;
            gameController.PopulateTileSystems(newSpecialTile);
        }
        specialTiles = GameObject.FindGameObjectsWithTag("SpecialTile");
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void ToggleButtons()
    {
        if (gameController.specialTilesPlacedThisRound.Count == 1)
        {
            DisableSpecialTileTray();
        } else
        {
            EnableSpecialTileTray();
        }

        if (gameController.tilesPlacedThisRound.Count == 4 && !disbursingTiles)
        {
            EnableButton();
        } else
        {
            DisableButton();
        }
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
            }
        }
    }

    void EnableButton() 
    {
        if (!disburseTilesButton.interactable)
        {
            disburseTilesButton.GetComponentInChildren<Text>().text = "Confirm Placement";
            disburseTilesButton.interactable = true;
            disburseTilesButton.onClick.AddListener(DisburseTiles);
        }
    }

    void DisableButton()
    {
        disburseTilesButton.interactable = false;
        disburseTilesButton.onClick.RemoveListener(DisburseTiles);
    }

    void DisableSpecialTileTray()
    {
        specialTileTray.SetActive(false);
        DisableSpecialTiles();
    }

    void EnableSpecialTileTray()
    {
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
        TileOptions.Add(new Tile("rStraight_tStraight", 2, "Trail", "River", "Path", "Path"));
        TileOptions.Add(new Tile("rtCorner", 2, "Path", "River", "Path", "Trail"));
        TileOptions.Add(new Tile("tBridge_rStraight", 2, "River", "River", "Trail", "Trail"));

        // Special Tiles
        TileOptions.Add(new Tile("rStraight_tBroken", 3, "River", "River", "Trail", "Trail"));
        TileOptions.Add(new Tile("rCross_tStraight", 3, "River", "Trail", "River", "River"));
        TileOptions.Add(new Tile("rCross", 3, "River", "River", "River", "River"));
        TileOptions.Add(new Tile("tFork_rStraight", 3, "River", "Trail", "Trail", "Trail"));
        TileOptions.Add(new Tile("tCross", 3, "Trail", "Trail", "Trail", "Trail"));
        TileOptions.Add(new Tile("tCorner_rCorner", 3, "River", "Trail", "River", "Trail"));
    }

}
