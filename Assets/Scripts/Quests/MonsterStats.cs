using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MonsterStats
{
    public string name;
    public int currentHealth;
    public int maxHealth, strength, intelligence, dexterity;
    public int baseDamage, defence;
    public int maxBonusDamage;
    //In heroes: experienced is used to gain levels
    //In Monsters: experience is the amount a hero gains when killing it
    public int level;

    [Range(0, 100)]
    public int dropChance;

    public DamageModifier[] defenceMods;
    public DamageTypes damageMod;
}
