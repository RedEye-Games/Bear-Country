using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Highlightable : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Color originalColor;

    private bool isHighlighted;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalColor = meshRenderer.material.color;
    }

    public void Highlight() { isHighlighted = true; UpdateDisplay(); }
    public void Unhighlight() { isHighlighted = false; UpdateDisplay(); }

    private void UpdateDisplay()
    {
        if (isHighlighted) { meshRenderer.material.color = Color.white; }
        else { meshRenderer.material.color = originalColor; }
    }
}
