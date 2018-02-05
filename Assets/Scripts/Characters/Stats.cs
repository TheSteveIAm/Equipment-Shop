using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public bool isPlayer;
    private int gold;
    private int health, strength, intelligence, dexterity;
    private int level, experience;

    public int Gold
    {
        get { return gold; }
    }

    /// <summary>
    /// Send a message out when the player's gold has changed, to update UI
    /// </summary>
    public delegate void GoldChangeDelegate();
    public static event GoldChangeDelegate OnGoldChange;

    /// <summary>
    /// Spends gold, doesn't allow spending more gold than owned!
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public bool SpendGold(int amount)
    {
        if (amount >= gold)
        {
            gold -= amount;
            ChangeGold();
            return true;
        }

        return false;
    }

    /// <summary>
    /// Receieves gold from another source, doesn't allow receiving negative gold!
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public bool ReceiveGold(int amount)
    {
        if (amount >= 0)
        {
            gold += amount;
            ChangeGold();
            return true;
        }
        return false;
    }

    private void ChangeGold()
    {
        if (isPlayer && OnGoldChange != null)
        {
            OnGoldChange();
        }
    }
}
