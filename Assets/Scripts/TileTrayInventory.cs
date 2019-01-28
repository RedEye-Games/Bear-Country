using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTrayInventory : MonoBehaviour
{
    public static TileTrayInventory instance;
    public int maxTiles = 4;

    public delegate void OnTileTrayInventoryChanged();
    public OnTileTrayInventoryChanged onTileTrayInventoryChangedCallback;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("multiple tile rack inventories detected");
            return;
        }
        instance = this;
    }
    public List<TileData> currentTiles = new List<TileData>();

    public bool Add( TileData tile)
    {
        if (currentTiles.Count >= maxTiles)
        {
            Debug.LogWarning("too many tiles in tile rack");
            return false;
        }
        currentTiles.Add(tile);

        if (onTileTrayInventoryChangedCallback != null) { onTileTrayInventoryChangedCallback.Invoke(); }
        return true;
    }

    public bool Remove( TileData tile)
    {
        currentTiles.Remove(tile);

        if (onTileTrayInventoryChangedCallback != null) { onTileTrayInventoryChangedCallback.Invoke(); }

        return true;
    }
}
