using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AudioButton : MonoBehaviour
{
    public Sprite AudioOff;
    public Sprite AudioOn;
    public Button button;
    public bool buttonHere = false;

    // Start is called before the first frame update
    void Start()
    {
    }

public void AudioToggle ()
    {
        if (button.image.sprite == AudioOn)
        {
            button.image.sprite = AudioOff;
        }
        else
        {
            button.image.sprite = AudioOn;
        }
    }

    public void EnterButton ()
    {
        buttonHere = true;
        button.interactable = true;
    }
    public void ExitButton()
    {
        buttonHere = false;
        button.interactable = false;
    }
}