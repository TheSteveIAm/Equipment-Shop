using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Legendary,
    Unique
}

public enum EquipType
{
    Helmet,
    Armor,
    OneHandWeapon,
    TwoHandWeapon,
    Shield,
    Boots,
    Necklace,
    Ring
}

public class Equipment : Item {

    public int minDamage, maxDamage;
    public int armor;
    public int strBonus, intBonus, dexBonus;
    public DamageTypes dmgType;
    public Rarity rarity;

    public EquipType equipmentType;

}
