using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChestItem : MonoBehaviour {

    public delegate Item ChestItemSelectedDelegate(ItemCode chestItem);
    public static event ChestItemSelectedDelegate OnItemSelected;

    public ItemCode item;

    public void SelectItem()
    {
        if(OnItemSelected != null)
        {
            OnItemSelected(item);
        }
    }
}
