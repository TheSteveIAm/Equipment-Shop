using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : MonoBehaviour
{
    /// <summary>
    /// reference to the item factory, so we can create items from this station using it
    /// </summary>
    protected ItemFactory itemList;

    /// <summary>
    /// Recipes that this station will handle
    /// Some stations won't use recipes
    /// </summary>
    public Recipe[] possibleRecipes;

    /// <summary>
    /// The active recipe this station is using
    /// </summary>
    protected Recipe currentRecipe;

    /// <summary>
    /// position to spit out created items
    /// Some stations don't use this and instead the player grabs an item directly from it
    /// </summary>
    public Transform itemSpitPosition;

    protected virtual void Start()
    {
        //give each station the ability to access the item factory, for easier item management
        itemList = FindObjectOfType<ItemFactory>();

        if (itemList == null)
        {
            Debug.LogError("CRITICAL ERROR: ItemFactory was not found on " + gameObject.name + "!", this);
        }
    }

    /// <summary>
    /// Passes Item to recipe, in each station's own unique way
    /// </summary>
    /// <param name="item"></param>
    public virtual bool GiveItem(Item item)
    {
        Debug.LogWarning("GiveItem(Item item) not implemented in: " + name, this);
        return false;
        //currentRecipe.GiveItem(item);
    }

    /// <summary>
    /// Removes selected item from the station, if able to
    /// </summary>
    /// <param name="item"></param>
    public virtual void RemoveItem(Item item)
    {
        Debug.LogWarning("RemoveItem(Item item) not implemented in: " + name, this);
        //currentRecipe.RemoveItem(item);
    }

    /// <summary>
    /// Generic remove item function for each station to define
    /// </summary>
    public virtual Item RemoveItem()
    {
        Debug.LogWarning("RemoveItem() not implemented in: " + name, this);
        return null;
    }

    /// <summary>
    /// Generic CreateItem creates an item at the station's "spit" point, if it exists
    /// </summary>
    /// <param name="item"></param>
    public virtual Item CreateItem(ItemCode item)
    {
        if (itemSpitPosition == null)
        {
            Debug.LogWarning("Spit point on " + name + " does not exist!", this);
            return null;
        }

        Item craftedItem = itemList.CreateItem(item);
        craftedItem.transform.position = itemSpitPosition.position;
        craftedItem.GetComponent<Rigidbody>().AddForce(itemSpitPosition.forward * 2f, ForceMode.Impulse);

        return craftedItem;
    }

    /// <summary>
    /// When an item collides with the station, it will attempt to accept it
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionEnter(Collision col)
    {
        Item item = col.gameObject.GetComponent<Item>();

        if (item != null && !item.untouched)
        {
            GiveItem(item);
        }
    }
}