using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChest : UIBase
{
    public GameObject chestItemPrefab;
    private Transform contentArea;
    private List<UIChestItem> chestItems = new List<UIChestItem>();

    private ItemFactory itemList;

    void OnEnable()
    {
        Chest.OnOpenChest += OpenChest;
    }

    void OnDisable()
    {
        Chest.OnOpenChest -= OpenChest;
    }

    protected override void Start()
    {
        base.Start();

        contentArea = GetComponentInChildren<GridLayoutGroup>().transform;
        itemList = ItemFactory.Instance;
    }

    public void OpenChest(Inventory inventory)
    {
        if(inventory != null)
        {
            EnableUI(true);
            //populate item scroll list
            //TODO: this is a bad way of doing this, items should be added when the chest receives an item
            for (int i = 0; i < inventory.ItemCount(); i++)
            {
                GameObject chestItemGO = Instantiate(chestItemPrefab);
                UIChestItem chestItem = chestItemGO.GetComponent<UIChestItem>();
                chestItem.GetComponentInChildren<Text>().text = itemList.GetItemName(inventory.Items[i]);
                //also set item image
                chestItem.transform.SetParent(contentArea, false);
                chestItem.item = inventory.Items[i];
                chestItems.Add(chestItem);
            }
        }
        else
        {
            CloseChest(ItemCode.None);
        }
    }

    void CloseChest(ItemCode item)
    {
        EnableUI(false);

        //TODO: This is a bad way to do this, but it's low priority, so make it better later!
        for (int i = 0; i < chestItems.Count; i++)
        {
            Destroy(chestItems[i]);
        }

        chestItems.Clear();
    }
}