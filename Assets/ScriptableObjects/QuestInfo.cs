using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lv1_Type_Name", menuName = "Quest", order = 0)]
public class QuestInfo : ScriptableObject
{
    public List<Hero> heroes = new List<Hero>();
    //TODO: to be changed to List<MonsterInfo> 
    public List<Monster> monsters = new List<Monster>();
    public QuestType questType;
    public Reward questReward;
    public int questLevel = 1;
}
