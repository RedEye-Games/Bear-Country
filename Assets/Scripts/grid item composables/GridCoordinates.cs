using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCoordinates : MonoBehaviour
{
    public int column;
    public int row;

    public void Init(int newColumn, int newRow)
    {
        column = newColumn;
        row = newRow;
    }
}
