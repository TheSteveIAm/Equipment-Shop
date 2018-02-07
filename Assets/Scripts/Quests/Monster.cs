using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    public Stats stats;

    private void Start()
    {
        stats = GetComponent<Stats>();
    }


}
