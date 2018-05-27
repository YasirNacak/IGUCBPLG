using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClickListeners : MonoBehaviour {
    
    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }


}
