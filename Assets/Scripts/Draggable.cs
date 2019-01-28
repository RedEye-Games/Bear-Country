using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;
    public float distance = 100;
    public bool isPlaced = false;
    public bool isConfirmed = false;
    public bool isArmed = false;

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        //if (isPlaced && !isConfirmed)
        //{
            //gameController.GetComponent<TileDisbursementController>().UpdatePlaceCount(1);
        //}
    }

    void OnMouseDrag()
    {
        if (isConfirmed == false)
        {
            //if (isPlaced)
            //{
                //gameController.AddScore(4);
            //}
            isArmed = true;
            isPlaced = false;
            Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z - 2);
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
            //transform.SetParent()
            transform.position = cursorPosition;
            //transform.parent.gameObject.transform.position = cursorPosition;
        }
    }

    //void OnMouseUp()
    //{
    //    // Sends tiles back to spawn point

    //    if (isConfirmed == false)
    //    {
    //        if (isArmed && !isPlaced)
    //        {
    //            isArmed = false;
    //            RaycastHit boardCheck;
    //            Ray ray = new Ray(transform.position, -transform.up);
    //            if (Physics.Raycast(ray, out boardCheck, Mathf.Infinity))
    //            {
    //                if (boardCheck.collider.name == "boardSpaceMain" || boardCheck.collider.name == "boardSpaceCollider01")
    //                {
    //                    isPlaced = true;
    //                    gameController.GetComponent<GameController>().selectedTile = gameObject;
    //                    transform.parent.gameObject.transform.position = new Vector3(theSquarePosition.x, transform.position.y, theSquarePosition.z);
    //                    gameController.GetComponent<TileDisbursementController>().UpdatePlaceCount(-1);
    //                }
    //                else
    //                {
    //                    theSquarePosition = spawnPoint;
    //                    transform.parent.gameObject.transform.position = spawnPoint;
    //                }
    //            }
    //            else
    //            {
    //                theSquarePosition = spawnPoint;
    //                transform.parent.gameObject.transform.position = spawnPoint;
    //            }
    //        }
    //    }
    //}
}
