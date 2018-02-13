using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipmet", menuName = "Equipment", order = 4)]
public class EquipmentInfo : ScriptableObject {

    public int minDamage, maxDamage;
    public int armor;
    public int strBonus, intBonus, dexBonus;
    public DamageTypes dmgType;
    public Rarity rarity;

    public EquipType equipmentType;

}
