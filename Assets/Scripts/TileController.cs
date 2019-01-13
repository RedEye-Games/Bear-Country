﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileController : MonoBehaviour
{
    // Tile Buttons
    public Button flipButton;
    public Button rotateCWButton;
    public Button rotateCCWButton;
    // how to find canvas so you can fade it on and off
    public GameObject canvas;
    public GameObject theQuad;
    CanvasGroup theGroup;


    // Placement Vars
    bool isArmed = false;
    bool isConfirmed = false;
    Vector3 theSquarePosition;

    // Mouse Drag vars
    private Vector3 screenPoint;
    private Vector3 offset;
    public GameObject lastHit; // Keep track of last object clicked
    public float distance = 100;
    //RaycastHit[] hits;

    // Start is called before the first frame update
    void Start()
    {
        //Announcing the buttons
        flipButton.onClick.AddListener(Flip);
        rotateCWButton.onClick.AddListener(RotateCW);
        rotateCCWButton.onClick.AddListener(RotateCCW);

        // Canvas Group vars
        theGroup = canvas.GetComponent<CanvasGroup>();
        theGroup.alpha = 0;
        theGroup.interactable = false;
    }

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        if (isConfirmed == false)
        {
            ControlsDisable();
            isArmed = true;
            Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
            transform.parent.gameObject.transform.position = cursorPosition;
        }
    }

    private void OnMouseUp()
    {
        if (isArmed)
        {
            transform.parent.gameObject.transform.position = new Vector3(theSquarePosition.x, transform.position.y, theSquarePosition.z);
            ControlsEnable();
            isArmed = false;
        }
    }

    // Tile Controls Enable
    private void ControlsEnable()
    {
        theGroup.alpha = 1;
        theGroup.interactable = true;
        theQuad.SetActive(true);
    }
    // Tile Controls Disable
    private void ControlsDisable()
    {
        theGroup.alpha = 0;
        theGroup.interactable = false;
        theQuad.SetActive(false);
    }

    // Button functions
    void Flip()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, -transform.localScale.z);
    }

    void RotateCW()
    {
        transform.Rotate(0, 90, 0);
    }

    void RotateCCW()
    {
        transform.Rotate(0, -90, 0);
    }

    void Confirm()
    {
        isConfirmed = true;
        ControlsDisable();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.tag != "Tile" && hit.collider.tag != "TileButton")
                {
                    ControlsDisable();
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BoardSpace"))
        {
            GameObject theSquare = other.gameObject;
            theSquarePosition = other.gameObject.transform.position;
            TileCollider tileCollider = theSquare.GetComponent<TileCollider>();
            if (tileCollider.isEmpty == true)
            {
                isArmed = true;
            }
        }
    }
}