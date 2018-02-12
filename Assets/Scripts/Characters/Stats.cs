using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Modifiers to be used in conjunction with DamageTypes, to determine strengths and weaknesses to a specific type
/// </summary>
public enum Modifier
{
    Strong,
    Weak,
    Immune
}

/// <summary>
/// Possible types of damage
/// </summary>
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

/// <summary>
/// Determines stat bonuses when levelling up
/// ordered by index, so:
/// index 0 = level 1 (immediate stat bonuses applied to a character)
/// index 1 = level 2...
/// etc...
/// </summary>
[System.Serializable]
public struct LevelChart
{
    public int health, strength, intelligence, dexterity;
}

/// <summary>
/// Combines Modifiers and DamageTypes to create a damage modifer
/// </summary>
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

    public int Gold
    {
        get { return gold; }
    }

    private int currentHealth = 10;
    private int maxHealth = 3, strength = 1, intelligence = 1, dexterity = 1;
    private int baseDamage = 1, maxBonusDamage = 1, baseDefence = 0;
    //In heroes: experienced is used to gain levels
    //In Monsters: experience is the amount a hero gains when killing it
    private int level = 1, experience = 0;

    /// <summary>
    /// Array of damage types this character interacts differently
    /// example: Weak Fire, means this character takes more damage from fire, etc.
    /// NOTE: there should never be more than 1 modifier for any DamageType!
    /// </summary>
    public DamageModifier[] damageMods;

    /// <summary>
    /// Array of stat bonuses this character will gain each level
    /// index 0 = level 1, index 1 = level 2...
    /// </summary>
    public LevelChart[] statGainsPerLevel;

    /// <summary>
    /// Send a message out when the player's gold has changed, to update UI
    /// </summary>
    public delegate void GoldChangeDelegate();
    public static event GoldChangeDelegate OnGoldChange;

    public void Init()
    {
        GainStats();
        CalculateBases();
    }

    /// <summary>
    /// Assigns damage to this character
    /// </summary>
    /// <param name="amount">amount of damage to take, negative values are healing</param>
    /// <returns>Is this character defeated?</returns>
    public int TakeDamage(int amount, DamageTypes type)
    {
        int actualAmount = amount;

        for (int i = 0; i < damageMods.Length; i++)
        {
            if (type == damageMods[i].damageType)
            {
                switch (damageMods[i].modifierType)
                {
                    case Modifier.Immune:
                        actualAmount = 0;
                        break;

                    case Modifier.Strong:
                        actualAmount = Mathf.RoundToInt(amount / 1.5f);
                        break;

                    case Modifier.Weak:
                        actualAmount = Mathf.RoundToInt(amount * 1.5f);
                        break;
                }
            }
        }

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            //character is defeated
            currentHealth = 0;
        }

        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }

        //character is not defeated
        return actualAmount;
    }

    public bool IsDefeated()
    {
        return (currentHealth <= 0);
    }

    /// <summary>
    /// Calculate base damage and defence from stats
    /// </summary>
    private void CalculateBases()
    {
        baseDamage = (level) + (strength);
        baseDefence = Mathf.FloorToInt(dexterity / 2);
    }

    /// <summary>
    /// give this character experience points, which may level them up and make them stronger
    /// </summary>
    /// <param name="experiencePoints"></param>
    public void EarnExperience(int experiencePoints)
    {
        experience += experiencePoints;

        if (experience >= (level * 100))
        {
            experience -= (level * 100);
            level++;
            GainStats();
        }
    }

    /// <summary>
    /// Upon level up, gain stats based on this character's level chart
    /// </summary>
    private void GainStats()
    {
        if (statGainsPerLevel.Length > 0)
        {
            maxHealth += statGainsPerLevel[level - 1].health;
            strength += statGainsPerLevel[level - 1].strength;
            intelligence += statGainsPerLevel[level - 1].intelligence;
            dexterity += statGainsPerLevel[level - 1].dexterity;
        }
    }

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

    /// <summary>
    /// if this belongs to the player's character, send a message out, to update UI
    /// </summary>
    private void ChangeGold()
    {
        if (isPlayer && OnGoldChange != null)
        {
            OnGoldChange();
        }
    }

    /// <summary>
    /// Set the stats for a character
    /// </summary>
    public void SetStats(int goldValue, int healthValue, int strengthValue, int intelligenceValue, int dexterityValue, int levelValue, int experienceValue)
    {
        gold = goldValue;
        maxHealth = healthValue;
        strength = strengthValue;
        intelligence = intelligenceValue;
        dexterity = dexterityValue;
        level = levelValue;
        experience = experienceValue;
    }
}
