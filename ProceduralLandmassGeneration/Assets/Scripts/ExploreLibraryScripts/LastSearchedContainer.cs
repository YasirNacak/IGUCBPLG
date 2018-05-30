using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LastSearchedContainer : MonoBehaviour, IDeselectHandler
{
    private string lastAdded;
    private LinkedList<GameObject> objList = new LinkedList<GameObject>();
    private HashSet<string> elementSet = new HashSet<string>();
    public Transform contentPanel;
    public ObjectPoolScript objectPool;
    private const int HISTORY = 5;
    public static GameObject searchContainer;

    // Use this for initialization
    void Start()
    {
        searchContainer = GameObject.Find("SearchContainer");
        searchContainer.SetActive(false);
    }

    public void FocusSearchField()
    {
        if (objList.Count != 0)
            searchContainer.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        string lastInput = EntityScrollList.SearchField.text;
        if (!elementSet.Contains(lastInput) && lastInput != "")
        {
            if (objList.Count == HISTORY)
            {
                GameObject retVal = objList.Last.Value;
                objList.RemoveLast();
                elementSet.Remove(lastAdded);
                Destroy(retVal);
            }
            elementSet.Add(lastInput);
            lastAdded = lastInput;
            GameObject obj = objectPool.GetObject();
            obj.transform.SetParent(contentPanel);
            SearchedElementInfo element = obj.GetComponent<SearchedElementInfo>();
            element.Setup(lastInput);
            objList.AddFirst(obj);
            searchContainer.SetActive(false);
        }
    }
}
