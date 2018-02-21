using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITrade : UIBase
{
    private Trade currentTrade;
    public Text itemName, goldOffer;


    void Update()
    {
        //TODO: for Gamepad support
    }

    public void GetTrade(Trade trade)
    {
        currentTrade = trade;
        itemName.text = "Item: " + trade.ItemName;
        goldOffer.text = "Offer: " + trade.OfferedGold;
    }

    public void ConfirmTrade()
    {
        if (interactable)
        {
            currentTrade.Confirm();
            EnableUI(false);
            currentTrade = null;
        }
    }

    public void DeclineTrade()
    {
        if (interactable)
        {
            currentTrade.DeclineOffer();
            EnableUI(false);
            currentTrade = null;
        }
    }

    public void CounterTrade()
    {
        if (interactable)
        {
            currentTrade.CounterOffer();
        }
    }
}
