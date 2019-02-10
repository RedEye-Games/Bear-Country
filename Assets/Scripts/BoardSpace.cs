using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSpace : MonoBehaviour
{
    public bool isOccupied = false;
    public bool hasPotential = false;
    public bool isHighlighted = false;
    public Renderer rend;
    public List<string> pathTagList;

    // Start is called before the first frame update
    void Start()
    {
        rend.material.color = Color.clear;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Untagged" && other.tag != "Path" && other.tag != "Board" && other.tag != "BoardSpace" && other.tag != "Tile")
        {
            pathTagList.Add(other.tag);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Untagged" && other.tag != "Path" && other.tag != "Board" && other.tag != "BoardSpace" && other.tag != "Tile")
        {
            pathTagList.Remove(other.tag);
        }
    }

    public void Highlight(bool highlight)
    {
        // Highlight here.
        isHighlighted = highlight;
        if (hasPotential)
        {
            rend.material.color = Color.green;
        } 
        else 
        {
            rend.material.color = Color.clear;
        }

    }

    public void CheckPotential(string pathTag)
    {
        if (pathTagList.Contains(pathTag))
        {
            //Highlight(true);
            hasPotential = true;
        }
    }

    public void ClearPotential()
    {
        hasPotential = false;
        Highlight(false);
    }
}
