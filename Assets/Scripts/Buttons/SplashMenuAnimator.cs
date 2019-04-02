using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashMenuAnimator : MonoBehaviour
{
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void SignArrive()
    {
        anim.Play("SignArrive");
    }

    public void SignToRight()
    {
        anim.Play("SignToRight");
    }

    public void SignLeave()
    {
        anim.Play("SignLeave");
    }

    public void SignReturn()
    {
        anim.Play("SignReturn");
    }

    public void SharedGameWindowOpen()
    {
        anim.Play("SharedGameWindow_Open");
    }

    public void SharedGameWindowStandby()
    {
        anim.Play("SharedGameWindow_Standby");
    }

}
