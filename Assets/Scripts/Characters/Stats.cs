using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Modifier
{
    Strong,
    Weak,
    Immune
}

public enum DamageTypes
{
    Physical,
    Arcane,
    Fire,
    Water,
    Lightning,
    Earth,
    Light,
    Dark
}

[System.Serializable]
public struct DamageModifier
{
    public Modifier modifierType;
    public DamageTypes damageType;
}

public class Stats : MonoBehaviour
{
    public bool isPlayer;
    private int gold = 10;
    private int health = 10, strength = 2, intelligence = 2, dexterity = 2;
    //In heroes: experienced is used to gain levels
    //In Monsters: experience is the amount a hero gains when killing it
    private int level = 1, experience = 0;

    public DamageModifier[] damageMods;

    //list of level experience requirements for next level

    public int Gold
    {
        get { return gold; }
    }

    /// <summary>
    /// Set the stats for a character
    /// </summary>
    /// <param name="goldValue"></param>
    /// <param name="healthValue"></param>
    /// <param name="strengthValue"></param>
    /// <param name="intelligenceValue"></param>
    /// <param name="dexterityValue"></param>
    /// <param name="levelValue"></param>
    /// <param name="experienceValue"></param>
    public void SetStats(int goldValue, int healthValue, int strengthValue, int intelligenceValue, int dexterityValue, int levelValue, int experienceValue)
    {
        gold = goldValue;
        health = healthValue;
        strength = strengthValue;
        intelligence = intelligenceValue;
        dexterity = dexterityValue;
        level = levelValue;
        experience = experienceValue;
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
        if (amount <= gold)
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
