using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour
{

    public Canvas activate;
    public GameObject deactivate;

    public void LoadByIndex(int sceneIndex)
    {
        if (activate != null)
        {
            activate.enabled = true;
        }
        if (deactivate !=null)
        {
            deactivate.SetActive(false);
        }
        SceneManager.LoadScene(sceneIndex);
    }
}