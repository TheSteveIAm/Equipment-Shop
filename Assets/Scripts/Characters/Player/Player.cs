using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour
{

    Movement move;
    Vector3 targetDir;
    public float speed = 7.5f;
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
        //while carrying an object
        if (carriedObject != null)
        {

            if (selectionTarget != null )
            {
                Station selectedStation = selectionTarget.GetComponent<Station>();

                if (selectedStation != null)
                {
                    selectedStation.GiveItem(carriedObject);
                    carriedObject = null;
                }
                //try to use item on station based on the station's rules

            }
            else
            {
                //drop item
                carriedObject.Drop();
                carriedObject = null;
            }
        }
        else if (selectionTarget != null) //no held item
        {
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
            }
        }
    }

    void FixedUpdate()
    {
        targetDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move.MoveDelta(targetDir * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);

        if (selectionTarget == null &&
        (other.GetComponent<Station>() || (other.GetComponent<Item>() && carriedObject == null)))
        {
            selectionTarget = other.gameObject;
            //Debug.Log(selectionTarget.name);
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
