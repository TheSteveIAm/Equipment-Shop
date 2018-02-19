using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterName", menuName = "Monster", order = 2)]
public class MonsterInfo : ScriptableObject
{
    public new string name;
    public Texture2D image;
    public StatsInfo stats;
    public Reward reward;

}
