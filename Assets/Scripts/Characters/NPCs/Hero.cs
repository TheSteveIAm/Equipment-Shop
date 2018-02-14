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

        if (item.GetType() == typeof(Equipment))
        {
            EquipItem((Equipment)item);
        }
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
        RemoveEquippedItemSlots(equip);
        stats.AddEquipment(equip);
        inventory.RemoveItem(equip.itemCode);

        equip.gameObject.SetActive(false);
    }

    /// <summary>
    /// Checks stats for equipped items that conflict with the one we're adding.
    /// If so, unequip them and return them to the inventory
    /// </summary>
    /// <param name="equip"></param>
    public void RemoveEquippedItemSlots(Equipment equip)
    {
        //if (stats.HasEquipment(equip))
        //{
        List<Equipment> returnedItems = stats.RemoveEquipment(equip);

        if (returnedItems.Count > 0)
        {
            for (int i = 0; i < returnedItems.Count; i++)
            {
                inventory.AddItem(returnedItems[i]);
            }
        }
        //}
    }
}