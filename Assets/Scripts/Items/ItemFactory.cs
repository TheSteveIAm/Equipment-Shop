using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Codes to represent every item in the game, allowing us to create and reference items by these codes
/// </summary>
public enum ItemCode
{
    None,
    IronOre,
    IronBar,
    IronSword,
    SuperUltraTestSword
}

/// <summary>
/// Pseudo-Factory to manage items.
/// I don't even care if this isn't a standard pattern, I love this thing.
/// </summary>
public class ItemFactory : MonoBehaviour
{
    private static ItemFactory instance = null;

    public static ItemFactory Instance
    {
        get
        {
            return instance;
        }
    }

    public UIItem itemUIPrefab;

    /// <summary>
    /// List of all available items
    /// </summary>
    public Item[] itemListToLoad;
    private Dictionary<ItemCode, Item> itemList = new Dictionary<ItemCode, Item>();

    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        //Thought: In the future i might load prefabs from folder, it will all depend on memory/load times
        //load prefabs into dictionary so we can reference an item object by its code
        //This way of doing it will let us know if an item is missing.
        //All we have to do is attempt to create an item of every code
        //by running through a list of all our ItemCode enum entries
        //example: foreach(ItemCode item in Enum.GetValues(typeof(ItemCode)))
        for (int i = 0; i < itemListToLoad.Length; i++)
        {
            if (itemListToLoad[i].GetType() == typeof(Equipment))
            {
                Equipment equip = (Equipment)itemListToLoad[i];
                itemList.Add(equip.info.itemCode, equip);
            }
            else
            {
                itemList.Add(itemListToLoad[i].itemCode, itemListToLoad[i]);
            }
        }
    }

    void OnEnable()
    {
        Reward.OnLootRoll += GetItemDropChance;
    }

    void OnDisable()
    {
        Reward.OnLootRoll -= GetItemDropChance;
    }

    //TODO: Change this to load data from a ScriptableObject, instead of creating a ton of prefabs

    /// <summary>
    /// Create an item object by its Item Code
    /// </summary>
    /// <param name="item"></param>
    /// <returns>An instance of an item</returns>
    public Item CreateItem(ItemCode item)
    {
        if (itemList.ContainsKey(item))
        {
            return Instantiate(itemList[item], transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("CRITICAL ERROR: Item Factory did not find item: " + item.ToString());
        }

        return null;
    }

    public ItemCode GetRandomItem()
    {
        return (ItemCode)Random.Range(1, System.Enum.GetNames(typeof(ItemCode)).Length);
    }

    /// <summary>
    /// Returns drop chance of specified item
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int GetItemDropChance(ItemCode item)
    {
        return itemList[item].dropChance;
    }

    public string GetItemName(ItemCode item)
    {
        Item selectedItem = itemList[item];
        if (selectedItem != null)
        {
            return selectedItem.name;
        }
        return null;
    }

    public int GetItemCost(ItemCode item)
    {
        Item selectedItem = itemList[item];
        if (selectedItem != null)
        {
            return selectedItem.cost;
        }
        return 0;
    }

    public EquipmentInfo GetEquipmentInfo(ItemCode item)
    {
        Item selectedItem = itemList[item];
        if (selectedItem.GetType() == typeof(Equipment))
        {
            Equipment equip = (Equipment)selectedItem;
            return equip.info;
        }
        return null;
    }

    /// <summary>
    /// compares and returns the equipment info of the better equipment
    /// </summary>
    /// <param name="item1"></param>
    /// <param name="item2"></param>
    /// <returns></returns>
    public EquipmentInfo CompareEquipment(ItemCode item1, ItemCode item2)
    {
        EquipmentInfo equip1 = GetEquipmentInfo(item1);
        EquipmentInfo equip2 = GetEquipmentInfo(item2);
        if (equip1.equipType.baseType == equip2.equipType.baseType)
        {
            int score1 = 0, score2 = 0;

            switch (equip1.equipType.baseType)
            {
                case BaseType.Armor:
                    score1 += equip1.armor + Mathf.FloorToInt(equip1.dexBonus / 2);
                    score2 += equip2.armor + Mathf.FloorToInt(equip2.dexBonus / 2);
                    break;

                case BaseType.Weapon:
                    score1 += equip1.minDamage + equip1.maxDamage + Mathf.FloorToInt(equip1.strBonus / 2);
                    score2 += equip2.minDamage + equip2.maxDamage + Mathf.FloorToInt(equip2.strBonus / 2);
                    break;
            }

            if (score1 > score2)
            {
                return equip1;
            }
            else if (score2 > score1)
            {
                return equip2;
            }
            else if (score1 == score2)
            {
                if (equip1.dmgType.damageType != DamageTypes.Physical &&
                    equip2.dmgType.damageType == DamageTypes.Physical)
                {
                    return equip1;
                }
                else if (equip2.dmgType.damageType != DamageTypes.Physical &&
                         equip1.dmgType.damageType == DamageTypes.Physical)
                {
                    return equip2;
                }
            }
        }

        return null;
    }

#if UNITY_EDITOR
    #region Debug Functions

    /// <summary>
    /// Loop through all Item to check if they exist
    /// Reminder: Do NOT run this in any release
    /// </summary>
    public void DebugCheckAllItems()
    {
        foreach (ItemCode item in System.Enum.GetValues(typeof(ItemCode)))
        {
            if (item != ItemCode.None)
            {
                var newItem = CreateItem(item);
                Debug.Log(newItem.itemCode + " created successfully.");
                Destroy(newItem.gameObject);
            }
        }
    }

    #endregion
#endif

}
