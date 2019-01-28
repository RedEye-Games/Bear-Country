//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class TileModifiers : MonoBehaviour
//{
//    // GameController
//    private GameController gameController;
//    private GameObject selectedTile;

//    // Start is called before the first frame update
//    void Start()
//    {
//        // Locate GameController Script
//        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
//        if (gameControllerObject != null)
//        {
//            gameController = gameControllerObject.GetComponent<GameController>();
//        }
//        if (gameController == null)
//        {
//            Debug.Log("Cannot find 'GameController' script");
//        }
//    }

//    public void Flip()
//    {
//        selectedTile = gameController.GetComponent<GameController>().selectedTile;
//        selectedTile.transform.localScale = new Vector3(selectedTile.transform.localScale.x, selectedTile.transform.localScale.y, selectedTile.transform.localScale.z * -1);
//    }

//    public void RotateCW()
//    {
//        selectedTile = gameController.GetComponent<GameController>().selectedTile;
//        selectedTile.transform.Rotate(0, 90, 0);
//    }

//    public void RotateCCW()
//    {
//        selectedTile = gameController.GetComponent<GameController>().selectedTile;
//        selectedTile.transform.Rotate(0, -90, 0);
//    }
//}
