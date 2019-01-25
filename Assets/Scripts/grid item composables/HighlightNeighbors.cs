using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(NeighborAwareness))]
public class HighlightNeighbors : MonoBehaviour
{
    private NeighborAwareness neighborAwareness;
    public bool alsoHighlightSelf = true;
    private Highlightable selfHighlightable;
    private void Awake()
    {
        neighborAwareness = GetComponent<NeighborAwareness>();
        if (alsoHighlightSelf)
        {
            selfHighlightable = GetComponent<Highlightable>();
            if ( !selfHighlightable || !selfHighlightable.enabled) throw new Exception("Cant set 'alsoHightSelf' without setting 'highlightable'");
        }
    }

    private void OnMouseEnter()
    {
        foreach (KeyValuePair<string, GameObject> n in neighborAwareness.neighbors)
        {
            GameObject neighbor = n.Value;
            if (neighbor)
            {
                Highlightable highlightable = neighbor.GetComponent<Highlightable>();
                if (highlightable) { highlightable.Highlight(); }
            }
        }
        if (alsoHighlightSelf) selfHighlightable.Highlight();
    }

    private void OnMouseExit()
    {
        foreach (KeyValuePair<string, GameObject> n in neighborAwareness.neighbors)
        {
            GameObject neighbor = n.Value;
            if (neighbor)
            {
                Highlightable highlightable = neighbor.GetComponent<Highlightable>();
                if (highlightable) highlightable.Unhighlight();
            }
        }
        if (alsoHighlightSelf) selfHighlightable.Unhighlight();
    }
}
