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
    private GameObject BlankGridItemPrefab;

    [SerializeField]
    private GameObject RiverHeadPrefab;

    [SerializeField]
    private GameObject TrailHeadPrefab;

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
        boardSpaces = new GameObject[numberOfRows + 2, numberOfColumns + 2];
        Vector3 boardCenter = new Vector3(-6F, 0, 82F); // need better centering.
        CreateBoard(numberOfColumns, numberOfRows, boardCenter);
    }

    void CreateBoard(int numberOfColumns, int numberOfRows, Vector3 boardCenter)
    {
        Vector3 spawnPosition = new Vector3();
        spawnPosition.x = boardCenter.x - (numberOfColumns * tileSpacing / 2);
        spawnPosition.z = boardCenter.z - (numberOfRows * tileSpacing / 2);
        Quaternion spawnRotation = new Quaternion();

        void getHead(Head head, int column, int row)
        {
            GameObject prefab;
            if (head.type == Head.HeadType.River)
            {
                prefab = Instantiate(RiverHeadPrefab, spawnPosition, spawnRotation);
                prefab.name = "RiverHead(top," + head.location + ")";
                prefab.transform.parent = BoardContainer.transform;
                boardSpaces[column, row] = prefab;
                BoardSpace script = prefab.GetComponent<BoardSpace>();
                script.Init(column, row);
                if (head.edge == Head.Edge.Left || head.edge == Head.Edge.Right) prefab.transform.Rotate(new Vector3(0, 1, 0), 90);
            }
            else if (head.type == Head.HeadType.Trail)
            {
                prefab = Instantiate(TrailHeadPrefab, spawnPosition, spawnRotation);
                prefab.name = "TrailHead(top," + head.location + ")";
                prefab.transform.parent = BoardContainer.transform;
                boardSpaces[column, row] = prefab;
                BoardSpace script = prefab.GetComponent<BoardSpace>();
                script.Init(column, row);
                if (head.edge == Head.Edge.Left || head.edge == Head.Edge.Right) prefab.transform.Rotate(new Vector3(0, 1, 0), 90);
            }
        }

        void getBlank(int column, int row)
        {
            GameObject gridItem = Instantiate(BlankGridItemPrefab, spawnPosition, spawnRotation);
            gridItem.name = "blankSpace(" + column + "," + row + ")";
            gridItem.transform.parent = BoardContainer.transform;
            boardSpaces[column, row] = gridItem;
            gridItem.GetComponent<MeshRenderer>().enabled = false;
            BoardSpace script = gridItem.GetComponent<BoardSpace>();
            script.Init(column, row);

        }

        void getBoardSpace(int column, int row)
        {
            GameObject gridItem;
            gridItem = Instantiate(BoardSpacePrefab, spawnPosition, spawnRotation);
            gridItem.name = "boardSpace(" + column + "," + row + ")";
            gridItem.transform.parent = BoardContainer.transform;
            boardSpaces[column, row] = gridItem;
            BoardSpace script = gridItem.GetComponent<BoardSpace>();
            script.Init(column, row);
        }

        for (int row = 0; row < numberOfRows + 2; row++)
        {
            for (int column = 0; column < numberOfColumns + 2; column++)
            {
                if (row == 0) 
                {
                    bool found = false;
                    foreach (Head head in Heads)
                    {
                        if (head.edge == Head.Edge.Top && head.location == column) { 
                            getHead(head, column, row);
                            found = true;
                        }

                    }
                    if (!found) { getBlank(column, row); }
                }
                else if (row ==  numberOfRows + 1)
                {
                    bool found = false;
                    foreach (Head head in Heads)
                    {
                        if (head.edge == Head.Edge.Bottom && head.location == column) { 
                            getHead(head, column, row);
                            found = true;
                        }
                    }
                    if (!found) { getBlank(column, row); }
                }
                else if (column == 0)
                {
                    bool found = false;
                    foreach (Head head in Heads)
                    {
                        if (head.edge == Head.Edge.Left && head.location == row) { 
                            getHead(head, column, row);
                            found = true;
                        }
                    }
                    if (!found) { getBlank(column, row); }
                }
                else if (column == numberOfColumns + 1)
                {
                    bool found = false;
                    foreach (Head head in Heads)
                    {
                        if (head.edge == Head.Edge.Right && head.location == row) { 
                            getHead(head, column, row);
                            found = true;
                        }
                    }
                    if (!found) { getBlank(column, row); }
                }
                else {
                    getBoardSpace(column, row);
                }

                //boardSpace.transform.Translate(new Vector3(row * tileSpacing, 0, -column * tileSpacing));
                spawnPosition.x = spawnPosition.x + tileSpacing;
            }
            spawnPosition.x = boardCenter.x - (numberOfColumns * tileSpacing / 2);
            spawnPosition.z = spawnPosition.z - tileSpacing;
        }

        // Now that all board squares are created, we can loop through again to set the neighbors.
        for (int col = 0; col < numberOfRows + 2; col++)
        {
            for (int row = 0; row < numberOfColumns + 2; row++)
            {
                GameObject boardSpace = boardSpaces[col, row];
                boardSpace.GetComponent<BoardSpace>().CacheNeighbors();
            }
        }
    }

    public Dictionary<string, GameObject> GetNeighborsOf(int column, int row)
    {
        GameObject left = (column - 1 < 0) ? null : boardSpaces[column - 1, row];
        GameObject right = (column + 1 > numberOfRows + 1) ? null : boardSpaces[column + 1, row];
        GameObject top = (row - 1 < 0) ? null : boardSpaces[column, row - 1];
        GameObject bottom = (row + 1 > numberOfColumns + 1) ? null : boardSpaces[column, row + 1];

        Dictionary<string, GameObject> results = new Dictionary<string, GameObject>();
        results.Add("top", top);
        results.Add("bottom", bottom);
        results.Add("left", left);
        results.Add("right", right);
        return results;
    }
}
