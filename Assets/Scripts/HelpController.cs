using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HelpController : MonoBehaviour
{
    public Button helpOpenButton;
    public Button helpCloseButton;
    public GameObject tutorialPanel;

    private float timeOpened;

    private readonly string tutorialName = "how to play - plain text";

    void Start()
    {
        helpOpenButton.onClick.AddListener(OpenTutorial);
        helpCloseButton.onClick.AddListener(CloseTutorial);
    }

    void OpenTutorial()
    {
        AnalyticsWrapper.Report.Tutorial.Start(tutorialName, SceneManager.GetActiveScene().name);
        AnalyticsWrapper.Report.ScreenVisit("help screen", SceneManager.GetActiveScene().name);
        tutorialPanel.SetActive(true);
        timeOpened = Time.time;
    }

    void CloseTutorial()
    {
        AnalyticsWrapper.Report.Tutorial.Complete(tutorialName, SceneManager.GetActiveScene().name, Time.time - timeOpened);
        tutorialPanel.SetActive(false);
    }
}
