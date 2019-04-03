using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class SharedGameController : MonoBehaviour
{
    readonly string shareCodeGenerationString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    bool joiningSharedGame;

    public Toggle joinButton;
    public Toggle hostButton;
    public Button startButton;
    //public Button closeButton;
    public GameObject selector;

    private bool isRotating;
    private int rotationDir;
    private int rotationSpeed = 90;
    private Quaternion to;

    private void OnEnable()
    {
        JoinSharedGame(true);
        joinButton.onValueChanged.AddListener(delegate {
            JoinSharedGame(joinButton);
        });
        hostButton.onValueChanged.AddListener(delegate {
            HostSharedGame(joinButton);
        });
        //joinButton.OnSelect.AddListener(JoinSharedGame);
        //hostButton.onClick.AddListener(HostSharedGame);
        //startButton.onClick.AddListener(InitiateSharedGame);
        //closeButton.onClick.AddListener(ClosePanel);
        //joinButton.Select();
        //joinButton.OnSelect(null);
    }

    void Update()
    {
        if (isRotating)
        {
            selector.transform.rotation = Quaternion.RotateTowards(selector.transform.rotation, to, rotationSpeed * 10 * Time.deltaTime);
            if (selector.transform.rotation == to)
            {
                selector.transform.rotation = to;
                isRotating = false;
            }
        }
    }

    public string GenerateShareCode()
    {
        string shareString = "";

        for (int i = 0; i < 5; i++)
        {
            int shareCodeGenerationInt = Random.Range(0, shareCodeGenerationString.Length);
            char newChar = shareCodeGenerationString[shareCodeGenerationInt];
            shareString = shareString + newChar;
        }

        return shareString;
    }

    public void JoinSharedGame(bool state)
    {
        if (state)
        {
            if (!joiningSharedGame)
            {
                joiningSharedGame = true;
                DataHolder.sharedString = null;
                gameObject.GetComponentInChildren<InputField>().text = null;
                gameObject.GetComponentInChildren<InputField>().enabled = true;
                RotateSelector(-180);
            }
        }

    }

    public void HostSharedGame(bool state)
    {
        if (state && joiningSharedGame)
        {
            joiningSharedGame = false;
            DataHolder.sharedString = GenerateShareCode();
            gameObject.GetComponentInChildren<InputField>().text = DataHolder.sharedString;
            gameObject.GetComponentInChildren<InputField>().enabled = false;
            RotateSelector(180);
        }
    }

    public void RotateSelector(int dir)
    {
        isRotating = true;
        Quaternion from = selector.transform.rotation;
        to = from * Quaternion.AngleAxis(dir, Vector3.forward);
    }

    public void InitiateSharedGame()
    {
        if (joiningSharedGame)
        {
            DataHolder.sharedString = gameObject.GetComponentInChildren<InputField>().text.ToUpper();
        }
        SceneManager.LoadScene("BC_MainBoard");
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

}
