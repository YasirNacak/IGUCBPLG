using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LastSearchedContainer : MonoBehaviour, IDeselectHandler
{
    private string lastAdded;
    private Deque<GameObject> objList = new Deque<GameObject>();
    private HashSet<string> elementSet = new HashSet<string>();
    public Transform contentPanel;
    public ObjectPoolScript objectPool;
    private const int HISTORY = 5;
    public static GameObject searchContainer;

    void Start()
    {
        searchContainer = GameObject.Find("SearchContainer");
        searchContainer.SetActive(false);
    }

    public void DeActive()
    {
        searchContainer.SetActive(false);
    }

    public void FocusSearchField()
    {
        if (objList.GetSize() != 0)
            searchContainer.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        string lastInput = EntityScrollList.SearchField.text;
        if (!elementSet.Contains(lastInput) && lastInput != "")
        {
            if (objList.GetSize() == HISTORY)
            {
                GameObject retVal = objList.PollLast();
                elementSet.Remove(lastAdded);
                objectPool.ReturnObject(retVal);
            }
            elementSet.Add(lastInput);
            lastAdded = lastInput;
            GameObject obj = objectPool.GetObject();
            obj.transform.SetParent(contentPanel);
            SearchedElementInfo element = obj.GetComponent<SearchedElementInfo>();
            element.Setup(lastInput);
            objList.OfferFirst(obj);
        }
    }
}
