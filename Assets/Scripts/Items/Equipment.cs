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

public class Equipment : Item
{
    public EquipmentInfo info;

    public int minDamage, maxDamage;
    public int armor;
    public int strBonus, intBonus, dexBonus;
    public DamageTypes dmgType;
    public Rarity rarity;

    public EquipType equipmentType;

    protected override void Start()
    {
        base.Start();

        LoadInfo();
    }

    void LoadInfo()
    {
        minDamage = info.minDamage;
        maxDamage = info.maxDamage;
        armor = info.armor;
        strBonus = info.strBonus;
        intBonus = info.intBonus;
        dexBonus = info.dexBonus;
        dmgType = info.dmgType;
        rarity = info.rarity;

        equipmentType = info.equipmentType;
    }
}
