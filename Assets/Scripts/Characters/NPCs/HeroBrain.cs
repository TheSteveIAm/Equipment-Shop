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
    public DamageTypes wantedDamageType;
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

    private ItemFactory itemList;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        points = FindObjectsOfType<PointOfInterest>();
        hero = GetComponent<Hero>();
        itemList = ItemFactory.Instance;

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

                //Debug.Log(CurrentStation);

                if (CurrentStation != null && lingerTimer <= lingerTime)
                {
                    RotateTowards(CurrentStation.transform);

                    if (currentStation.GetType() == typeof(ItemDisplay))
                    {
                        ItemDisplay display = (ItemDisplay)currentStation;

                        if (display.displayedItem != null)
                        {
                            if (display.displayedItem.GetType() == typeof(Equipment))
                            {
                                Equipment displayedEquip = (Equipment)display.displayedItem;
                                bool wantThisitem = false;

                                if (hero.stats.equippedItems.ContainsKey(displayedEquip.info.equipType.slot))
                                {
                                    wantThisitem = CompareItems(displayedEquip.itemCode, hero.stats.equippedItems[displayedEquip.info.equipType.slot]);
                                }
                                else
                                {
                                    wantThisitem = true;
                                }

                                if (wantThisitem && !wantedItems.Contains(displayedEquip.itemCode))
                                {
                                    AddWantedItem(displayedEquip.itemCode);
                                }
                            }

                            for (int i = 0; i < wantedItems.Count; i++)
                            {
                                if (wantedItems[i] == display.displayedItem.itemCode &&
                                    display.displayedItem.cost <= hero.stats.Gold)
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
                    StopLingering();
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
                            lingerTime = Random.Range(10f, 20f);
                            break;
                    }
                }
                break;

            case BrainStates.WaitingToTrade:

                //Debug.Log(string.Format("{0} | {1} | {2} ", currentStation, currentStation.GetType(), currentTrade == null));
                TradeTable table = (TradeTable)currentStation;

                lingerTimer += Time.deltaTime;

                if (lingerTimer >= lingerTime)
                {
                    currentTrade.DeclineOffer();
                    return;
                }

                if (currentStation != null && currentStation.GetType() == typeof(TradeTable) && currentTrade == null)
                {
                    //TODO: create actual gold offer logic
                    currentTrade = table.CreateTrade(hero.carriedItem, itemList.GetItemCost(hero.carriedItem.itemCode), hero);
                }

                if (currentTrade != null && !table.HasTrade(currentTrade))
                {
                    currentTrade = null;
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

    void StopLingering()
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

    bool CompareItems(ItemCode newItem, ItemCode equippedItem)
    {
        if (itemList.CompareEquipment(newItem, equippedItem).itemCode == newItem)
        {
            return true;
        }

        return false;
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

    private void OnTriggerStay(Collider other)
    {
        if (currentStation == null)
        {
            Station station = other.GetComponent<Station>();

            if (station != null)
            {
                currentStation = station;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        currentStation = null;
    }
}
