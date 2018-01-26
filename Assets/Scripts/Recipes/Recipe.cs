using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ItemRequirement
{
    public Item item;
    public int amount;
    public bool filled;
}

public class Recipe : MonoBehaviour {

    //TODO: List of items given

    public Item[] givenItems;
    public ItemRequirement[] requirements;
    public Item itemCreated;

    public void GiveItem(Item item)
    {
        //check if given item matches an unfilled requirement
        //then add item
    }

    public void RemoveItem(Item item)
    {
        
    }

    void CheckRequirements()
    {
        //loop through requirements, check the list of given items
        //does the given items completely fulfill the recipe?
        //then Craft them item!
        for(int i = 0; i < requirements.Length; i++)
        {

        }
    }

    void CraftItem()
    {
        //Create item, place it where it needs to go
    }

}
