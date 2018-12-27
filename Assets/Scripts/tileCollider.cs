﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileCollider : MonoBehaviour {

    public bool isEmpty = true;
    Color colorStart = Color.red;
    Color colorEnd = Color.green;
    Renderer rend;


    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
   
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag ("Tile") && isEmpty)
        {
            print("Tile Collided");
            rend.material.color = Color.green;
        }
    }

    void OnTriggerExit(Collider other)
    {
        rend.material.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
