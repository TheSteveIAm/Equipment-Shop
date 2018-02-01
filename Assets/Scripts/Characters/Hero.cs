using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Character {

    private Inventory inventory;

	// Use this for initialization
	protected override void Start () {
        base.Start();

        inventory = GetComponent<Inventory>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
