using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HelpController : MonoBehaviour
{
    //public Button helpOpenButton;
    public Button helpOpenButton;
    public Button helpCloseButton;
    public Button helpCloseButton02;
    public GameObject tutorialPanel;
    //Animator anim;

    public ScrollRect myScrollRect;
    public Scrollbar newScrollBar;

    private float timeOpened;

    private readonly string tutorialName = "how to play - plain text";

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
        AnalyticsWrapper.Report.Tutorial.Start(tutorialName, SceneManager.GetActiveScene().name);
        AnalyticsWrapper.Report.ScreenVisit("help screen", SceneManager.GetActiveScene().name);
        tutorialPanel.SetActive(true);
        timeOpened = Time.time;
        //Set scroll to top of page
        myScrollRect.verticalNormalizedPosition = 1;
        //anim.Play("HowToPlay_Open");
    }

    void CloseTutorial()
    {
        AnalyticsWrapper.Report.Tutorial.Complete(tutorialName, SceneManager.GetActiveScene().name, Time.time - timeOpened);
        tutorialPanel.SetActive(false);
    }
}
