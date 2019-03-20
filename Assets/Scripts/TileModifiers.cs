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

    public IEnumerator Flip()
    {
        selectedTile = gameController.GetComponent<GameController>().selectedTile;
        from = selectedTile.transform.rotation;
        to = from * Quaternion.AngleAxis(180, Vector3.forward);
        rotating = true;
        while (rotating)
        {
            yield return null;
        }
        LegalityCheck("CW");
        StartCoroutine(selectedTile.GetComponent<TileController>().TileLegality());
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
        rotating = true;
        StartCoroutine(RotateCW());
    }

    public void startRotationCCW()
    {
        rotating = true;
        StartCoroutine(RotateCCW());
    }

    public void StartFlip()
    {
        rotating = true;
        StartCoroutine(Flip());
    }

    public void LegalityCheck(string direction)
    {
        selectedTile.GetComponent<TileController>().ClearLegality();
        selectedTile.GetComponent<TileController>().checkingLegalityDirection = direction;
        StartCoroutine(selectedTile.GetComponent<TileController>().TileLegality());
    }
}
