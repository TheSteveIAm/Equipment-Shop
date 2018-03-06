using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDisplay : Station
{

    public Item displayedItem;

    public override bool GiveItem(Item item)
    {
        if (displayedItem == null)
        {
            displayedItem = item;
            displayedItem.Pickup(itemSpitPosition);
            return true;
        }
        return false;
    }

    public override Item Interact()
    {
        if (displayedItem != null)
        {
            Item itemToReturn = displayedItem;
            displayedItem = null;
            return itemToReturn;
        }
        return null;
    }

    void OnTriggerEnter(Collider col)
    {
        Player player = col.GetComponent<Player>();

        if (player != null && !player.CarryingObject() && displayedItem != null)
        {
            displayedItem.ItemPopup(displayedItem.transform);
        }
    }

    void OnTriggerExit(Collider col)
    {
        Player player = col.GetComponent<Player>();

        if (player != null && displayedItem != null)
        {
            displayedItem.ItemPopup(null);
        }
    }
}
