using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItem : UIBase
{
    public Text nameLabel, costLabel;
    private Transform transformToFollow;
    //later on: attribute fields
    //just for equipment: stats
    bool open;

    void OnEnable()
    {
        Item.OnItemPopup += ShowPopup;
        Item.OnItemRemoved += ItemRemoved;
    }

    void OnDisable()
    {
        Item.OnItemPopup -= ShowPopup;
        Item.OnItemRemoved -= ItemRemoved;
    }

    public void AssignUI(Transform toFollow, string name, int cost)
    {
        nameLabel.text = name;
        costLabel.text = "Cost: " + cost.ToString();
        transformToFollow = toFollow;
    }

    void Update()
    {
        if (transformToFollow != null)
        {
            transform.position = transformToFollow.position + (Vector3.up * 4);
        }
    }

    void ShowPopup(Transform itemTransform)
    {
        if (itemTransform == transformToFollow)
        {
            EnableUI(true);
        }
        else
        {
            EnableUI(false);
        }
    }

    void ItemRemoved(Transform itemTransform)
    {
        if (itemTransform == transformToFollow)
        {
            Destroy(gameObject);
        }
    }

}
