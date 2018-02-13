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

    protected override void Start()
    {
        base.Start();

    }
}
