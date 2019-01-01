using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour
{
    public GameObject boardSpace;
    public float boardScale = 10;


    // Start is called before the first frame update
    void Start()
    {
        Vector3 BoardCenter = new Vector3(0.5F, 0, 0.5F);
        GenerateBoard(7, 7, BoardCenter);
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
        
    }
}
