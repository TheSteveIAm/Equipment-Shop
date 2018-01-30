using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public ItemCode itemType;
    private Rigidbody body;
    private Collider col;

    void Start()
    {
        col = GetComponent<Collider>();
        body = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	//void Update () {
		
	//}

    public void Pickup(Transform newParent)
    {
        transform.parent = newParent;
        transform.rotation = newParent.rotation;
        transform.position = newParent.position;

        body.isKinematic = true;
        col.enabled = false;
    }

    public void Drop()
    {
        transform.parent = null;
        body.isKinematic = false;
        col.enabled = true;
        body.AddForce(transform.forward * 4f, ForceMode.Impulse);
    }
}
