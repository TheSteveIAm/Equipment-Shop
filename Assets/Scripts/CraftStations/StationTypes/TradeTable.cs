using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeTable : Station
{

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

        if (pendingTrades.Count > 0)
        {
            //TODO: Choice between these options will go in after some UI is implemented.
            //TEST #1: Successful trade:
            pendingTrades[0].Confirm();

            //TEST #2: Decline Trade:
            //pendingTrades[0].DeclineOffer();

            pendingTrades.RemoveAt(0);

            //TODO: Create Counter offer. NOTE: Will be made after some UI is implemented, and after market prices go in
            //TEST #3: Counter Offer
            //counter too high for hero

            //counter acceptable range for hero
        }

        return null;
    }
}
