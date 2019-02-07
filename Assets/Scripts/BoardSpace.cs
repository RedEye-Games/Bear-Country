using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSpace : MonoBehaviour
{
    public bool isOccupied = false;
    public bool isHighlighted = false;
    public List<string> pathTagList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Untagged" && other.tag != "Board" && other.tag != "BoardSpace")
        {
            pathTagList.Add(other.tag);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Untagged" && other.tag != "Board" && other.tag != "BoardSpace")
        {
            pathTagList.Remove(other.tag);
        }
    }

    public void Highlight(bool highlight)
    {
        // Highlight here.
        isHighlighted = highlight;
        if (highlight)
        {
            GetComponentInChildren<TileCollider>().rend.material.color = Color.green;
        } 
        else 
        {
            GetComponentInChildren<TileCollider>().rend.material.color = Color.red;
        }

    }

    public void CheckPotential(string pathTag)
    {
        if (pathTagList.Contains(pathTag))
        {
            Highlight(true);
        }
    }

    public void ClearPotential()
    {
        Highlight(false);
    }
}
