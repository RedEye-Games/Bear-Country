using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCoordinates : MonoBehaviour
{
    private int column;
    private int row;

    public void Init(int newColumn, int newRow)
    {
        column = newColumn;
        row = newRow;
    }
}
