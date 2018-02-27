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

    //private UIItem itemUIPrefab;
    //private float noTouchTime = 0.2f, noTouchTimer = 0f;
    //private bool untouchable = false;

    public delegate void ItemPopupDelegate(Transform itemTransform);
    public static event ItemPopupDelegate OnItemPopup;

    public delegate void ItemRemovedDelegate(Transform itemTransform);
    public static event ItemRemovedDelegate OnItemRemoved;

    /// <summary>
    /// Inform if the item has been touched by the player after coming out of a crafting station
    /// </summary>
    public bool untouched = true;

    protected virtual void Start()
    {
        EnsureComponents();
    }

    void OnDestroy()
    {
        if (OnItemRemoved != null)
        {
            OnItemRemoved(transform);
        }
    }

    /// <summary>
    /// This is to ensure the item has references to its components when creating it and the player picking it up at the same time,
    /// the item doesn't have time to call its Start() method otherwise
    /// </summary>
    protected void EnsureComponents()
    {
        if (body == null || col == null)
        {
            col = GetComponent<Collider>();
            body = GetComponent<Rigidbody>();

            ItemFactory itemList = ItemFactory.Instance;

            UIItem itemUIPrefab = Instantiate(itemList.itemUIPrefab);
            itemUIPrefab.AssignUI(transform, itemList.GetItemName(itemCode), cost);
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

        //TODO: FIX THIS
        if (OnItemPopup != null)
        {
            OnItemPopup(null);
        }
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

    void OnTriggerEnter(Collider col)
    {
        Player player = col.GetComponent<Player>();

        if (player != null && !player.CarryingObject())
        {
            if (OnItemPopup != null)
            {
                OnItemPopup(transform);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        Player player = col.GetComponent<Player>();

        if (player != null)
        {
            if (OnItemPopup != null)
            {
                OnItemPopup(null);
            }
        }
    }
}
