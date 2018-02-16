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
    Hunt,
    Boss,
    Deliver,
    Escort,
    Rescue
}

[CreateAssetMenu(fileName = "QuestName", menuName = "Quest", order = 4)]
public class Quest 
{
    //public QuestInfo info;

    public Quest(List<Hero> questHeroes, List<Monster> questMonsters, QuestType type, Reward reward, int level)
    {
        heroes = questHeroes;
        monsters = questMonsters;
        questType = type;
        questReward = reward;
        questLevel = level;
    }

    private List<Hero> heroes = new List<Hero>();
    private List<Monster> monsters = new List<Monster>();
    private int readyCount = 0;
    private QuestType questType;
    private Reward questReward;
    private int questLevel = 1;
    
    private List<Reward> monsterDrops = new List<Reward>();

    public delegate void QuestCompleteDelegate(List<Hero> heroes);
    public static event QuestCompleteDelegate OnQuestComplete;

    //Track if the hero has run through the quest, and if they have
    //Was it a success or failure?
    private bool completed, success;

    public void HeroReady(Hero hero, bool isReady)
    {
        if (heroes.Contains(hero))
        {
            readyCount += (isReady) ? 1 : -1;

            if(readyCount == heroes.Count)
            {
                RunQuest();
            }
        }
    }

    public void RunQuest()
    {
        switch (questType)
        {
            case QuestType.Boss:
            //Same as monsters, but signify on Quest that it's a boss

            case QuestType.Hunt:
                CalculateBattle();
                break;
        }

        if (completed)
        {
            int totalGold = 0;
            int goldShare = 0;
            int totalExp = 0;
            List<ItemCode> earnedItems = new List<ItemCode>();

            if (monsterDrops.Count > 0)
            {

                for (int i = 0; i < monsterDrops.Count; i++)
                {
                    totalExp += monsterDrops[i].experience;
                    totalGold += monsterDrops[i].gold;

                    if (monsterDrops[i].rolledItem != ItemCode.None)
                    {
                        earnedItems.Add(monsterDrops[i].rolledItem);
                    }
                }
            }

            if (success)
            {
                totalGold += questReward.gold;
                totalExp += questReward.experience;

                if (questReward.rolledItem != ItemCode.None)
                {
                    earnedItems.Add(questReward.rolledItem);
                }
            }

            goldShare = Mathf.RoundToInt(totalGold / heroes.Count);

            int heroCount = 0;

            //TODO: figure out how you're going to split the loot between heroes

            //each hero earns their experience and share of gold
            for (int i = 0; i < heroes.Count; i++)
            {
                heroes[i].stats.ReceiveGold(goldShare);
                heroes[i].stats.EarnExperience(totalExp);
                heroes[i].stats.Heal(9999);
            }

            if (earnedItems.Count > 0)
            {
                for (int i = 0; i < earnedItems.Count; i++)
                {
                    heroes[heroCount].AddItemToInventory(earnedItems[0]);
                    heroCount++;
                    if (heroCount >= heroCount - 1)
                    {
                        heroCount = 0;
                    }
                }

                earnedItems.Clear();
            }
        }

        if(OnQuestComplete != null)
        {
            OnQuestComplete(heroes);
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
                    Monster targetMonster = monsters[Random.Range(0, monsters.Count)];
                    Stats targetMonsterStats = targetMonster.stats;
                    int dmg = targetMonsterStats.TakeDamage(hero.RollAttack(), hero.damageMod);

                    Debug.Log(string.Format("{0} hits {1} for {2} {3} Damage!", hero.name, targetMonster.name, dmg, hero.damageMod));

                    if (targetMonsterStats.IsDefeated())
                    {
                        Debug.Log(targetMonster.name + " has been vanquished!");

                        monsterDrops.Add(targetMonster.Defeat());
                        monsters.Remove(targetMonster);
                        //TODO: Maybe keep the monsters but in a defeated state
                    }
                }

                if (monsters.Count == 0)
                {
                    Debug.Log("All monsters have been defeated, Quest success!");
                    success = true;
                    completed = true;
                    return;
                }

                for (int i = 0; i < monsters.Count; i++)
                {
                    Stats monster = monsters[i].stats;
                    Hero targetHero = heroes[Random.Range(0, heroes.Count - 1)];
                    Stats targetHeroStats = targetHero.stats;
                    int dmg = targetHeroStats.TakeDamage(monster.RollAttack(), monster.damageMod);

                    Debug.Log(string.Format("{0} hits {1} for {2} {3} Damage!", monster.name, targetHero.name, dmg, monster.damageMod));

                    int defeatedHeroes = 0;

                    for (int j = 0; j < heroes.Count; j++)
                    {
                        if (heroes[j].stats.IsDefeated())
                        {
                            defeatedHeroes++;

                            Debug.Log(heroes[j].name + " was defeated!");
                        }
                    }

                    if (defeatedHeroes >= heroes.Count)
                    {
                        Debug.Log("All heroes have been defeated, Quest failed!");

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
