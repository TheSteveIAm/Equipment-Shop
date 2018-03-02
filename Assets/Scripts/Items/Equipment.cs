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

public enum EquipSlot
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

public enum EquipType
{
    //Weapons
    Melee,
    Magic,
    Ranged,
    //Armor
    Cloth,
    Light,
    Heavy
    //Spells

    //Accessories

    //Potions
}

public enum BaseType
{  
    Weapon,
    Armor
}

public struct EquipmentType
{
    public BaseType baseType;
    public EquipSlot slot;
    public EquipType equipType;
}

public class Equipment : Item
{
    public EquipmentInfo info;

    protected override void Start()
    {
        base.Start();

        itemCode = info.itemCode;
    }
}
