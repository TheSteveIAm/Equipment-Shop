using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StatsInfo
{
    public int currentHealth;
    public int maxHealth, strength, intelligence, dexterity;
    public int baseDamage, defence;
    public int maxBonusDamage;
    public int level;
    //Note: only used for heroes!
    public int experience, gold;

    //Note: Only used for monsters!
    [Range(0, 100)]
    public int dropChance;

    public List<DamageModifier> defenceMods;
    public DamageTypes damageMod;
}