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
    //Recipe Info (See above)
    public RecipeInfo info;
    //List of items given to this receipe (via crafting station)
    public List<Item> givenItems = new List<Item>();
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
        for (int i = 0; i < info.requirements.Length; i++)
        {
            if (info.requirements[i].item == item.itemCode && !info.requirements[i].filled)
            {
                //fill requirement
                info.requirements[i].filled = true;
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
        for (int i = 0; i < info.requirements.Length; i++)
        {
            if (item.itemCode == info.requirements[i].item && info.requirements[i].filled)
            {
                info.requirements[i].filled = false;
                CheckRequirements();
                return item.itemCode;
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

        for (int i = 0; i < info.requirements.Length; i++)
        {
            if (!info.requirements[i].filled)
            {
                requirementsMet = false;
                return;
            }
        }

        requirementsMet = true;

        if (currentProcessingTime >= info.processTime)
        {
            itemReady = true;
        }
    }

    /// <summary>
    /// Process the item closer to completion, this version is to be used every frame,
    /// to update the process by actual time
    /// </summary>
    public void ProcessItem()
    {
        currentProcessingTime += Time.deltaTime;

        if (currentProcessingTime >= info.processTime)
        {
            itemReady = true;
        }
    }

    /// <summary>
    /// Process the item closer to completion, this version is used to manually update
    /// the amount of processed time, and should not be used every frame
    /// </summary>
    /// <param name="time"></param>
    public void ProcessItem(float time)
    {
        currentProcessingTime += time;

        if (currentProcessingTime >= info.processTime)
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

            return info.itemCreated;
        }
        return ItemCode.None;
    }

    #endregion
}
