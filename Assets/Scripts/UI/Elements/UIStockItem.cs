using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UIStockItem : MonoBehaviour {

    public delegate void ChestItemSelectedDelegate(ItemCode chestItem);
    public static event ChestItemSelectedDelegate OnItemSelected;

    public ItemCode item;
    public bool inCart;

    public void SelectItem()
    {
        if(OnItemSelected != null)
        {
            OnItemSelected(item);
        }
    }
}
