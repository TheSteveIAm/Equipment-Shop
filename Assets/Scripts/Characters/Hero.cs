using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Hero : Character
{

    private Inventory inventory;
    private Item carriedItem;
    private NavMeshAgent agent;

    private PointOfInterest[] points;
    //private PointOfInterest currentInterest;

    [Range(1, 4)]
    private float lingerTime = 1f;
    private float lingerTimer;

    private bool lingering;
    private Station currentStation;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        agent = GetComponent<NavMeshAgent>();

        inventory = GetComponent<Inventory>();

        points = FindObjectsOfType<PointOfInterest>();

        //test
        ChoosePointOfInterest(POIType.Item);
    }

    // Update is called once per frame
    void Update()
    {
        if (lingering)
        {
            lingerTimer += Time.deltaTime;

            if (lingerTimer >= lingerTime)
            {
                lingering = false;
                lingerTimer = 0f;
                ChoosePointOfInterest();
            }
            return;
        }

        if (Vector3.Distance(transform.position, agent.destination) <= 1.1f)
        {
            lingering = true;
            //lingerTime = Random.Range(0f, 4f);
            lingerTime = 1;
        }

    }

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

        agent.destination = pointsOfSelectedType[Random.Range(0, pointsOfSelectedType.Count - 1)].transform.position;
    }

    void ChoosePointOfInterest()
    {
        agent.destination = points[Random.Range(0, points.Length - 1)].transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        Station station = other.GetComponent<Station>();
        
        if(station != null)
        {
            currentStation = station;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        currentStation = null;
    }
}