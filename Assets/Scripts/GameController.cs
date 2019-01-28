using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSettings
{
    [SerializeField]
    private int numberOfRounds = 7;

    public int tilesPerRound = 4;

    [SerializeField]
    private int specialTilesPerGame = 3;
}

//public class Game
//{
//private int currentRound;
//private int score;
//    public Game (GameSettings gameSettings, Board board)
//    {

//    }
//void Start()
//{
//    score = 0;
//    currentRound = 1;
//}
//}
[RequireComponent(typeof(TileDispenser))]
public class GameController : MonoBehaviour
{
    public static GameController instance;

    public GameSettings gameSettings;
    public TileData selectedTile;
    private TileDispenser tileDispenser;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("multiple game controllers detected");
            return;
        }
        instance = this;

        gameSettings = new GameSettings();
        tileDispenser = GetComponent<TileDispenser>();
    }

    private void Start()
    {
        TileTrayUI.Instance.Init();
        tileDispenser.Dispense(gameSettings.tilesPerRound);
    }

    void Update()
    {
        // quick scene reset for debugging

        if (Input.GetKeyDown("backspace")) { SceneManager.LoadScene(1); }
    }

    public void SetSelectedTile(TileData tileToSelect)
    {
        selectedTile = tileToSelect;
    }

    //public void AddScore(int newScoreValue)
    //{
    //    score += newScoreValue;
    //    // Debug.Log("The score is " + score + " points");
    //}
}
