using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droppable : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("col entered " + GetComponent<GridCoordinates>().column + "," + GetComponent<GridCoordinates>().row);
    }
    private void OnCollisionStay(Collision collision)
    {
        //Debug.Log("col stay");
    }
    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("col exit");
    }

    private void OnTriggerEnter(Collider other)
    {
        Highlightable highlightable = GetComponent<Highlightable>();
        //highlightable?.Highlight();
        Debug.Log("trig enter");
    }
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("trig stay");
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("trig exit");
    }
}
