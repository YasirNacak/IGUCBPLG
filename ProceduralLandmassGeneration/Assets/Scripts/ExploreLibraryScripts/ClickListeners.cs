using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClickListeners : MonoBehaviour {

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
