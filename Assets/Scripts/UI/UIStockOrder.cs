using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStockOrder : UIBase
{
    public List<ItemCode> availableItems = new List<ItemCode>();
    public GameObject stockItemPrefab;
    public Transform catalogArea, cartArea;

    void OnEnable()
    {
        UIStockItem.OnItemSelected += ItemSelected;
    }

    void OnDisable()
    {
        UIStockItem.OnItemSelected -= ItemSelected;
    }

    void ItemSelected(ItemCode item)
    {

    }
}
