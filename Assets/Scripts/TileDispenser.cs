using UnityEngine;

public class TileDispenser : MonoBehaviour
{
    public TileData[] TileDatas;

    public void Dispense(int numberOfTiles)
    {
        for (int i=0; i<numberOfTiles; i++)
        {
            TileData tileToAdd = TileDatas[Random.Range(0, TileDatas.Length)];
            TileTrayInventory.instance.Add(tileToAdd);
        }
    }
}
