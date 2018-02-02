using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Station
{

    private Inventory inventory;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        inventory = GetComponent<Inventory>();
    }

    public override bool GiveItem(Item item)
    {
        inventory.items.Add(item.itemType);
        Destroy(item.gameObject);
        return true;
    }

    public override Item RemoveItem()
    {
        if (inventory.items.Count > 0)
        {
            Item item = CreateItem(inventory.items[inventory.items.Count - 1]);

            inventory.items.RemoveAt(inventory.items.Count - 1);

            return item;
        }

        return null;
    }

    public override Item CreateItem(ItemCode item)
    {
        return itemList.CreateItem(item);
    }
}
