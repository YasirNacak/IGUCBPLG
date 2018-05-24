using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour {
    public void GoToCreate()
    {
        SceneManager.LoadScene("Main");
    }
    public void GoToLibrary()
    {

    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
