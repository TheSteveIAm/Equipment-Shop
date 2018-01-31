using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : MonoBehaviour {

    //reference to the item factory, so we can create items from this station
    protected ItemFactory itemList;

    //Recipes that this station will handle
    public Recipe[] possibleRecipes;

    protected Recipe currentRecipe;

    public Transform itemSpitPosition;

	// Use this for initialization
	void Start () {
        //give each station the ability to access the item factory, for easier item management
        itemList = FindObjectOfType<ItemFactory>();
	}

    /// <summary>
    /// Passes Item to recipe, in each station's own unique way
    /// </summary>
    /// <param name="item"></param>
    public virtual void GiveItem(Item item)
    {
        //currentRecipe.GiveItem(item);
    }

    /// <summary>
    /// Removes selected item from the recipe, if able to
    /// </summary>
    /// <param name="item"></param>
    public virtual void RemoveItem(Item item)
    {
        //currentRecipe.RemoveItem(item);
    }

    public virtual void CreateItem(ItemCode item)
    {
        Item craftedItem = itemList.CreateItem(item);
        craftedItem.transform.position = itemSpitPosition.position;
        craftedItem.GetComponent<Rigidbody>().AddForce(itemSpitPosition.forward * 2f, ForceMode.Impulse);
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