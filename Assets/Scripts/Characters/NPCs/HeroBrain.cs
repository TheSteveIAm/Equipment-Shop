using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class HeroBrain : MonoBehaviour
{
    /// <summary>
    /// List of all points a hero can be interested in
    /// </summary>
    private PointOfInterest[] points;

    /// <summary>
    /// List of items the hero is interested in buying
    /// </summary>
    public ItemCode[] wantedItems;

    private NavMeshAgent agent;

    private bool lingering, waitingToTrade;
    private float lingerTime = 1f, lingerTimer = 0f;
    private float rotationSpeed = 6f;

    private Station currentStation;
    private Hero hero;

    public Station CurrentStation
    {
        get { return currentStation; }
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        points = FindObjectsOfType<PointOfInterest>();
        hero = GetComponent<Hero>();

        //test
        ChoosePointOfInterest();
    }

    void Update()
    {
        if (lingering)
        {
            lingerTimer += Time.deltaTime;

            if (CurrentStation != null && lingerTimer <= lingerTime)
            {
                RotateTowards(CurrentStation.transform);

                if (currentStation is ItemDisplay)
                {
                    ItemDisplay display = (ItemDisplay)currentStation;

                    if (display.displayedItem != null)
                    {
                        for (int i = 0; i < wantedItems.Length; i++)
                        {
                            if (wantedItems[i] == display.displayedItem.itemType)
                            {
                                hero.PickupItem(currentStation.RemoveItem());
                                lingering = false;
                                lingerTimer = 0f;
                                ChoosePointOfInterest(POIType.Trade);
                                waitingToTrade = true;
                                return;
                            }
                        }
                    }
                }
            }

            if (lingerTimer >= lingerTime)
            {
                lingering = false;
                lingerTimer = 0f;
                ChoosePointOfInterest();
            }
        }
        else if (!waitingToTrade && Vector3.Distance(transform.position, agent.destination) <= 1.1f)
        {
            lingering = true;
            lingerTime = Random.Range(1f, 5f);
        }
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
            if (points[i].navPointOfInterest == poi)
            {
                pointsOfSelectedType.Add(points[i]);
            }

        }

        agent.SetDestination(pointsOfSelectedType[Random.Range(0, pointsOfSelectedType.Count - 1)].transform.position);
    }

    /// <summary>
    /// This version chooses a random point of interest
    /// </summary>
    void ChoosePointOfInterest()
    {
        agent.SetDestination(points[Random.Range(0, points.Length - 1)].transform.position);
    }

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
