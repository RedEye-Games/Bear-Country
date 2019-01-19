using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoardManager : MonoBehaviour
{
    private static BoardManager _instance;
    public static BoardManager Instance { get { return _instance; } }

    [SerializeField]
    private GameObject BoardSpacePrefab;

    [SerializeField]
    private GameObject BoardContainer;

    public int tileSpacing = 12;
    public int numberOfColumns = 7;
    public int numberOfRows = 7;

    private GameObject[,] boardSpaces;
    public Head[] Heads;
    public int fav;

    // Singleton
    private void Awake()
    {
        if (_instance != null && _instance != this) { Destroy(this.gameObject); }
        else { _instance = this; }
    }

    void Start()
    {
        boardSpaces = new GameObject[numberOfRows, numberOfColumns];
        Vector3 boardCenter = new Vector3(6F, 0, 78F); // need better centering.
        CreateBoardSpaces(numberOfColumns, numberOfRows, boardCenter);
    }

    void CreateBoardSpaces(int numberOfColumns, int numberOfRows, Vector3 boardCenter)
    {
        Vector3 spawnPosition = new Vector3();
        spawnPosition.x = boardCenter.x - (numberOfColumns * tileSpacing / 2);
        spawnPosition.z = boardCenter.z - (numberOfRows * tileSpacing / 2);
        Quaternion spawnRotation = new Quaternion();

        for (int row = 0; row < numberOfRows; row++)
        {
            for (int column = 0; column < numberOfColumns; column++)
            {
                GameObject boardSpace = Instantiate(BoardSpacePrefab, spawnPosition, spawnRotation);
                boardSpace.name = "xBoardSpace(" + column + "," + row + ")";
                boardSpace.transform.parent = BoardContainer.transform;
                //boardSpace.transform.Translate(new Vector3(row * tileSpacing, 0, -column * tileSpacing));
                boardSpaces[column, row] = boardSpace;
                BoardSpace script = boardSpace.GetComponent<BoardSpace>();
                script.Init(column, row);
                spawnPosition.x = spawnPosition.x + tileSpacing;
            }
            spawnPosition.x = boardCenter.x - (numberOfColumns * tileSpacing / 2);
            spawnPosition.z = spawnPosition.z - tileSpacing;
        }

        // Now that all board squares are created, we can loop through again to set the neighbors.
        for (int col = 0; col < numberOfRows; col++)
        {
            for (int row = 0; row < numberOfColumns; row++)
            {
                GameObject boardSpace = boardSpaces[col, row];
                boardSpace.GetComponent<BoardSpace>().CacheNeighbors();
            }
        }
    }

    public Dictionary<string, GameObject> GetNeighborsOf(int column, int row)
    {
        GameObject left = (column - 1 < 0) ? null : boardSpaces[column - 1, row];
        GameObject right = (column + 1 > numberOfRows - 1) ? null : boardSpaces[column + 1, row];
        GameObject top = (row - 1 < 0) ? null : boardSpaces[column, row - 1];
        GameObject bottom = (row + 1 > numberOfColumns - 1) ? null : boardSpaces[column, row + 1];
        Dictionary<string, GameObject> results = new Dictionary<string, GameObject>();
        results.Add("top", top);
        results.Add("bottom", bottom);
        results.Add("left", left);
        results.Add("right", right);
        return results;
    }
}
