using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class HeroBrain : MonoBehaviour
{
    private NavMeshAgent agent;

    private PointOfInterest[] points;
    //private PointOfInterest currentInterest;

    private float lingerTime = 1f;
    private float lingerTimer;

    private float rotationSpeed = 6f;

    private bool lingering;

    private Station currentStation;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        points = FindObjectsOfType<PointOfInterest>();

        //test
        ChoosePointOfInterest();

    }

    void Update()
    {
        if (lingering)
        {
            lingerTimer += Time.deltaTime;

            if (currentStation != null)
            {
                RotateTowards(currentStation.transform);
            }

            if (lingerTimer >= lingerTime)
            {
                lingering = false;
                lingerTimer = 0f;
                ChoosePointOfInterest();
            }
            return;
        }

        if (!lingering && Vector3.Distance(transform.position, agent.destination) <= 1.1f)
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
