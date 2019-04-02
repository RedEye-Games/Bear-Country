using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareGameButton : MonoBehaviour
{
    public GameObject gameObjectToActivate;

    public void Activate()
    {
        gameObjectToActivate.SetActive(true);
    }
}
