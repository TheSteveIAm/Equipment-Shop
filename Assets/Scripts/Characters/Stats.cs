using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour {

    private int gold;
    private int health, strength, intelligence, dexterity;
    private int level, experience;

    public int Gold
    {
        get
        {
            return gold;
        }

        set
        {
            gold += value;
        }
    }
}
