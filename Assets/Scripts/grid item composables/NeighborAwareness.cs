using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighborAwareness : MonoBehaviour
{
    public Dictionary<string, GameObject> neighbors;

    // Caching a ref to each neighbor and its script, for performance. This has to happen after .Init() bc we have to wait for all other tiles to be created first.
    public void CacheNeighbors(int selfColumn, int selfRow)
    {
        neighbors = BoardManager.Instance.GetNeighborsOf(selfColumn, selfRow);
    }
}
