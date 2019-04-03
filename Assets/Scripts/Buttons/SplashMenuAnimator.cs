using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashMenuAnimator : MonoBehaviour
{
    public Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void SignArrive()
    {
        AnalyticsWrapper.Report.ScreenVisit("new game menu", "main menu");
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
        AnalyticsWrapper.Report.ScreenVisit("shared game menu", "new game menu");
        anim.Play("SharedGameWindow_Open");
    }

    public void SharedGameWindowStandby()
    {
        AnalyticsWrapper.Report.ScreenVisit("new game menu", "shared game menu");
        anim.Play("SharedGameWindow_Standby");
    }

}
