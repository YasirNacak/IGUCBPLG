using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneLoader : MonoBehaviour {
	void Start () {
        MapGenerator mapGen = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();
        mapGen.GenerateMap();
    }
}
