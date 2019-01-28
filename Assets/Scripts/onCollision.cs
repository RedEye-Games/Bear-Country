using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("enter");
        Debug.Log(collision.collider);
    }
    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("stay");
    }
    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("exit");
    }
}
