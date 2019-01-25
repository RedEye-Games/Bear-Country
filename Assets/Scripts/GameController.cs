using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSettings
{
    [SerializeField]
    private int numberOfRounds = 7;

    [SerializeField]
    private int tilesPerRound = 4;

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

public class GameController : MonoBehaviour
{
    public GameSettings gameSettings;
    public GameObject selectedTile;

    void Update()
    {
       // quick scene reset for debugging
       if (Input.GetKeyDown("backspace")) { SceneManager.LoadScene(1); }
    }

    //public void AddScore(int newScoreValue)
    //{
    //    score += newScoreValue;
    //    // Debug.Log("The score is " + score + " points");
    //}
}
