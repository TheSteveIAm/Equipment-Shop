using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BrainStates
{
    Lingering,
    Browsing,
    Moving,
    WaitingToTrade,
    Exiting,
    Questing
}

public class HeroBrain : MonoBehaviour
{
    /// <summary>
    /// List of all points a hero can be interested in
    /// </summary>
    private PointOfInterest[] points;

    /// <summary>
    /// List of items the hero is interested in buying
    /// </summary>
    //public ItemCode[] wantedItems;
    public List<ItemCode> wantedItems = new List<ItemCode>();
    private NavMeshAgent agent;

    private BrainStates state;

    public BrainStates State
    {
        get { return state; }
    }
    private float lingerTime = 1f, lingerTimer = 0f;
    private float rotationSpeed = 6f;

    private Station currentStation;
    private Hero hero;
    private Trade currentTrade;
    private PointOfInterest currentPOI;

    public Station CurrentStation
    {
        get { return currentStation; }
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        points = FindObjectsOfType<PointOfInterest>();
        hero = GetComponent<Hero>();

        //Test Code:
        ChoosePointOfInterest(POIType.Item);
        //End test code
    }

    void Update()
    {

        switch (state)
        {
            case BrainStates.Browsing:

                break;

            case BrainStates.Lingering:

                lingerTimer += Time.deltaTime;

                if (CurrentStation != null && lingerTimer <= lingerTime)
                {
                    RotateTowards(CurrentStation.transform);

                    if (currentStation is ItemDisplay)
                    {
                        ItemDisplay display = (ItemDisplay)currentStation;

                        if (display.displayedItem != null)
                        {
                            for (int i = 0; i < wantedItems.Count; i++)
                            {
                                if (wantedItems[i] == display.displayedItem.itemCode)
                                {
                                    hero.PickupItem(currentStation.Interact());
                                    lingerTimer = 0f;
                                    ChoosePointOfInterest(POIType.Trade);

                                    return;
                                }
                            }
                        }
                    }
                }

                if (lingerTimer >= lingerTime)
                {
                    lingerTimer = 0f;
                    if (Random.Range(1, 11) == 1)
                    {
                        ChoosePointOfInterest(POIType.Door);
                    }
                    else
                    {
                        ChoosePointOfInterest(POIType.Item);
                    }
                }

                break;

            case BrainStates.Moving:
                if (Vector3.Distance(transform.position, agent.destination) <= 1.1f)
                {
                    switch (currentPOI.navPointOfInterest)
                    {
                        case POIType.Door:
                            state = BrainStates.Exiting;
                            break;

                        case POIType.Item:
                            state = BrainStates.Lingering;
                            lingerTime = Random.Range(1f, 5f);
                            break;

                        case POIType.Trade:
                            state = BrainStates.WaitingToTrade;
                            break;
                    }
                }
                break;

            case BrainStates.WaitingToTrade:

                if (currentStation != null && currentStation.GetType() == typeof(TradeTable) && currentTrade == null)
                {
                    TradeTable table = (TradeTable)currentStation;
                    //TODO: create actual gold offer logic
                    currentTrade = table.CreateTrade(hero.carriedItem, 2, hero);
                }
                break;

            case BrainStates.Exiting:
                ExitShop();
                break;

            case BrainStates.Questing:
                hero.currentQuest.RunQuest();

                //For now just remove the quest
                //TODO: hold on to quest to tell player about?
                EnterShop();
                break;
        }
    }

    public void EnterShop()
    {
        //TEMP, TODO: more logic to come when game loop is more built up
        state = BrainStates.Lingering;
    }

    public void ExitShop()
    {
        state = BrainStates.Questing;
    }

    public void StopTrading()
    {
        currentTrade = null;

        if (wantedItems.Count == 0)
        {
            ChoosePointOfInterest(POIType.Door);
        }
        else
        {
            state = BrainStates.Lingering;
        }
    }

    public void AddWantedItem(ItemCode itemType)
    {
        wantedItems.Add(itemType);
    }

    public void RemoveWantedItem(ItemCode itemType)
    {
        wantedItems.Remove(itemType);
    }

    /// <summary>
    /// Chooses a point of interest by type
    /// </summary>
    /// <param name="poi"></param>
    void ChoosePointOfInterest(POIType poi)
    {
        List<PointOfInterest> pointsOfSelectedType = new List<PointOfInterest>();

        for (int i = 0; i < points.Length; i++)
        {
            if (points[i].navPointOfInterest == poi && !points[i].occupied)
            {
                pointsOfSelectedType.Add(points[i]);
            }
        }

        if (currentPOI != null)
        {
            currentPOI.occupied = false;
        }

        if (pointsOfSelectedType.Count == 0)
        {
            state = BrainStates.Lingering;
            return;
        }

        currentPOI = pointsOfSelectedType[Random.Range(0, pointsOfSelectedType.Count)];
        agent.SetDestination(currentPOI.transform.position);
        currentPOI.occupied = true;
        state = BrainStates.Moving;
    }

    /// <summary>
    /// This version chooses a random point of interest
    /// </summary>
    //void ChoosePointOfInterest()
    //{
    //    agent.SetDestination(points[Random.Range(0, points.Length - 1)].transform.position);
    //}

    /// <summary>
    /// Rotates the transform to look at the station they're standing at
    /// </summary>
    /// <param name="target"></param>
    private void RotateTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        Station station = other.GetComponent<Station>();

        if (station != null)
        {
            currentStation = station;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        currentStation = null;
    }
}
