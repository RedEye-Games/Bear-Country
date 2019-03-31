using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AudioButton : MonoBehaviour
{
    public Sprite AudioOff;
    public Sprite AudioOn;
    public Button button;
    public Animator anim;
    public Animation buttonAppear;
    public bool buttonHere = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void AudioToggle ()
    {
        if (button.image.sprite == AudioOn)
            button.image.sprite = AudioOff;
        else
        {
            button.image.sprite = AudioOn;
        }
    }

    public void CallButton()
    {
        if (buttonHere == true)
        {
            ExitButton();
        }
        else
        {
            EnterButton();
        }
    }

    public void EnterButton ()
    {
        buttonHere = true;
        anim.Play("AudioButtonEnter");
        button.interactable = true;
    }
    public void ExitButton()
    {
        buttonHere = false;
        anim.Play("AudioButtonExit");
        button.interactable = false;
    }
}