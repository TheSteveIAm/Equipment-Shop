using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {

    public List<QuestInfo> availableQuests = new List<QuestInfo>();
    public GameObject monsterTemplate;

    public List<Hero> availableHeroes = new List<Hero>();
    private List<Hero> heroesOnQuests = new List<Hero>();

    void OnEnable()
    {
        Quest.OnQuestComplete += MakeHeroesAvailable;
    }

    void OnDisable()
    {
        Quest.OnQuestComplete -= MakeHeroesAvailable;
    }

     // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void AssignQuests()
    {
        for (int i = 0; i < availableHeroes.Count; i++)
        {
            //TODO: roll quest, and do all this logic
            List<Hero> attendingHeroes = new List<Hero>();
            attendingHeroes.Add(availableHeroes[i]);

            List<Monster> attendingMonsters = new List<Monster>();

            availableHeroes[i].currentQuest = new Quest(attendingHeroes, attendingMonsters, QuestType.Hunt, new Reward(), 1);
        }
    }

    /// <summary>
    /// Adds heroes back to the available pool so they can be assigned another quest
    /// </summary>
    /// <param name="heroes"></param>
    void MakeHeroesAvailable(List<Hero> heroes)
    {
        for (int i = 0; i < heroes.Count; i++)
        {
            if (heroesOnQuests.Contains(heroes[i]))
            {
                Hero tempHero = heroes[i];
                availableHeroes.Add(tempHero);
                heroesOnQuests.Remove(tempHero);
            }
        }
    }
}
