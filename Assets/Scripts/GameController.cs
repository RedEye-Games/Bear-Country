using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject tile;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 BoardCenter = new Vector3(0.5F, 0, 0.5F);
        GenerateBoard(2, 2, BoardCenter);
    }

    void GenerateBoard(int sizeX, int sizeZ, Vector3 BoardCenter)
    {
        Vector3 spawnPosition = new Vector3();
        spawnPosition.x = BoardCenter.x - (sizeX / 2);
        spawnPosition.z = BoardCenter.z - (sizeZ / 2);
        Quaternion spawnRotation = new Quaternion();
        for (int i = 0; i < sizeZ; i++)
        {
            for (int b = 0; b < sizeX; b++)
            {
                Instantiate(tile, spawnPosition, spawnRotation);
                spawnPosition.x = spawnPosition.x + 1;
            }
            spawnPosition.x = BoardCenter.x - (sizeX / 2);
            spawnPosition.z = spawnPosition.z + 1;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
