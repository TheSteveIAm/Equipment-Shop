using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeTable : Station {

    public List<Trade> pendingTrades = new List<Trade>();

    public Trade CreateTrade(Item wantedItem, int heroGoldOffer, Hero inquiringHero)
    {
        Trade trade = new Trade(wantedItem, heroGoldOffer, inquiringHero);
        trade.player = FindObjectOfType<Player>();
        pendingTrades.Add(trade);

        return trade;
    }

    //The following functions are overridden and left blank, as they will not be used,
    //and I don't want warnings from them

    public override bool GiveItem(Item item)
    {
        return false;
    }

    public override Item RemoveItem(Item item)
    {
        return null;
    }

    public override Item Interact()
    {
        //TODO: Make this bring up trade dialog

        //TEST #1: Successful trade:
        if(pendingTrades.Count > 0)
        {
            pendingTrades[0].Confirm();
            pendingTrades.RemoveAt(0);
        }

        //This is a bit of a hack, since other stations return items, but this one doesn't, we'll just return null. Not my favorite, but whatever!
        return null;
    }
}
