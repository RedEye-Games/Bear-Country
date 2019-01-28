using UnityEngine;

public class TileTrayUI : MonoBehaviour
{
    public Transform tileTray;

    TileTrayInventory inventory;

    TileTrayItem[] slots;

    void Start()
    {
        inventory = TileTrayInventory.instance;
        inventory.onTileTrayInventoryChangedCallback += UpdateUI;

        slots = tileTray.GetComponentsInChildren<TileTrayItem>();
    }

    void UpdateUI()
    {
        Debug.Log("updating tile rack ui: " + inventory.currentTiles.Count);
        for (int i=0; i < slots.Length; i++)
        {
            if (i < inventory.currentTiles.Count)
            {
                slots[i].AddTile(inventory.currentTiles[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }

    public void UnselectAll()
    {
        for (int i=0; i<slots.Length; i++)
        {
            slots[i].Unselect();
        }
    }
}
