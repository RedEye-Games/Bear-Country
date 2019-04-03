using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButton : MonoBehaviour
{
    public Animator anim;
    public bool settingsOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void ClickSettings ()
    {
        if (settingsOpen == false)
        {
            anim.Play("SettingsButton_Launch");
            settingsOpen = true;
        }
        else
        {
            anim.Play("SettingsButton_Close");
            settingsOpen = false;
        }
    }

}
