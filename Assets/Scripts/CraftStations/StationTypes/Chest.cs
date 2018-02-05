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
        inventory.AddItem(item.itemType);
        Destroy(item.gameObject);
        return true;
    }

    public override Item Interact()
    {
        if (inventory.ItemCount() > 0)
        {
            Item item = CreateItem(inventory.RemoveLastItem());

            return item;
        }

        return null;
    }

    public override Item CreateItem(ItemCode item)
    {
        return itemList.CreateItem(item);
    }
}
