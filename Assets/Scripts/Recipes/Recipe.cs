using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Any item required in this recipe
/// Use a list of these for more complex recipes
/// </summary>
public struct ItemRequirement
{
    public ItemCode item;
    public int amount;
    public bool filled;
}

/// <summary>
/// A Recipe is what's used to craft new Items
/// It contains a list of Item Requirements
/// A list of items it has been given to fulfill those requirements
/// And how long it takes to process the recipe to create a new item
/// </summary>
public class Recipe : MonoBehaviour
{

    //List of items given to this receipe (via crafting station)
    public List<Item> givenItems = new List<Item>();
    //List of item requirements this receipe needs to craft an item
    public ItemRequirement[] requirements;
    //Item this recipe produces
    public ItemCode itemCreated;
    //Time it takes to process this recipe (if any)
    public float processTime;
    //Production time put into this recipe
    private float currentProcessingTime;
    //Does this recipe contain all the requirements necessary to craft the item?
    public bool requirementsMet;

    /// <summary>
    /// Gives an item to the recipe, after checking if the recipe accepts it
    /// </summary>
    /// <param name="item"></param>
    public bool GiveItem(Item item)
    {
        //check if given item matches an unfilled requirement
        for(int i = 0; i < requirements.Length; i++)
        {
            if(requirements[i].item == item.itemType && !requirements[i].filled)
            {
                //fill requirement
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Remove an item from this recipe (by pulling it out of a station)
    /// </summary>
    /// <param name="item"></param>
    public void RemoveItem(Item item)
    {
        //remove specified item, then re-evaluate the recipe requirement fulfillment
    }

    /// <summary>
    /// Evaluates the requirements and determines whether or not they've been fulfilled
    /// </summary>
    /// <returns>Whether the recipe can be completed</returns>
    bool CheckRequirements()
    {
        //Key: Item Code, Value: Required Item Count
        Dictionary<ItemCode, int> itemReqCount = new Dictionary<ItemCode, int>();
        //Key: Item Code, Value: Given Item Count
        Dictionary<ItemCode, int> itemCount = new Dictionary<ItemCode, int>();

        //loop through requirements, create required counts of each item code
        for (int i = 0; i < requirements.Length; i++)
        {
            if (!itemReqCount.ContainsKey(requirements[i].item))
            {
                itemReqCount.Add(requirements[i].item, 0);
            }
            else
            {
                itemReqCount[requirements[i].item]++; //TEST THIS! (I've never incremented a number in a dicitonary before)
            }
        }

        //loop through list of given items, count each item code
        for(int i = 0; i < givenItems.Count; i++)
        {
            if (!itemCount.ContainsKey(givenItems[i].itemType))
            {
                itemCount.Add(givenItems[i].itemType, 0);
            }
            else
            {
                itemCount[givenItems[i].itemType]++; //TEST THIS! (I've never incremented a number in a dicitonary before)
            }
        }

        //Compare counts of item codes, if given item counts are not equal to requirement counts, requirements have NOT been met,
        //otherwise they have


        return requirementsMet;
    }

    /// <summary>
    /// Craft the item this recipe is inteded to create
    /// </summary>
    void CompleteRecipe()
    {
        //Create item, place it where it needs to go
        //if item has processing time, add to currentProcessing
    }

}
