using UnityEngine;
using UnityEngine.UI;

public class MeshClick : MonoBehaviour
{
    private void OnMouseDown()
    {
        GameObject.Find("PopupName").GetComponent<Text>().text = "Nothing Selected";
        GameObject.Find("PopupDescription").GetComponent<Text>().text = "Nothing Selected";
    }

}