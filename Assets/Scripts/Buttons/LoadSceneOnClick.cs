using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour
{
    public Canvas loadingScreen;

    public void LoadByIndex(int sceneIndex)
    {
        if (loadingScreen != null)
        {
            loadingScreen.enabled = true;
        }
        SceneManager.LoadScene(sceneIndex);
    }
}