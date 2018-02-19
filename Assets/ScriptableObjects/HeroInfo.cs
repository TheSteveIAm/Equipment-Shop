using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroName", menuName = "Hero", order = 3)]
public class HeroInfo : ScriptableObject
{
    public new string name;
    public Texture2D image;
    public StatsInfo stats;
    public LevelChart[] statGainsPerLevel;

    public Dictionary<EquipType, ItemCode> equippedItems = new Dictionary<EquipType, ItemCode>();
}
