using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileClickDetector : MonoBehaviour
{
    GameObject lastHit; // Keep track of last object clicked

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 10)) // or whatever range, if applicable
        {
            if (hit.transform.gameObject == lastHit)
            {
                print("clicked again");
            }
            else
            {
                lastHit = hit.transform.gameObject;
                print("clicked something new");
            }
        }
        else
        {
            print("clicked nothing");
        }
    }
}