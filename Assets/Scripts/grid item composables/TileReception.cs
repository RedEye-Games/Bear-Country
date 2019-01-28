using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileReception : MonoBehaviour
{
    [SerializeField] private GameObject[] tilePrefabs;

    public int tileOrientation;

    private GameObject tile;

    private void OnMouseDown()
    {
        if (!tile) AddTile();
        //else RotateTile();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collide");
    }

    private void AddTile()
    {
        if (!tile)
        {
            GameObject prefab = tilePrefabs[UnityEngine.Random.Range(0, tilePrefabs.Length - 1)];
            GameObject newTile = Instantiate(prefab, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2.5f, gameObject.transform.position.z), gameObject.transform.rotation, gameObject.transform);
            tile = newTile;
        }
    }

    private void RemoveTile()
    {
        if (tile) Destroy(tile);
    }

    public void RotateTile()
    {
        tileOrientation = (tileOrientation + 90) % 360;
        tile.GetComponent<PathEnabled>().setOrientation(tileOrientation);
        tile.transform.Rotate(new Vector3(0, 1, 0), 90);
    }
}

