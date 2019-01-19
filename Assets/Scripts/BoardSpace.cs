using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSpace : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Color originalColor;

    [SerializeField]
    private GameObject TilePrefab;

    public int column;
    public int row;
    private int contentOrientation = 0;
    private Dictionary<string, GameObject> neighbors;
    private Dictionary<string, BoardSpace> neighborScripts;
    public bool isHighlighted;
    public GameObject contents;

    public void Init(int newColumn, int newRow)
    {
        column = newColumn;
        row = newRow;
        meshRenderer = GetComponent<MeshRenderer>();
        originalColor = meshRenderer.material.color;
    }

    // Caching a ref to each neighbor and its script, for performance. This has to happen after .Init() bc we have to wait for all other tiles to be created first.
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
        if (!contents)
        {
            AddTile();
        }
        else
        {
            RotateTile();
        }
    }

    private void AddTile()
    {
        if (!contents)
        {
            GameObject tile = Instantiate(TilePrefab, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.5f, gameObject.transform.position.z), gameObject.transform.rotation, gameObject.transform);
            contents = tile;
        }
    }

    private void RemoveTile()
    {
        if (contents)
        {
            Destroy(contents);
        }
    }

    public void RotateTile()
    {
        contentOrientation = (contentOrientation + 1) % 4;
        contents.transform.Rotate(new Vector3(0, 1, 0), 90);
    }

        public void Highlight() { isHighlighted = true; }
    private void Unhighlight() { isHighlighted = false; }

    private void UpdateDisplay()
    {
        if (contents) { meshRenderer.material.color = Color.green; }
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
