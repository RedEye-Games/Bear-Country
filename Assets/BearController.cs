using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearController : MonoBehaviour
{
    SpriteRenderer[] childBears;

    // Start is called before the first frame update
    void Start()
    {
        childBears = GetComponentsInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RotateBear()
    {
        var rotation = Quaternion.LookRotation(Vector3.up, Vector3.forward);
        transform.rotation = rotation;
    }

    public void ShowBear()
    {
        foreach (var bear in childBears)
        {
            bear.enabled = true;
        }
    }
}
