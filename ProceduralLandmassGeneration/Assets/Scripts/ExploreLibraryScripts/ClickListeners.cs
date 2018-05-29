using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickListeners : MonoBehaviour {

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
