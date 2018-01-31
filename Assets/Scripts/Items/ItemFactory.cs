using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Codes to represent every item in the game, allowing us to create and reference items by these codes
/// </summary>
public enum ItemCode {
    None,
    IronOre,
    IronBar
}

public class ItemFactory : MonoBehaviour
{
    public Item[] itemListToLoad;
    private Dictionary<ItemCode, Item> itemList = new Dictionary<ItemCode, Item>();

    // Use this for initialization
    void Start()
    {
        //Thought: In the future i might load prefabs from folder, it will all depend on memory/load times

        //load prefabs into dictionary so we can reference an item object by its code
        for (int i = 0; i < itemListToLoad.Length; i++)
        {
            itemList.Add(itemListToLoad[i].itemType, itemListToLoad[i]);
            //Debug.Log(itemListToLoad[i].itemType.ToString());
        }

        CreateItem(ItemCode.IronOre);
    }

    public Item CreateItem(ItemCode item)
    {
        if (itemList.ContainsKey(item)){
            return Instantiate(itemList[item], transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Item Factory did not find item: " + item.ToString());
        }

        return null;
    }

    //public void CreateItem(string itemName)
    //{
    //    if (itemList.ContainsKey(itemName))
    //    {
    //        Instantiate(itemList[itemName], transform.position, Quaternion.identity);
    //    }
    //    else
    //    {
    //        Debug.LogError("ItemFactory did not find item: " + itemName);
    //    }
    //}
}
