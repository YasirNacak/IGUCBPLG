using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeachContainer : MonoBehaviour {
    public static LinkedList<LivingThing> SearchedList { get; set; }
    private GameObject lastSearched;

    void Start()
    {
        lastSearched = GameObject.Find("SearchContainer");
        SearchedList = new LinkedList<LivingThing>();
    }

}
