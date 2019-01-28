using UnityEngine;

public class TileTrayUI : MonoBehaviour
{
    private static TileTrayUI _instance;
    public static TileTrayUI Instance { get { return _instance; } }

    public Transform tileTray;

    TileTrayInventory inventory;

    TileTrayItem[] slots;

    private void Awake()
    {
        if (_instance != null && _instance != this) { Debug.LogWarning("too many tiletrayUIs"); }
        else { _instance = this; }
    }

    public void Init()
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
