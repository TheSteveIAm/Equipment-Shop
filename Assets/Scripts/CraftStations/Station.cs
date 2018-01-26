using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : MonoBehaviour {

    private ItemFactory itemList;

	// Use this for initialization
	void Start () {
        //give each station the ability to access the item factory, for easier item management
        itemList = FindObjectOfType<ItemFactory>();
	}
}
