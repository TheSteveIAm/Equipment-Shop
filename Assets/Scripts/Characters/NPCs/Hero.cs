using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HeroBrain))]
public class Hero : Character
{

    private Inventory inventory;
    private Item carriedItem;
    public Transform pickupPoint;

    private HeroBrain brain;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        inventory = GetComponent<Inventory>();
        brain = GetComponent<HeroBrain>();

    }

    public void PickupItem(Item item)
    {
        item.Pickup(pickupPoint);
    }
}