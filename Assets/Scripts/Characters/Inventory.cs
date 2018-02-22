using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Each hero has an inventory, as well as the Chest Station
/// </summary>
public class Inventory : MonoBehaviour
{
    private List<ItemCode> items = new List<ItemCode>();

    public List<ItemCode> Items
    {
        get { return items; }
    }

    public int ItemCount()
    {
        return items.Count;
    }

    public void AddItem(Item item)
    {
        items.Add(item.itemCode);
        Destroy(item.gameObject);
    }

    public void AddItem(ItemCode itemCode)
    {
        items.Add(itemCode);
    }

    public ItemCode RemoveItem(ItemCode item)
    {
        //find item by type, if it exists in inventory
        if (items.Contains(item))
        {
            items.Remove(item);
            return item;
        }

        return ItemCode.None;
    }

    public ItemCode RemoveLastItem()
    {
        ItemCode item = items[items.Count - 1];
        items.RemoveAt(items.Count - 1);
        return item;
    }
}
