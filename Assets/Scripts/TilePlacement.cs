using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TilePlacement : MonoBehaviour
{

    // Tile Buttons
    public Button rotateClockwiseButton;
   // GameObject gameObject;

    // Placement Vars
    bool tileArmed;
    Vector3 theSquarePosition;

    // Mouse Drag
    private Vector3 screenPoint;
    private Vector3 offset;

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
        transform.position = cursorPosition;
    }

    private void OnMouseUp()
    {
        print("tile released");
        if (tileArmed = true)
        {
            transform.position = new Vector3(theSquarePosition.x, transform.position.y, theSquarePosition.z);
            tileArmed = false;
        }


    }

    // Start is called before the first frame update
    void Start()
    {
        tileArmed = false;
        rotateClockwiseButton.onClick.AddListener(RotateClockwise);
    }

    void RotateClockwise()
    {
        //        transform.rotation.y = transform.rotation.y + 90f;
        print(gameObject.transform.rotation);
        }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BoardSpace"))
        {
            GameObject theSquare = other.gameObject;
            theSquarePosition = other.gameObject.transform.position;
            TileCollider tileCollider = theSquare.GetComponent<TileCollider>();
            if (tileCollider.isEmpty = true)
            {
                tileArmed = true;
                print(theSquarePosition);
            }
        }

    }
}
