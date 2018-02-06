using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The Trade class handles trades between the player and Heroes
/// </summary>
public class Trade
{
    /// <summary>
    /// Reference to the trading characters
    /// </summary>
    public Player player;
    public Hero hero;

    /// <summary>
    /// Offered gold from each side of the trade
    /// </summary>
    private int offeredGold;

    private Item item;

    /// <summary>
    /// Creates a blank trade
    /// </summary>
    public Trade()
    {
    }

    /// <summary>
    /// Creates a new trade
    /// </summary>
    /// <param name="wantedItem"></param>
    /// <param name="offeredGold"></param>
    /// <param name="inquiringHero"></param>
    public Trade(Item wantedItem, int heroGoldOffer, Hero inquiringHero)
    {
        hero = inquiringHero;
        item = wantedItem;
        offeredGold = heroGoldOffer;
    }

    /// <summary>
    /// Confirms and completes the trade
    /// </summary>
    public bool Confirm()
    {
        if (player != null)
        {
            Stats heroStats = hero.GetComponent<Stats>();

            if (heroStats.SpendGold(offeredGold))
            {
                player.GetComponent<Stats>().ReceiveGold(offeredGold);
                hero.AddItemToInventory(item);

                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Denies the offer, causing the hero to leave the item on the trade table
    /// </summary>
    public void DeclineOffer()
    {
        hero.CancelTrade();
    }

    /// <summary>
    /// Player counters the hero's offer, causing the hero to rethink if they want the new offer
    /// </summary>
    public void CounterOffer()
    {

    }
}
