using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipmet", menuName = "Equipment", order = 4)]
public class EquipmentInfo : ScriptableObject {

    public Mesh itemModel;
    public Material itemMaterial;

    public int minDamage, maxDamage;
    public int armor;
    public int strBonus, intBonus, dexBonus;
    public DamageModifier dmgType;
    public Rarity rarity;

    public ItemCode itemCode;
    public EquipmentType equipType;
    
}
