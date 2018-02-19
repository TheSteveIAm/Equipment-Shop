using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Monster
{
    public string name;
    public Stats stats;
    public MonsterInfo info;
    public Reward reward;
    //Percentage, 0-100% chance to drop an item
    [Range(0, 100)]
    public int dropChance;

    public Monster(MonsterInfo newInfo)
    {
        info = newInfo;

        stats = new Stats(info.stats.maxHealth, info.stats.strength, info.stats.intelligence, info.stats.dexterity, info.stats.level);
        reward = info.reward;

        dropChance = info.stats.dropChance;

        stats.defenceMods = info.stats.defenceMods;
        stats.damageMod = info.stats.damageMod;

        //stats.Init();
    }

    public Reward Defeat()
    {
        if (Random.Range(0, 101) <= dropChance)
        {
            reward.RollLoot();
        }
        return reward;
    }

#if UNITY_EDITOR
    #region Debug Functions

    public void TestRewardCounts()
    {
        Dictionary<ItemCode, int> rewardCounts = new Dictionary<ItemCode, int>();

        for (int i = 0; i < 100; i++)
        {
            ItemCode rewardedItem = Defeat().rolledItem;

            if (!rewardCounts.ContainsKey(rewardedItem))
            {
                rewardCounts.Add(rewardedItem, 1);
            }
            else
            {
                rewardCounts[rewardedItem]++;
            }

            reward.rolledItem = ItemCode.None;
        }

        string debugItemCounts = "";

        foreach (ItemCode key in rewardCounts.Keys)
        {
            debugItemCounts += key.ToString() + ": " + rewardCounts[key] + ", ";
        }

        Debug.Log(debugItemCounts);
    }

    #endregion
#endif
}
