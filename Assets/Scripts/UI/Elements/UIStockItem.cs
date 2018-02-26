using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIStockItem : MonoBehaviour {

    public delegate void StockItemSelectedDelegate(UIStockItem stockItem);
    public static event StockItemSelectedDelegate OnItemSelected;

    public ItemCode itemCode;
    public Text nameLabel, costLabel;
    public bool inCart;
    public int cost;

    public void SetName()
    {

    }


    public void SelectItem()
    {
        if(OnItemSelected != null)
        {
            OnItemSelected(this);
        }
    }
}
