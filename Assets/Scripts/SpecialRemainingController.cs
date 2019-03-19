using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialRemainingController : MonoBehaviour
{
    public int remainingSpecials;
    public GameObject gameController;
    public GameObject remaining01;
    public GameObject remaining02;
    public GameObject remaining03;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        remainingSpecials = gameController.GetComponent<TileDisbursementController>().remainingSpecialTiles;

        if (remainingSpecials <= 2)
        {
            remaining01.SetActive(false);

        }
        if (remainingSpecials <= 1)
        {
            remaining02.SetActive(false);
        }
        if (remainingSpecials == 0)
        {
            remaining03.SetActive(false);
        }
    }
}
