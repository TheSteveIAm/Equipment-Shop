using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterName", menuName = "Monster", order = 2)]
public class MonsterInfo : ScriptableObject
{
    public MonsterStats stats;
    public Reward reward;

}
