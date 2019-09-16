using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalmonController : MonoBehaviour
{
    SpriteRenderer[] childSalmon;

    // Start is called before the first frame update
    void Start()
    {
        childSalmon = GetComponentsInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RotateSalmon()
    {
        var rotation = Quaternion.LookRotation(Vector3.up, Vector3.forward);
        transform.rotation = rotation;
    }

    public void ShowSalmon()
    {
        RotateSalmon();
        foreach (var salmon in childSalmon)
        {
            salmon.enabled = true;
        }
    }

    public void HideSalmon()
    {
        foreach (var salmon in childSalmon)
        {
            salmon.enabled = false;
        }
    }
}
