using UnityEngine;
using System;
using System.Collections;

public class TileModifiers : MonoBehaviour
{
    // GameController
    private GameController gameController;
    private GameObject selectedTile;

    // Animation
    public Quaternion from;
    public Quaternion to;
    public bool rotating = false;
    private float smooth = 1f;

    public static event Action<TileEventName, GameObject> TileEvent;

    // Start is called before the first frame update
    void Start()
    {
        // Locate GameController Script
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
    }

    void Update()
    {
        if (rotating)
        {
            selectedTile.transform.rotation = Quaternion.Lerp(selectedTile.transform.rotation, to, 10 * smooth * Time.deltaTime);
            if (selectedTile.transform.rotation == to)
            {
                selectedTile.transform.rotation = to;
                rotating = false;
            }
        }
    }

    public void Flip()
    {
        selectedTile = gameController.GetComponent<GameController>().selectedTile;
        selectedTile.transform.localScale = new Vector3(selectedTile.transform.localScale.x, selectedTile.transform.localScale.y, selectedTile.transform.localScale.z * -1);
        LegalityCheck("CW");
        TileEvent(TileEventName.Flipped, gameObject);
    }

    public IEnumerator RotateCW()
    {
        selectedTile = gameController.GetComponent<GameController>().selectedTile;
        from = selectedTile.transform.rotation;
        to = from * Quaternion.AngleAxis(90, Vector3.up);
        rotating = true;
        while (rotating)
        {
            yield return null;
        }
        LegalityCheck("CW");
        TileEvent(TileEventName.Rotated, gameObject);
    }

    public IEnumerator RotateCCW()
    {
        selectedTile = gameController.GetComponent<GameController>().selectedTile;
        from = selectedTile.transform.rotation;
        to = from * Quaternion.AngleAxis(-90, Vector3.up);
        rotating = true;
        while (rotating)
        {
            yield return null;
        }
        LegalityCheck("CCW");
        TileEvent(TileEventName.Rotated, gameObject);
    }

    public void startRotationCW()
    {
        StartCoroutine(RotateCW());
    }

    public void LegalityCheck(string direction)
    {
        selectedTile.GetComponent<TileController>().ClearLegality();
        selectedTile.GetComponent<TileController>().checkingLegalityDirection = direction;
        selectedTile.GetComponent<TileController>().checkingLegality = true;
    }
}
