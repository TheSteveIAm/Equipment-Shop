using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An item describes something that can be picked up and moved around
/// It can potentially be used in a Recipe by being manipulated by crafting Stations
/// Items can be sold to Heroes, who use them for their Quests
/// </summary>
public class Item : MonoBehaviour {

    public ItemCode itemType;
    private Rigidbody body;
    private Collider col;

    /// <summary>
    /// Inform if the item has been touched by the player after coming out of a crafting station
    /// </summary>
    public bool untouched = true;
    

    void Start()
    {
        col = GetComponent<Collider>();
        body = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Allows a character to pickup this item
    /// </summary>
    /// <param name="newParent"></param>
    public void Pickup(Transform newParent)
    {
        transform.parent = newParent;
        transform.rotation = newParent.rotation;
        transform.position = newParent.position;

        body.isKinematic = true;
        col.enabled = false;
        untouched = false;
    }

    /// <summary>
    /// Allows player to drop this item
    /// </summary>
    public void Drop()
    {
        transform.parent = null;
        body.isKinematic = false;
        col.enabled = true;
        body.AddForce(transform.forward * 5f, ForceMode.Impulse);

        //TODO: create a very short "No Pickup" timer after dropping item
    }

    /// <summary>
    /// Get directly given to a station, therefore disappear
    /// </summary>
    public void DirectGive()
    {

    }

    void OnCollisionEnter(Collision col)
    {
        if(untouched && col.gameObject.GetComponent<Player>() != null)
        {
            untouched = false;
        }
    }
}
