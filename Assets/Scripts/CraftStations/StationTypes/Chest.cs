using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Station
{

    private Inventory inventory;

    public delegate void OpenChestDelegate(Inventory chestInventory);
    public static event OpenChestDelegate OnOpenChest;

    void OnEnable()
    {
        UIChestItem.OnItemSelected += CreateItem;
    }

    void OnDisable()
    {
        UIChestItem.OnItemSelected -= CreateItem;
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        inventory = GetComponent<Inventory>();
    }

    public override bool GiveItem(Item item)
    {
        inventory.AddItem(item.itemCode);
        Destroy(item.gameObject);
        return true;
    }

    public override Item Interact()
    {
        if (inventory.ItemCount() > 0)
        {
            Item item = CreateItem(inventory.RemoveLastItem());

            return item;
        }

        return null;
    }

    public override Item CreateItem(ItemCode item)
    {
        if (inventory.RemoveItem(item) != ItemCode.None)
        {
            return itemList.CreateItem(item);
        }
        return null;
    }

    void OnTriggerEnter(Collider col)
    {
        Player player = col.GetComponent<Player>();

        if (player != null && !player.CarryingObject())
        {
            if (OnOpenChest != null)
            {
                OnOpenChest(inventory);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        Player player = col.GetComponent<Player>();

        if (player != null)
        {
            if (OnOpenChest != null)
            {
                OnOpenChest(null);
            }
        }
    }
}
