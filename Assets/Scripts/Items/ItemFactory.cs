using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCode { None, IronOre }

public class ItemFactory : MonoBehaviour
{

    public Item[] itemListToLoad;
    private Dictionary<string, Item> itemList = new Dictionary<string, Item>();

    // Use this for initialization
    void Start()
    {
        //Load items into a dictionary so we can reference and do lookups by name
        //This may be completely replaced by enums, but we'll leave it for now as it can be handy for console use
        for (int i = 0; i < itemListToLoad.Length; i++)
        {
            itemList.Add(itemListToLoad[i].itemType.ToString(), itemListToLoad[i]);
            Debug.Log(itemListToLoad[i].itemType.ToString());
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
