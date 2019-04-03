using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpController : MonoBehaviour
{
    public Button helpOpenButton;
    public Button helpCloseButton;
    public GameObject tutorialPanel;

    // Start is called before the first frame update
    void Start()
    {
        helpOpenButton.onClick.AddListener(OpenTutorial);
        helpCloseButton.onClick.AddListener(CloseTutorial);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OpenTutorial()
    {
        tutorialPanel.SetActive(true);
    }
    void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
    }
}
