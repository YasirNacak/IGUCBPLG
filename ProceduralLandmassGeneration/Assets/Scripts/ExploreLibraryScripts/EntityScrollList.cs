using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityScrollList : MonoBehaviour 
{
    private List<LivingThing> itemList = World.getExplored();
    public Transform contentPanel;
    public ObjectPoolScript entityObject;
    private InputField searchField;
    private List<KeyValuePair<string, GameObject>> pair = new List<KeyValuePair<string, GameObject>>();

    void Start ()
    {
        searchField = GameObject.Find("SearchField").GetComponent<InputField>();
        searchField.onValueChanged.AddListener( delegate { CheckVisibility(); });
        Refresh();
    }

    public void Refresh()
    {
        HeapSort.Sort(itemList);
        AddEntities();
    }

    private void AddEntities()
    {
        pair.Clear();
        for (int i = 0; i < itemList.Count; ++i)
        {
            LivingThing currentItem = itemList[i];
            GameObject newItem = entityObject.GetObject();
            newItem.transform.SetParent(contentPanel);
            EntityInfo element = newItem.GetComponent<EntityInfo>();
            element.Setup(currentItem, this);
            pair.Add(new KeyValuePair<string, GameObject>(currentItem.getName(), newItem));
        }
    }

    public void CheckVisibility()
    {
        string value = searchField.text;
        for (int i = 0; i < pair.Count; ++i)
        {
            if (value.Length <= pair[i].Key.Length)
            {
                if (value.CompareTo(pair[i].Key.Substring(0, value.Length)) != 0)
                    pair[i].Value.SetActive(false);
                else
                    pair[i].Value.SetActive(true);
            }
            else
            {
                pair[i].Value.SetActive(false);
            }
        }
    }
}

