using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpController : MonoBehaviour
{
    //public Button helpOpenButton;
    public Button helpOpenButton;
    public Button helpCloseButton;
    public Button helpCloseButton02;
    public GameObject tutorialPanel;
    public GameObject blocker;
    //Animator anim;

    public ScrollRect myScrollRect;
    public Scrollbar newScrollBar;

    // Start is called before the first frame update
    void Start()
    {
        myScrollRect.verticalNormalizedPosition = 1;
        helpOpenButton.onClick.AddListener(OpenTutorial);
        helpCloseButton.onClick.AddListener(CloseTutorial);
        helpCloseButton02.onClick.AddListener(CloseTutorial);

        //anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Cancel"))
        {
            CloseTutorial();
        }
    }

    void OpenTutorial()
    {
        tutorialPanel.SetActive(true);
        //Set scroll to top of page
        myScrollRect.verticalNormalizedPosition = 1;
        //anim.Play("HowToPlay_Open");
        blocker.SetActive(true);

    }
    void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
        blocker.SetActive(false);
    }
}
