using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
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

    public ItemType itemType;

}
