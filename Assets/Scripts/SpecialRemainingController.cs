using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SpecialRemainingController : MonoBehaviour
{
    public int remainingSpecials;
    public GameObject gameController;
    public GameObject remaining01;
    public GameObject remaining02;
    public GameObject remaining03;
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        text.text = "Specials Used: 0/3";
    }

    // Update is called once per frame
    void Update()
    {
        remainingSpecials = gameController.GetComponent<TileDisbursementController>().remainingSpecialTiles;

        if (remainingSpecials == 2)
        {
            remaining01.SetActive(false);
            text.text = "Specials Used: 1/3";
        }
        if (remainingSpecials == 1)
        {
            remaining02.SetActive(false);
            text.text = "Specials Used: 2/3";
        }
        if (remainingSpecials == 0)
        {
            remaining03.SetActive(false);
            text.text = "Specials Used: 3/3";
        }
    }
}
