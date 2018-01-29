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

    public virtual void GiveItem(Item item)
    {
        //if this station accepts that item, receive it
    }
}