using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HeroBrain))]
[RequireComponent(typeof(Inventory))]
public class Hero : Character
{

    private Inventory inventory;
    public Item carriedItem;
    public Transform pickupPoint;
    private Trade trade;

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
        carriedItem = item;
    }

    public void AddItemToInventory(Item item)
    {
        inventory.AddItem(item);
        Destroy(item.gameObject);
        carriedItem = null;
        brain.RemoveWantedItem(item.itemType);
        brain.StopTrading();
    }

    public void CancelTrade()
    {
        //The hero just fucking DROPS the item on the ground, lol what a DICK!
        carriedItem.Drop();
        carriedItem = null;
        brain.StopTrading();
    }

    //public 
}