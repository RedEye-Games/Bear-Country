using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoardManager : MonoBehaviour
{
    private static BoardManager _instance;
    public static BoardManager Instance { get { return _instance; } }

    [SerializeField]
    private BoardManagerPrefabs prefabs;

    [SerializeField]
    private GameObject BoardContainer;

    [SerializeField]
    private GameObject TileSpawnPoint;

    public int tileSpacing = 12;
    public int numberOfColumns = 7;
    public int numberOfRows = 7;

    private GameObject[,] boardSpaces;
    public Head[] Heads;

    private void Awake()
    {
        if (_instance != null && _instance != this) { Destroy(this.gameObject); }
        else { _instance = this; }
    }

    void Start()
    {
        boardSpaces = new GameObject[numberOfRows + 2, numberOfColumns + 2];
        GameObject boardContainer = GetBoardContainer();
        CreateBoard(numberOfColumns, numberOfRows, boardContainer);
    }

    GameObject GetBoardContainer()
    {
        GameObject gameBoard = GameObject.FindWithTag("Board");
        if (gameBoard == null) Debug.Log("Cannot find game object with tag 'Board'");
        return gameBoard;
    }

    void CreateBoard(int numberOfColumns, int numberOfRows, GameObject boardContainer)
    {
        //Vector3 spawnPosition = new Vector3(-44F, 0F, 74F); // need better centering.
        Vector3 spawnPosition = new Vector3( TileSpawnPoint.transform.position.x, TileSpawnPoint.transform.position.y, TileSpawnPoint.transform.position.z);
        //spawnPosition.x = boardContainer.transform.position.x;
        //spawnPosition.y = 0;
        //spawnPosition.z = boardContainer.transform.position.z;
        Quaternion spawnRotation = new Quaternion();

        void getHead(Head head, int column, int row)
        {
            GameObject prefab = new GameObject();

            if (head.type == Head.HeadType.River)
            {
                prefab = Instantiate(prefabs.RiverHeadPrefab, spawnPosition, spawnRotation);
                prefab.name = "RiverHead(top," + head.location + ")";
            }
            else if (head.type == Head.HeadType.Trail)
            {
                prefab = Instantiate(prefabs.TrailHeadPrefab, spawnPosition, spawnRotation);
                prefab.name = "TrailHead(top," + head.location + ")";
            }

            boardSpaces[column, row] = prefab;
            prefab.GetComponent<GridCoordinates>().Init(column, row);
            if (head.edge == Head.Edge.Left || head.edge == Head.Edge.Right) prefab.transform.Rotate(new Vector3(0, 1, 0), 90);
            //prefab.transform.parent = BoardContainer.transform;
        }

        void getBlank(int column, int row)
        {
            GameObject gridItem = Instantiate(prefabs.BlankGridItemPrefab, spawnPosition, spawnRotation);
            gridItem.name = "blankSpace(" + column + "," + row + ")";
            //gridItem.transform.parent = BoardContainer.transform;
            boardSpaces[column, row] = gridItem;
            gridItem.GetComponent<MeshRenderer>().enabled = false;
            gridItem.GetComponent<GridCoordinates>().Init(column, row);
        }

        void getBoardSpace(int column, int row)
        {
            GameObject gridItem = Instantiate(prefabs.BoardSpacePrefab, spawnPosition, spawnRotation);
            gridItem.name = "boardSpace(" + column + "," + row + ")";
            //gridItem.transform.parent = BoardContainer.transform;
            boardSpaces[column, row] = gridItem;
            gridItem.GetComponent<GridCoordinates>().Init(column, row);
        }

        float originalSpawnX = spawnPosition.x;

        /* TODO: Refactor a more elegant way to do this part */
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

                spawnPosition.x = spawnPosition.x + tileSpacing;
            }
            spawnPosition.x = originalSpawnX;
            spawnPosition.z = spawnPosition.z - tileSpacing;
        }

        // Now that all board squares are created, we can loop through again to set the neighbors.
        for (int col = 0; col < numberOfRows + 2; col++)
        {
            for (int row = 0; row < numberOfColumns + 2; row++)
            {
                GameObject gridItem = boardSpaces[col, row];
                if (gridItem.tag == "BoardSpace")
                    gridItem.GetComponent<NeighborAwareness>().CacheNeighbors(col, row);
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

[Serializable]
public class BoardManagerPrefabs
{
    [SerializeField]
    private GameObject boardSpacePrefab;
    public GameObject BoardSpacePrefab { get { return boardSpacePrefab; } }

    [SerializeField]
    private GameObject blankGridItemPrefab;
    public GameObject BlankGridItemPrefab { get { return blankGridItemPrefab; } }

    [SerializeField]
    private GameObject riverHeadPrefab;
    public GameObject RiverHeadPrefab { get { return riverHeadPrefab; } }

    [SerializeField]
    private GameObject trailHeadPrefab;
    public GameObject TrailHeadPrefab { get { return trailHeadPrefab; } }
}