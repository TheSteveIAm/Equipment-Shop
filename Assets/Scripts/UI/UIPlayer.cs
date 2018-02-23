using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour
{
    public Text goldCount;

    void OnEnable()
    {
        Stats.OnGoldChange += UpdateGold;
    }

    void OnDisable()
    {
        Stats.OnGoldChange -= UpdateGold;
    }

    void UpdateGold(int amount)
    {
        goldCount.text = amount.ToString();
    }
}
