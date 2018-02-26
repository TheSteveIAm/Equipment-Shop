using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An item describes something that can be picked up and moved around
/// It can potentially be used in a Recipe by being manipulated by crafting Stations
/// Items can be sold to Heroes, who use them for their Quests
/// </summary>
public class Item : MonoBehaviour
{

    public ItemCode itemCode;
    protected Rigidbody body;
    protected Collider col;
    public int dropChance;
    public int cost;
    //private float noTouchTime = 0.2f, noTouchTimer = 0f;
    //private bool untouchable = false;

    /// <summary>
    /// Inform if the item has been touched by the player after coming out of a crafting station
    /// </summary>
    public bool untouched = true;

    protected virtual void Start()
    {
        col = GetComponent<Collider>();
        body = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// This is to ensure the item has references to its components when creating it and the player picking it up at the same time,
    /// the item doesn't have time to call its Start() method otherwise
    /// </summary>
    protected void EnsureComponents()
    {
        if (body == null || col == null)
        {
            Start();
        }
    }

    //private void Update()
    //{
    //    if (untouchable)
    //    {
    //        noTouchTimer += Time.deltaTime;

    //        if (noTouchTimer >= noTouchTime)
    //        {
    //            untouchable = false;
    //            noTouchTimer = 0f;
    //        }
    //    }
    //}

    /// <summary>
    /// Allows a character to pickup this item
    /// </summary>
    /// <param name="newParent"></param>
    public void Pickup(Transform newParent)
    {
        EnsureComponents();
        //if (!untouchable)
        //{
            transform.parent = newParent;
            transform.rotation = newParent.rotation;
            transform.position = newParent.position;

            body.isKinematic = true;
            col.enabled = false;
            untouched = false;
        //}
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
        //untouchable = true;
    }
}
