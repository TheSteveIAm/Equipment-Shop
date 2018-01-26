using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory : MonoBehaviour {

    public Item[] itemListToLoad;
    private Dictionary<string, Item> itemList = new Dictionary<string, Item>();

    // Use this for initialization
    void Start()
    {
        //Load items into a dictionary so we can reference and do lookups by name
        for (int i = 0; i < itemListToLoad.Length; i++)
        {
            itemList.Add(itemListToLoad[i].itemName, itemListToLoad[i]);
        }
    }

    public void CreateItem(string itemName)
    {
        if (itemList.ContainsKey(itemName))
        {
        Instantiate(itemList[itemName], transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("ItemFactory did not find item: " + itemName);
        }
    }
}
