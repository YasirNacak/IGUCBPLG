using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EntityScrollList : MonoBehaviour
{
    private List<LivingThing> itemList = World.getExplored();
    public Transform contentPanel;
    public ObjectPoolScript entityObject;
    private static List<KeyValuePair<string, GameObject>> pair = new List<KeyValuePair<string, GameObject>>();
    public static InputField SearchField { get; set; }
    public static GameObject Definition { get; set; }
    public static GameObject Header { get; set; }
    public static GameObject NothingSelected { get; set; }

    void Start ()
    {
        SearchField = GameObject.Find("SearchField").GetComponent<InputField>();
        Definition = GameObject.Find("Definition");
        Header = GameObject.Find("Header");
        NothingSelected = GameObject.Find("NothingSelected");
        Definition.SetActive(false);
        Header.SetActive(false);
        NothingSelected.SetActive(true);
        SearchField.onValueChanged.AddListener( delegate { CheckVisibility(); } );
        pair.Clear();
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
            element.Setup(currentItem);
            pair.Add(new KeyValuePair<string, GameObject>(currentItem.getName(), newItem));
        }
    }

    public void CheckVisibility()
    {
        string value = SearchField.text;
        CheckElements(value);
    }

    public static void CheckElements(string value)
    {
        for (int i = 0; i < pair.Count; ++i)
        {
            if (value.Length <= pair[i].Key.Length)
            {
                if (value.ToLower().CompareTo(pair[i].Key.Substring(0, value.Length).ToLower()) != 0)
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

    public void BackToMenu()
    {
        for (int i = 0; i < pair.Count; ++i)
        {
            entityObject.ReturnObject(pair[i].Value);
        }
        SceneManager.LoadScene("Menu");
    }
}