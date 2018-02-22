using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITrade : UIBase
{
    private Trade currentTrade;
    public Text itemName, goldOffer;

    void OnEnable()
    {
        TradeTable.OnOpenTrade += GetTrade;
    }

    void OnDisable()
    {
        TradeTable.OnOpenTrade -= GetTrade;
    }

    void Update()
    {
        //TODO: for Gamepad support
    }

    public void GetTrade(Trade trade)
    {
        if (trade != null)
        {
            currentTrade = trade;
            itemName.text = "Item: " + trade.ItemName;
            goldOffer.text = "Offer: " + trade.OfferedGold;
            EnableUI(true);
        }
        else
        {
            currentTrade = null;
            EnableUI(false);
        }
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
