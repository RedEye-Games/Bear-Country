using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(NeighborAwareness))]
public class HighlightIfValid : MonoBehaviour
{
    private NeighborAwareness neighborAwareness;
    private Highlightable highlightable;
    private GridCoordinates gridCoordinates;

    private void Awake()
    {
        neighborAwareness = GetComponent<NeighborAwareness>();
        highlightable = GetComponent<Highlightable>();
        gridCoordinates = GetComponent<GridCoordinates>();
    }

    private void OnMouseEnter()
    {
        //bool isValid = IsValid();
        bool isValid = BoardManager.Instance.CanPlaceTileAt(gridCoordinates.column, gridCoordinates.row, GameController.instance.selectedTile, Orientation._0);


        if (isValid)
        {
            highlightable?.Highlight();
        }
    }

    private bool IsValid()
    {
        bool isItValid = true;

        foreach (KeyValuePair<string, GameObject> n in neighborAwareness.neighbors)
        {
            GameObject neighbor = n.Value;
            if (neighbor)
            {
                TileReception tileReception = neighbor.GetComponent<TileReception>();
                if (tileReception != null)
                {
                    //isItValid = true;
                }
                else
                {
                    isItValid = false;
                }
            }
        }

        return isItValid;
    }
    private void OnMouseExit()
    {
        highlightable?.Unhighlight();
    }
}
