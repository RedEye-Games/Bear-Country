using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour
{

    public Canvas loadingScreen;

    public void LoadByIndex(int sceneIndex)
    {
        loadingScreen.enabled = true;
        SceneManager.LoadScene(sceneIndex);
    }
}