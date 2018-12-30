using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileDisbursementController : MonoBehaviour
{

    public GameObject Tile;
    public Button disburseTilesButton;
    public Transform[] tileSpawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        disburseTilesButton.onClick.AddListener(DisburseTiles);
    }

    void OnClick()
    {
        print("Yes.");
    }

    void DisburseTiles()
    {
        for (int i = 0; i < tileSpawnPoints.Length; i++)
        {
            Instantiate(Tile, tileSpawnPoints[i].position, tileSpawnPoints[i].rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
