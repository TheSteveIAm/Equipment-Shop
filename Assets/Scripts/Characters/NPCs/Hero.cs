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
    public Quest currentQuest;

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
        carriedItem = null;
        brain.RemoveWantedItem(item.itemCode);
        brain.StopTrading();
    }

    public void AddItemToInventory(ItemCode item)
    {
        inventory.AddItem(item);
    }

    public void CancelTrade()
    {
        //The hero just fucking DROPS the item on the ground, lol what a DICK!
        carriedItem.Drop();
        carriedItem = null;
        brain.StopTrading();
    }

    public void EquipItem(Equipment equip)
    {
        RemoveEquippedItem(equip);
        stats.AddEquipment(equip);
    }

    public void RemoveEquippedItem(Equipment equip)
    {
        if (stats.HasEquipment(equip))
        {
            inventory.AddItem(stats.RemoveEquipment(equip));
        }
    }
}