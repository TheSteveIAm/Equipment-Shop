using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStockOrder : UIBase
{
    private ItemFactory itemList;

    public List<ItemCode> availableItems = new List<ItemCode>();
    private List<UIStockItem> catalogItems = new List<UIStockItem>();
    private List<UIStockItem> cartItems = new List<UIStockItem>();

    public GameObject stockItemPrefab;
    public Transform catalogArea, cartArea;
    public Text costLabel;

    private int cartTotal;
    private Stats playerStats;

    void OnEnable()
    {
        UIStockItem.OnItemSelected += ItemSelected;
        StockOrder.OnOpenStock += OpenStockOrder;
    }

    void OnDisable()
    {
        UIStockItem.OnItemSelected -= ItemSelected;
        StockOrder.OnOpenStock -= OpenStockOrder;
    }

    protected override void Start()
    {
        base.Start();

        itemList = ItemFactory.Instance;

        for (int i = 0; i < availableItems.Count; i++)
        {
            catalogItems.Add(CreateItem(availableItems[i], catalogArea));
        }
    }

    UIStockItem CreateItem(ItemCode item, Transform area)
    {
        GameObject catalogItem = Instantiate(stockItemPrefab, area, false);
        UIStockItem newStockitem = catalogItem.GetComponent<UIStockItem>();
        newStockitem.cost = itemList.GetItemCost(item);
        newStockitem.itemCode = item;

        newStockitem.nameLabel.text = itemList.GetItemName(item);
        newStockitem.costLabel.text = newStockitem.cost.ToString();

        if (area == cartArea)
        {
            newStockitem.inCart = true;
        }

        return newStockitem;
    }

    public void AddItemToCatalog(ItemCode item)
    {
        if (!availableItems.Contains(item))
        {
            availableItems.Add(item);

            catalogItems.Add(CreateItem(item, catalogArea));
        }
    }

    void ItemSelected(UIStockItem item)
    {
        if (item.inCart)
        {
            cartItems.Remove(item);
            Destroy(item.gameObject);
        }
        else
        {
            cartItems.Add(CreateItem(item.itemCode, cartArea));
        }
        CalculateCartTotal();
    }

    void CalculateCartTotal()
    {
        cartTotal = 0;

        for (int i = 0; i < cartItems.Count; i++)
        {
            cartTotal += cartItems[i].cost;
        }

        costLabel.text = "Cost: " + cartTotal;
    }

    public void ConfirmOrder()
    {
        if (playerStats.SpendGold(cartTotal))
        {
            StartCoroutine(TimedSpawn(0.4f));
        }
    }

    public IEnumerator TimedSpawn(float spawnCooldown)
    {
        int i = 0;
        while (i < cartItems.Count)
        {
            Item item = itemList.CreateItem(cartItems[i].itemCode);
            item.transform.position = transform.position;
            yield return new WaitForSeconds(spawnCooldown);
            i++;
        }
        ClearCart();
    }

    void ClearCart()
    {
        for (int i = 0; i < cartItems.Count; i++)
        {
            Destroy(cartItems[i].gameObject);
        }

        cartItems.Clear();
        CalculateCartTotal();
        EnableUI(false);
    }

    void OpenStockOrder(Player player)
    {
        if (player != null)
        {
            if (playerStats == null)
            {
                playerStats = player.stats;
            }

            EnableUI(true);
        }
        else
        {
            ClearCart();
        }
    }
}
