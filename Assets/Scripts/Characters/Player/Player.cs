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

    void FixedUpdate()
    {
        //this isn't normalized yet, because this method of moving the character causes normalized vectors to act janky
        targetDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move.MoveDelta(targetDir * speed);
    }

    void DropItem()
    {
        carriedObject.Drop();
        carriedObject = null;
        selectionTarget = null;
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
                    Vector3 toStation = selectedStation.transform.position - transform.position;

                    if (Vector3.Dot(transform.forward, toStation) > 0)
                    {
                        if (selectedStation.GiveItem(carriedObject))
                        {
                            carriedObject = null;
                        }
                    }
                    else
                    {
                        DropItem();
                    }
                }

            }
            else
            {
                DropItem();
            }
        }
        //While not carrying an object
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
                    //interact with station, no item in hand, will be used to remove selected item from station or
                    //activate its own behavior
                    if(selectedStation is TradeTable)
                    {
                        selectedStation.Interact();
                        return;
                    }

                    Item removedItem = selectedStation.Interact();

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

    private void OnTriggerStay(Collider other)
    {
        if (selectionTarget == null &&
        (other.GetComponent<Station>() || (other.GetComponent<Item>() && carriedObject == null)))
        {
            selectionTarget = other.gameObject;
        }

        if (selectionTarget != null && selectionTarget.GetComponent<Station>() && other.GetComponent<Item>())
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
