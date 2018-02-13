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

            for (int i = 0; i < possibleItems.Length; i++)
            {
                chanceCount -= OnLootRoll(possibleItems[i]);
                if (chanceCount <= rolledNumber)
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

    //FOR TESTING PURPOSES
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RunQuest();
        }
    }

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

        if (completed)
        {
            if(monsterDrops.Count > 0)
            {
                int totalGold = 0;
                int goldShare = 0;
                int totalExp = 0;
                List<ItemCode> earnedItems = new List<ItemCode>();

                for (int i = 0; i < monsterDrops.Count; i++)
                {
                    totalExp += monsterDrops[i].experience;
                    totalGold += monsterDrops[i].gold;

                    if(monsterDrops[i].rolledItem != ItemCode.None)
                    {
                        earnedItems.Add(monsterDrops[i].rolledItem);
                    }
                }

                goldShare = Mathf.RoundToInt(totalGold / heroes.Count);

                for (int i = 0; i < heroes.Count; i++)
                {
                    heroes[i].stats.ReceiveGold(goldShare);
                    heroes[i].stats.EarnExperience(totalExp);

                    if(earnedItems.Count > 0)
                    {
                        heroes[i].AddItemToInventory(earnedItems[0]);
                        earnedItems.RemoveAt(0);
                    }
                }

            }

            if (success)
            {

            }
        }
    }

    public void CalculateBattle()
    {
        if (heroes.Count > 0 && monsters.Count > 0)
        {
            while (!completed)
            {
                for (int i = 0; i < heroes.Count; i++)
                {
                    Stats hero = heroes[i].stats;
                    Monster targetMonster = monsters[Random.Range(0, monsters.Count - 1)];
                    Stats targetMonsterStats = targetMonster.stats;
                    targetMonsterStats.TakeDamage(hero.RollAttack(), hero.damageMod);

                    if (targetMonsterStats.IsDefeated())
                    {
                        monsterDrops.Add(targetMonster.Defeat());
                        monsters.Remove(targetMonster);
                        //TODO: Maybe keep the monsters but in a defeated state somewhere
                    }
                }

                if (monsters.Count == 0)
                {
                    success = true;
                    completed = true;
                    return;
                }

                for (int i = 0; i < monsters.Count; i++)
                {
                    Stats monster = monsters[i].stats;
                    Hero targetHero = heroes[Random.Range(0, heroes.Count - 1)];
                    Stats targetHeroStats = targetHero.stats;
                    targetHeroStats.TakeDamage(monster.RollAttack(), monster.damageMod);

                    int defeatedHeroes = 0;

                    for (int j = 0; j < heroes.Count; j++)
                    {
                        if (heroes[i].stats.IsDefeated())
                        {
                            defeatedHeroes++;
                        }
                    }

                    if (defeatedHeroes >= heroes.Count)
                    {
                        completed = true;
                        success = false;
                        return;
                    }
                }
            }
        }
        else
        {
            Debug.LogError("CRITICAL ERROR: Battle Quest either has 0 heroes or 0 monsters!");
        }
    }

}
