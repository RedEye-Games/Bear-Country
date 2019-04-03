using UnityEngine.UI;
using UnityEngine;

public class SpecialRemainingController : MonoBehaviour
{
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

    }

    public void UpdateSpecials()
    {
        int remainingSpecials = 3 - gameController.GetComponent<TileDisbursementController>().remainingSpecialTiles;
        text.text = "Specials Used: " + remainingSpecials + "/3";

        // ToDo: Update UI with Visual Indicators
        if (remainingSpecials == 2)
        {
            remaining01.SetActive(false);
        }
        if (remainingSpecials == 1)
        {
            remaining02.SetActive(false);
        }
        if (remainingSpecials == 0)
        {
            remaining03.SetActive(false);
        }
    }
}
