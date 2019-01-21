using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// quick scene reset for debugging
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    public GameObject boardSpace;
    public float boardScale = 10;
    public GameObject selectedTile;

    private int score;


    // Start is called before the first frame update
    void Start()
    {
        Vector3 BoardCenter = new Vector3(boardScale / 2, 0, boardScale / 2);
        GenerateBoard(7, 7, BoardCenter);
        score = 0;
    }

    void GenerateBoard(int sizeX, int sizeZ, Vector3 BoardCenter)
    {
        Vector3 spawnPosition = new Vector3();
        spawnPosition.x = BoardCenter.x - (sizeX * boardScale / 2);
        spawnPosition.z = BoardCenter.z - (sizeZ * boardScale / 2);
        Quaternion spawnRotation = new Quaternion();
        for (int i = 0; i < sizeZ; i++)
        {
            for (int b = 0; b < sizeX; b++)
            {
                Instantiate(boardSpace, spawnPosition, spawnRotation);
                spawnPosition.x = spawnPosition.x + boardScale;
            }
            spawnPosition.x = BoardCenter.x - (sizeX * boardScale / 2);
            spawnPosition.z = spawnPosition.z + boardScale;
        }
    }
    // Update is called once per frame

    void Update()
    {
       // quick scene reset for debugging
       if (Input.GetKeyDown("backspace"))
        {
            SceneManager.LoadScene(1);
        }
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
//        Debug.Log("The score is " + score + " points");
    }

}
