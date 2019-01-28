using UnityEngine;
using UnityEngine.UI;

public class TileTrayItem : MonoBehaviour
{
    private TileData tileData;
    private Image icon;
    private Image selfBg;

    private void Start()
    {
        icon = transform.Find("Icon").GetComponent<Image>();
        selfBg = GetComponent<Image>();
    }
 
    public void AddTile(TileData newTileData)
    {
        tileData = newTileData;
        icon.sprite = newTileData.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        tileData = null;
        icon.enabled = false;
    }

    public void Select()
    {
        GameController.instance.SetSelectedTile(tileData);
    }

    public void Unselect()
    {
        GameController.instance.SetSelectedTile(null);
    }
}
