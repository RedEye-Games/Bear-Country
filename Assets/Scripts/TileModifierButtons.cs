using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileModifierButtons : MonoBehaviour
{

    // TileModifiers
    private TileModifiers tileModifiers;

    // Start is called before the first frame update
    void Start()
    {
        // Locate TileModifiers Script
        GameObject TileModifiersObject = GameObject.FindWithTag("TileModifiers");
        if (TileModifiersObject != null)
        {
            tileModifiers = TileModifiersObject.GetComponent<TileModifiers>();
        }
        if (tileModifiers == null)
        {
            Debug.Log("Cannot find 'TileModifiers' script");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Flip()
    {
        tileModifiers.StartFlip();
    }

    public void RotateCW()
    {
        tileModifiers.startRotationCW();
    }

    public void RotateCCW()
    {
        tileModifiers.startRotationCCW();
    }
}
