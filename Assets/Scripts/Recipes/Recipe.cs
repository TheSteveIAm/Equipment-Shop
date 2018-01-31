using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Any item required in this recipe
/// Use a list of these for more complex recipes
/// </summary>
[System.Serializable]
public struct ItemRequirement
{
    public ItemCode item;
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
    #region Properties
    //List of items given to this receipe (via crafting station)
    public List<Item> givenItems = new List<Item>();
    //List of item requirements this receipe needs to craft an item
    public ItemRequirement[] requirements;
    //Item this recipe produces
    public ItemCode itemCreated;
    //Time it takes to process this recipe (if any)
    public float processTime = 0;
    //Production time put into this recipe
    private float currentProcessingTime = 0;
    //Does this recipe contain all the requirements necessary to craft the item?
    public bool requirementsMet;
    //Item will be set to ready when processing is complete
    public bool itemReady;
    #endregion

    #region Functions
    /// <summary>
    /// Gives an item to the recipe, after checking if the recipe accepts it
    /// </summary>
    /// <param name="item"></param>
    public bool GiveItem(Item item)
    {
        //check if given item matches an unfilled requirement
        for (int i = 0; i < requirements.Length; i++)
        {
            if (requirements[i].item == item.itemType && !requirements[i].filled)
            {
                //fill requirement
                requirements[i].filled = true;
                Destroy(item.gameObject);
                CheckRequirements();
                //Tell craft station fulfillment was successful
                return true;
            }
        }
        //Tell craft station fulfillment was a failure
        return false;
    }

    /// <summary>
    /// Remove an item from this recipe (by pulling it out of a station)
    /// </summary>
    /// <param name="item"></param>
    public ItemCode RemoveItem(Item item)
    {
        for (int i = 0; i < requirements.Length; i++)
        {
            if (item.itemType == requirements[i].item && requirements[i].filled)
            {
                requirements[i].filled = false;
                CheckRequirements();
                return item.itemType;
            }
        }

        return ItemCode.None;
        //remove specified item, then re-evaluate the recipe requirement fulfillment
    }

    /// <summary>
    /// Evaluates the requirements and determines whether or not they've been fulfilled
    /// </summary>
    /// <returns>Whether the recipe can be completed</returns>
    void CheckRequirements()
    {

        for (int i = 0; i < requirements.Length; i++)
        {
            if (!requirements[i].filled)
            {
                requirementsMet = false;
                return;
            }
        }

        requirementsMet = true;

        if (currentProcessingTime >= processTime)
        {
            itemReady = true;
        }
    }

    public void ProcessItem()
    {
        currentProcessingTime += Time.deltaTime;

        if (currentProcessingTime >= processTime)
        {
            itemReady = true;
        }
    }

    /// <summary>
    /// Craft the item this recipe is inteded to create
    /// </summary>
    public ItemCode CraftItem()
    {

        if (itemReady)
        {
            requirementsMet = false;
            itemReady = false;
            currentProcessingTime = 0;

            return itemCreated;
        }

        return ItemCode.None;
    }
    #endregion

}
