using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Reward
{
    public int experience;
    public int gold;
    //Number of times this reward will roll for items.
    //NOTE: If an item is rolled, it should be taken out of of possible items
    //public int itemRolls;

    public ItemCode[] possibleItems;
    public ItemCode rolledItem;

    public delegate int LootRollDelegate(ItemCode item);
    public static event LootRollDelegate OnLootRoll;

    public void RollLoot()
    {

        if (OnLootRoll != null)
        {
            //Totalled
            int chanceCount = 0;

            for (int i = 0; i < possibleItems.Length; i++)
            {
                chanceCount += OnLootRoll(possibleItems[i]);
            }

            int rolledNumber = Random.Range(0, chanceCount + 1);
            //Debug.Log("Chance count: " + chanceCount);

            for(int i = 0; i < possibleItems.Length; i++)
            {
                chanceCount -= OnLootRoll(possibleItems[i]);
                if(chanceCount <= rolledNumber)
                {
                    rolledItem = possibleItems[i];
                    //Debug.Log(rolledItem + ", index number: " + i + ", with a roll of: " + rolledNumber);
                    return;
                }
            }

        }
    }
}

public enum QuestType
{
    Monsters,
    Boss,
    Deliver,
    Escort,
    Rescue
}

public class Quest : MonoBehaviour
{
    public List<Hero> heroes = new List<Hero>();
    public List<Monster> monsters = new List<Monster>();

    public QuestType questType;
    public Reward questReward;

    private List<Reward> monsterDrops = new List<Reward>();

    //Track if the hero has run through the quest, and if they have
    //Was it a success or failure?
    public bool completed, success;

    public void RunQuest()
    {
        //do some fun battle logic of heroes vs monsters!

        switch (questType)
        {
            case QuestType.Boss:
            //Same as monsters, but signify on Quest that it's a boss

            case QuestType.Monsters:
                CalculateBattle();
                break;

        }

        if (monsters.Count == 0)
        {
            completed = true;
            success = true;
            //award experience to hero
            //roll items from possible items
        }
    }

    public void CalculateBattle()
    {

        for (int i = 0; i < heroes.Count; i++)
        {
            //Stats targetMonsterStats;
        }

        for (int i = 0; i < monsters.Count; i++)
        {

        }
    }

}
