using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCollider : MonoBehaviour {

    public bool isEmpty = true;
    Color colorStart = Color.red;
    Color colorEnd = Color.green;
    public Renderer rend;


    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.color = Color.red;

    }

    void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag ("Tile") && isEmpty)
        {
            //rend.material.color = Color.green;
        }
    }

    void OnTriggerExit(Collider other)
    {
        //rend.material.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
