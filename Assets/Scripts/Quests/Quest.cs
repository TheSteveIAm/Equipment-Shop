using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Reward
{
    public int experience;
    public ItemCode[] possibleItems;
}

public enum QuestType
{
    Monsters,
    Boss,
    Deliver,
    Escort,
    Rescue
}

public class Quest
{
    public List<Hero> heroes = new List<Hero>();
    public List<Monster> monsters = new List<Monster>();
    public QuestType questType;
    public Reward questReward;

    //Track if the hero has run through the quest, and if they have
    //Was it a success or failure?
    public bool completed, success;

    public void RunQuest()
    {
        //do some fun battle logic of heroes vs monsters!

        if(monsters.Count == 0)
        {
            completed = true;
            success = true;
            //award experience to hero
            //roll items from possible items
        }
    }

}
