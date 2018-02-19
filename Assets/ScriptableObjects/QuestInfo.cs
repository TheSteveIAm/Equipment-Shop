using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lv1_Type_Name", menuName = "Quest", order = 0)]
public class QuestInfo : ScriptableObject
{
    public int heroCount = 1;
    public List<MonsterInfo> monsters = new List<MonsterInfo>();
    public QuestType questType;
    public Reward questReward;
    public int questLevel = 1;
}
