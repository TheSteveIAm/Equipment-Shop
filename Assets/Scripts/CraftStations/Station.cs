using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : MonoBehaviour {

    //reference to the item factory, so we can create items from this station
    protected ItemFactory itemList;

    //Recipes that this station will handle
    public Recipe[] possibleRecipes;

    protected Recipe currentRecipe;

	// Use this for initialization
	void Start () {
        //give each station the ability to access the item factory, for easier item management
        itemList = FindObjectOfType<ItemFactory>();
	}

    /// <summary>
    /// Processes items, checks to see if this station will accept it based on recipes
    /// each station will process this in their own way
    /// </summary>
    /// <param name="item"></param>
    public virtual void GiveItem(Item item)
    {
        //this function may be completely useless
        //if this station accepts that item, receive it, and give it to the recipe
        //otherwise, spit it back out
    }

    /// <summary>
    /// When an item collides with the station, it will attempt to accept it
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionEnter(Collision col)
    {
        Item item = col.gameObject.GetComponent<Item>();

        if(item != null)
        {
            GiveItem(item);
        }

        //Thought: it may be possible for a newly created item to be spit out and thrown into another station.
        //it may be worth created a boolean variable on item "newItem", if it's true, stations will not accept the item
    }
}