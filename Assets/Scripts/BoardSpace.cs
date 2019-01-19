using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSpace : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Color originalColor;
    public int column;
    public int row;
    private Dictionary<string, GameObject> neighbors;
    private Dictionary<string, BoardSpace> neighborScripts;
    public bool hasTile;
    public bool isHighlighted;

    public void Init(int newColumn, int newRow)
    {
        column = newColumn;
        row = newRow;
        meshRenderer = GetComponent<MeshRenderer>();
        originalColor = meshRenderer.material.color;
    }

    // Caching a ref to each neighbor and its script. This has to happen separately after .init() bc we have to wait for all other tiles to be created first.
    public void CacheNeighbors()
    {
        neighbors = BoardManager.Instance.GetNeighborsOf(column, row);
        neighborScripts = new Dictionary<string, BoardSpace>();
        foreach (KeyValuePair<string, GameObject> neighbor in neighbors)
        {
            if (neighbor.Value != null) neighborScripts.Add(neighbor.Key, neighbor.Value.GetComponent<BoardSpace>());
        }
    }

    private void OnMouseDown()
    {
        AddTile();
    }

    private void AddTile()
    {
        if (!hasTile)
        {
            hasTile = true;
            UpdateDisplay();
        }
    }

    public void Highlight() { isHighlighted = true; UpdateDisplay(); }
    private void Unhighlight() { isHighlighted = false; }

    private void UpdateDisplay()
    {
        if (hasTile) { meshRenderer.material.color = Color.green; }
        else if (isHighlighted) { meshRenderer.material.color = Color.white; }
        else { meshRenderer.material.color = originalColor; }
    }

    private void OnMouseEnter()
    {
        foreach (KeyValuePair<string, BoardSpace> neighborScript in neighborScripts)
        {
            if (neighborScript.Value != null)
            {
                neighborScript.Value.Highlight();
                neighborScript.Value.UpdateDisplay();
            }
        }
        Highlight();
        UpdateDisplay();
    }

    private void OnMouseExit()
    {
        foreach (KeyValuePair<string, BoardSpace> neighborScript in neighborScripts)
        {
            if (neighborScript.Value != null)
            {
                neighborScript.Value.Unhighlight();
                neighborScript.Value.UpdateDisplay();
            }
        }
        Unhighlight();
        UpdateDisplay();
    }
}
