using UnityEngine;
using UnityEngine.UI;

public class SearchedElementInfo : MonoBehaviour
{
    public Button itemButton;
    public Text itemText;

    public void Setup(string text)
    {
        itemText.text = text;
        itemButton.onClick.AddListener(delegate { EntityOnClick(); });
    }

    public void EntityOnClick()
    {
        EntityScrollList.CheckElements(itemText.text);
        LastSearchedContainer.searchContainer.SetActive(false);
    }
}
