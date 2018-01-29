using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour
{

    Movement move;
    Vector3 targetDir;
    public Transform pickupPoint;

    GameObject selectionTarget;
    Item carriedObject;

    // Use this for initialization
    void Start()
    {
        move = GetComponent<Movement>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Interact();
        }
    }

    void Interact()
    {
        Debug.Log("trying to, boss");

        //while carrying an object
        if (carriedObject != null)
        {
            Station selectedStation = selectionTarget.GetComponent<Station>();

            if (selectionTarget != null && selectedStation != null)
            {
                //try to use item on station based on the station's rules

            }
            else
            {
                //drop item
                carriedObject.Drop();
                carriedObject = null;
            }
        }
        else if(selectionTarget != null) //no held item
        {
            Item selectedItem = selectionTarget.GetComponent<Item>();
            Station selectedStation = selectionTarget.GetComponent<Station>();
            if (selectedItem != null)
            {
                //pickup item
                selectedItem.Pickup(pickupPoint);
                carriedObject = selectedItem;

            }
            else if (selectedStation != null)
            {
                //interact with station, no item in hand
            }
        }
    }

    void FixedUpdate()
    {
        targetDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move.MoveDelta(targetDir * 7.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);

        if (selectionTarget == null &&
        (other.GetComponent<Station>() || other.GetComponent<Item>()))
        {
            selectionTarget = other.gameObject;
            Debug.Log(selectionTarget.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (selectionTarget != null)
        {
            selectionTarget = null;
            Debug.Log("Null Target");
        }
    }
}
