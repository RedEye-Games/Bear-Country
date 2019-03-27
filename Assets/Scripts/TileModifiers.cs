using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TileModifiers : MonoBehaviour
{
    // GameController
    private GameController gameController;
    private GameObject selectedTile;

    // Animation
    public Quaternion from;
    public Quaternion to;
    public bool isRotating;
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
        isRotating = false;

    }

    void Update()
    {
        if (isRotating)
        {
            selectedTile.transform.rotation = Quaternion.RotateTowards(selectedTile.transform.rotation, to, 540 * Time.deltaTime);
            if (selectedTile.transform.rotation == to)
            {
                selectedTile.transform.rotation = to;
                isRotating = false;

                StartCoroutine(selectedTile.GetComponent<TileController>().CheckLineage());
            }
        }
    }

    public IEnumerator Flip()
    {
        // Locate path which isn't a dead end. Flip along that axis.
        Vector3 tileAxis = new Vector3(0,0,1);
        selectedTile = gameController.GetComponent<GameController>().selectedTile;
        selectedTile.GetComponent<TileController>().isFlipped = !selectedTile.GetComponent<TileController>().isFlipped;
        List<GameObject> selectedTilePaths = selectedTile.GetComponent<TileController>().pathList;
        foreach (var tilePath in selectedTilePaths)
        {
            if (!tilePath.GetComponent<PathController>().isDeadEnd)
            {
                if (tilePath.GetComponent<PathController>().name == "Path (North)" || tilePath.GetComponent<PathController>().name == "Path (South)")
                {
                    tileAxis = new Vector3(0, 0, 1);
                }
                else
                {
                    tileAxis = new Vector3(1, 0, 0);
                }
            }
        }
        from = selectedTile.transform.rotation;
        to = from * Quaternion.AngleAxis(180, tileAxis);
        selectedTile.GetComponent<TileController>().isLegal = false;
        isRotating = true;
        while (isRotating)
        {
            yield return null;
        }
        LegalityCheck("CW");
        TileEvent(TileEventName.Flipped, gameObject);
    }

    public IEnumerator RotateCW()
    {
        float angle = 90;
        selectedTile = gameController.GetComponent<GameController>().selectedTile;
        if (selectedTile.GetComponent<TileController>().isFlipped)
        {
            angle = -90;
        }
        from = selectedTile.transform.rotation;
        to = from * Quaternion.AngleAxis(angle, Vector3.up);
        selectedTile.GetComponent<TileController>().isLegal = false;
        isRotating = true;
        yield return new WaitUntil(() => !isRotating);
        LegalityCheck("CW");
        TileEvent(TileEventName.Rotated, gameObject);
    }

    public IEnumerator RotateCCW()
    {
        float angle = -90;
        selectedTile = gameController.GetComponent<GameController>().selectedTile;
        if (selectedTile.GetComponent<TileController>().isFlipped)
        {
            angle = 90;
        }
        from = selectedTile.transform.rotation;
        to = from * Quaternion.AngleAxis(angle, Vector3.up);
        selectedTile.GetComponent<TileController>().isLegal = false;
        isRotating = true;
        yield return new WaitUntil(() => !isRotating);
        LegalityCheck("CCW");
        TileEvent(TileEventName.Rotated, gameObject);
    }

    public void startRotationCW()
    {
        if (isRotating == false)
        {
            StartCoroutine(RotateCW());
        }
    }

    public void startRotationCCW()
    {
        if (isRotating == false)
        {
            StartCoroutine(RotateCCW());
        }
    }

    public void StartFlip()
    {
        if (isRotating == false)
        {
            StartCoroutine(Flip());
        }
    }

    public void LegalityCheck(string direction)
    {
        selectedTile.GetComponent<TileController>().ClearLegality();
        selectedTile.GetComponent<TileController>().checkingLegalityDirection = direction;
        StartCoroutine(selectedTile.GetComponent<TileController>().TileLegality());
    }
}
