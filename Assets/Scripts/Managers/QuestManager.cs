using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {

    private static QuestManager instance = null;

    public static QuestManager Instance
    {
        get
        {
            return instance;
        }
    }

    public List<QuestInfo> availableQuests = new List<QuestInfo>();

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

    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        AssignQuests();
    }

    void AssignQuests()
    {
        for (int i = 0; i < availableHeroes.Count; i++)
        {
            //select quest
            QuestInfo selectedQuest = availableQuests[Random.Range(0, availableQuests.Count - 1)];

            //add available hero
            List<Hero> questHeroes = new List<Hero>();
            questHeroes.Add(availableHeroes[i]);

            //add monsters from QuestInfo
            List<Monster> questMonsters = new List<Monster>();

            for (int j = 0; j < selectedQuest.monsters.Count; j++)
            {
                questMonsters.Add(new Monster(selectedQuest.monsters[j]));
            }

            availableHeroes[i].currentQuest = new Quest(questHeroes, questMonsters, selectedQuest.questType, selectedQuest.questReward, selectedQuest.questLevel);

            heroesOnQuests.Add(availableHeroes[i]);
        }

        availableHeroes.Clear();
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

        AssignQuests();
    }
}
