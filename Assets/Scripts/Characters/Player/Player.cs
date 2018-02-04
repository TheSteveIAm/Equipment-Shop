using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : Character
{

    Movement move;
    Vector3 targetDir;
    public float speed = 7.5f;
    public Transform pickupPoint;

    GameObject selectionTarget;
    Item carriedObject;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        move = GetComponent<Movement>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Interact();
        }
    }

    /// <summary>
    /// Deals with interacting with objects in the world
    /// </summary>
    void Interact()
    {
        //while carrying an object
        if (carriedObject != null)
        {

            if (selectionTarget != null)
            {
                Station selectedStation = selectionTarget.GetComponent<Station>();

                if (selectedStation != null)
                {
                    if (selectedStation.GiveItem(carriedObject))
                    {
                        carriedObject = null;
                    }
                }
                //try to use item on station based on the station's rules

            }
            else
            {
                //drop item
                carriedObject.Drop();
                carriedObject = null;
                selectionTarget = null;
            }
        }
        else
        {
            if (selectionTarget != null)
            { //no held item
                Item selectedItem = selectionTarget.GetComponent<Item>();
                Station selectedStation = selectionTarget.GetComponent<Station>();

                if (selectedItem != null)
                {
                    //pickup item
                    selectedItem.Pickup(pickupPoint);
                    carriedObject = selectedItem;
                    selectionTarget = null;

                }
                else if (selectedStation != null)
                {
                    //interact with station, no item in hand, will be used to remove selected item from station
                    Item removedItem = selectedStation.RemoveItem();
                    if (removedItem != null)
                    {
                        removedItem.Pickup(pickupPoint);
                        carriedObject = removedItem;
                        selectionTarget = null;
                    }
                }
            }

        }
    }

    void FixedUpdate()
    {
        //this isn't normalized yet, because this method of moving the character causes normalized vectors to act janky
        targetDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move.MoveDelta(targetDir * speed);
    }

    private void OnTriggerStay(Collider other)
    {
        if (selectionTarget == null &&
        (other.GetComponent<Station>() || (other.GetComponent<Item>() && carriedObject == null)))
        {
            selectionTarget = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (selectionTarget != null)
        {
            selectionTarget = null;
        }
    }
}
