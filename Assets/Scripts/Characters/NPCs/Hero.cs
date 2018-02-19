using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HeroBrain))]
[RequireComponent(typeof(Inventory))]
public class Hero : Character
{
    public HeroInfo info;
    public Item carriedItem;
    public Transform pickupPoint;
    public Quest currentQuest;

    private Trade trade;
    private ItemFactory itemList;
    private Inventory inventory;

    private HeroBrain brain;

    // Use this for initialization
    protected override void Start()
    {
        stats = new Stats(info.stats.gold, info.stats.maxHealth, info.stats.strength, info.stats.intelligence, info.stats.dexterity, info.stats.level, info.stats.experience, info.statGainsPerLevel);

        inventory = GetComponent<Inventory>();
        brain = GetComponent<HeroBrain>();
        itemList = ItemFactory.Instance;
    }

    public void PickupItem(Item item)
    {
        item.Pickup(pickupPoint);
        carriedItem = item;
    }

    /// <summary>
    /// Used for trades and dealing with the items when they are physically present
    /// </summary>
    /// <param name="item"></param>
    public void AddItemToInventory(Item item, bool fromTrade)
    {
        inventory.AddItem(item);
        carriedItem = null;

        if (fromTrade)
        {
            ClearWanteditem(item.itemCode);
        }

        if(item.GetType() == typeof(Equipment))
        {
            Equipment equip = (Equipment)item;
            EquipItem(equip.info);
        }

        Destroy(item.gameObject);
    }

    /// <summary>
    /// Used to move items around without having them physically present
    /// </summary>
    /// <param name="item"></param>
    public void AddItemToInventory(ItemCode item)
    {
        inventory.AddItem(item);
    }

    public Item RemoveItemFromInventory(ItemCode item)
    {
        return itemList.CreateItem(inventory.RemoveItem(item));
    }

    public void CancelTrade()
    {
        //The hero just fucking DROPS the item on the ground, lol what a DICK!
        carriedItem.Drop();
        carriedItem = null;
        brain.StopTrading();
    }

    public void EquipItem(EquipmentInfo equip)
    {
        //TODO: compare at some point (probably before buying) the stats of the items, and then decide to equip
        RemoveEquippedItemSlots(equip);
        stats.AddEquipment(equip);
        inventory.RemoveItem(equip.itemCode);
    }

    void ClearWanteditem(ItemCode item)
    {
        brain.RemoveWantedItem(item);
        brain.StopTrading();
    }

    /// <summary>
    /// Checks stats for equipped items that conflict with the one we're adding.
    /// If so, unequip them and return them to the inventory
    /// </summary>
    /// <param name="equip"></param>
    public void RemoveEquippedItemSlots(EquipmentInfo equip)
    {
        //if (stats.HasEquipment(equip))
        //{
        List<ItemCode> returnedItems = stats.RemoveEquipment(equip.equipmentType);

        if (returnedItems.Count > 0)
        {
            for (int i = 0; i < returnedItems.Count; i++)
            {
                AddItemToInventory(returnedItems[i]);
            }
        }
        //}
    }
}