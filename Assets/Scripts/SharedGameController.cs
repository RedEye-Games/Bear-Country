using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SharedGameController : MonoBehaviour
{
    readonly string shareCodeGenerationString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    bool joiningSharedGame;

    public Button joinButton;
    public Button hostButton;
    public Button startButton;

    // Start is called before the first frame update
    void Start()
    {
        JoinSharedGame();
        joinButton.onClick.AddListener(JoinSharedGame);
        hostButton.onClick.AddListener(HostSharedGame);
        startButton.onClick.AddListener(InitiateSharedGame);
    }

    // Update is called once per frame
    void Update()
    {

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

    public void JoinSharedGame()
    {
        joiningSharedGame = true;
        DataHolder.sharedString = null;
        gameObject.GetComponentInChildren<InputField>().text = null;
        gameObject.GetComponentInChildren<InputField>().enabled = true;
    }

    public void HostSharedGame()
    {
        joiningSharedGame = false;
        DataHolder.sharedString = GenerateShareCode();
        gameObject.GetComponentInChildren<InputField>().text = DataHolder.sharedString;
        gameObject.GetComponentInChildren<InputField>().enabled = false;
    }

    public void InitiateSharedGame()
    {
        if (joiningSharedGame)
        {
            DataHolder.sharedString = gameObject.GetComponentInChildren<InputField>().text.ToUpper();
        }
        SceneManager.LoadScene("BC_MainBoard");
    }

}
