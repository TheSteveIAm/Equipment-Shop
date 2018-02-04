﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDisplay : Station
{

    private Item displayedItem;

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

    public override Item RemoveItem()
    {
        if (displayedItem != null)
        {
            Item itemToReturn = displayedItem;
            displayedItem = null;
            return itemToReturn;
        }
        return null;
    }
}