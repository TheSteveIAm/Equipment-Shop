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
    IronBar
}

/// <summary>
/// Pseudo-Factory to manage items.
/// I don't even care if this isn't a standard pattern, I love this thing.
/// </summary>
public class ItemFactory : MonoBehaviour
{
    /// <summary>
    /// List of all available items
    /// </summary>
    public Item[] itemListToLoad;
    private Dictionary<ItemCode, Item> itemList = new Dictionary<ItemCode, Item>();

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
            itemList.Add(itemListToLoad[i].itemCode, itemListToLoad[i]);
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
