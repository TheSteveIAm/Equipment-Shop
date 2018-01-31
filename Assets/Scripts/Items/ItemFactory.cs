using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //Load items into a dictionary so we can reference and do lookups by name
        //This may be completely replaced by enums, but we'll leave it for now as it can be handy for console use

        //Thought: In the future i might load prefabs from folder, but I don't think the time spent creating that (again) will be worth it
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
