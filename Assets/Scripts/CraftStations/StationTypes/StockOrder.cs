using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockOrder : Station {

    public delegate void OpenStockOrderDelegate(Player player);
    public static event OpenStockOrderDelegate OnOpenStock;

    void OnTriggerEnter(Collider col)
    {
        Player player = col.GetComponent<Player>();

        if (player != null)
        {
            if (OnOpenStock != null)
            {
                OnOpenStock(player);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        Player player = col.GetComponent<Player>();

        if (player != null)
        {
            if (OnOpenStock != null)
            {
                OnOpenStock(null);
            }
        }
    }
}
