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

[System.Serializable]
public class Stats
{
    //public StatsInfo info;
    private ItemFactory itemList;

    public bool isPlayer;
    private int gold = 10;

    public int Gold
    {
        get { return gold; }
    }

    private int currentHealth = 3;
    private int maxHealth = 3, strength = 0, intelligence = 0, dexterity = 0;
    private int baseDamage = 1, defence = 0;
    public int maxBonusDamage = 1;
    //In heroes: experienced is used to gain levels
    //In Monsters: experience is the amount a hero gains when killing it
    private int level = 1, experience = 0;

    public Dictionary<EquipType, ItemCode> equippedItems = new Dictionary<EquipType, ItemCode>();

    /// <summary>
    /// Array of damage types this character interacts differently
    /// example: Weak Fire, means this character takes more damage from fire, etc.
    /// NOTE: there should never be more than 1 modifier for any DamageType!
    /// </summary>
    public List<DamageModifier> defenceMods = new List<DamageModifier>();
    public DamageTypes damageMod;
    public DamageTypes defaultDamageType = DamageTypes.Physical;

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

    public Stats()
    {
        Init();
    }

    /// <summary>
    /// Set the stats for a Monster
    /// </summary>
    public Stats(int healthValue, int strengthValue, int intelligenceValue, int dexterityValue, int levelValue)
    {
        maxHealth = healthValue;
        strength = strengthValue;
        intelligence = intelligenceValue;
        dexterity = dexterityValue;
        level = levelValue;
        currentHealth = maxHealth;

        CalculateBases();

        Init();
    }

    /// <summary>
    /// Set the stats for a Hero
    /// </summary>
    public Stats(int goldValue, int healthValue, int strengthValue, int intelligenceValue, int dexterityValue, int levelValue, int experienceValue, LevelChart[] newLevelChart)
    {
        gold = goldValue;
        maxHealth = healthValue;
        strength = strengthValue;
        intelligence = intelligenceValue;
        dexterity = dexterityValue;
        level = levelValue;
        experience = experienceValue;
        currentHealth = maxHealth;

        statGainsPerLevel = newLevelChart;

        CalculateBases();

        Init();
    }

    public void Init()
    {
        itemList = ItemFactory.Instance;
    }

    public int RollAttack()
    {
        return DiceManager.RollDice(baseDamage, baseDamage + maxBonusDamage);
    }

    /// <summary>
    /// Assigns damage to this character
    /// </summary>
    /// <param name="amount">amount of damage to take</param>
    /// <returns>Is this character defeated?</returns>
    public int TakeDamage(int amount, DamageTypes type)
    {
        Debug.Log("damage roll was " + amount);
        int actualAmount = amount;

        for (int i = 0; i < defenceMods.Count; i++)
        {
            if (type == defenceMods[i].damageType)
            {
                switch (defenceMods[i].modifierType)
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

        actualAmount -= defence;

        currentHealth -= ((actualAmount) > 0) ? actualAmount : 0;

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

    /// <summary>
    /// Heals the character for a certain amount
    /// </summary>
    /// <param name="amount"></param>
    public void Heal(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
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
        baseDamage = (level + 1) + (strength);
        defence = Mathf.FloorToInt(dexterity / 2);
    }

    /// <summary>
    /// Adds a piece of equipment to this character's stats, and adds all the bonuses that equipment has
    /// </summary>
    /// <param name="equip"></param>
    public void AddEquipment(EquipmentInfo equip)
    {
        equippedItems.Add(equip.equipmentType, equip.itemCode);

        baseDamage += equip.minDamage;
        maxBonusDamage += equip.maxDamage;
        defence += equip.armor;
        strength += equip.strBonus;
        intelligence += equip.intBonus;
        dexterity += equip.dexBonus;
        damageMod = equip.dmgType;
    }

    /// <summary>
    /// Checks if equipment of existing type is equipped, if so, removes it and returns it to inventory
    /// </summary>
    /// <param name="equip"></param>
    /// <returns></returns>
    public List<ItemCode> RemoveEquipment(EquipType equip)
    {
        List<ItemCode> returnedItems = new List<ItemCode>();

        for (int i = 0; i < equippedItems.Count; i++)
        {
            switch (equip)
            {
                case EquipType.TwoHandWeapon:
                    if (equippedItems.ContainsKey(EquipType.OneHandWeapon))
                    {
                        returnedItems.Add(equippedItems[EquipType.OneHandWeapon]);
                        RemoveStats(EquipType.OneHandWeapon);
                    }

                    if (equippedItems.ContainsKey(EquipType.Shield))
                    {
                        returnedItems.Add(equippedItems[EquipType.Shield]);
                        RemoveStats(EquipType.Shield);
                    }
                    break;

                case EquipType.OneHandWeapon:
                case EquipType.Shield:
                    if (equippedItems.ContainsKey(EquipType.TwoHandWeapon))
                    {
                        returnedItems.Add(equippedItems[EquipType.TwoHandWeapon]);
                        RemoveStats(EquipType.TwoHandWeapon);
                    }
                    break;
            }

            if (equippedItems.ContainsKey(equip))
            {
                //Equipment tempEquip = equip;
                returnedItems.Add(equippedItems[equip]);
                RemoveStats(equip);
            }

        }

        return returnedItems;
    }

    private void RemoveStats(EquipType equip)
    {
        EquipmentInfo equipStats = itemList.GetEquipmentInfo(equippedItems[equip]);

        baseDamage -= equipStats.minDamage;
        maxBonusDamage -= equipStats.maxDamage;
        defence -= equipStats.armor;
        strength -= equipStats.strBonus;
        intelligence -= equipStats.intBonus;
        dexterity -= equipStats.dexBonus;
        damageMod = defaultDamageType;

        equippedItems.Remove(equip);
    }

    /// <summary>
    /// give this character experience points, which may level them up and make them stronger
    /// </summary>
    /// <param name="experiencePoints"></param>
    public void EarnExperience(int experiencePoints)
    {
        experience += experiencePoints;

        if (experience >= ((level + 1) * 100))
        {
            experience -= ((level + 1) * 100);
            level++;
            GainStats();
        }
    }

    /// <summary>
    /// Upon level up, gain stats based on this character's level chart
    /// </summary>
    private void GainStats()
    {
        if (statGainsPerLevel.Length > level - 1)
        {
            maxHealth += statGainsPerLevel[level - 1].health;
            strength += statGainsPerLevel[level - 1].strength;
            intelligence += statGainsPerLevel[level - 1].intelligence;
            dexterity += statGainsPerLevel[level - 1].dexterity;
            currentHealth = maxHealth;
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
}
