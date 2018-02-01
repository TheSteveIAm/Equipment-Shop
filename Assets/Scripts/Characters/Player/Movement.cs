using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Movement module for the player
/// </summary>
public class Movement : MonoBehaviour
{
    //private Animator anim;
    public float speed = 1f;
    public float pushPower = 1f;
    private CharacterController charControl;

    void Start()
    {
        //anim = GetComponentInChildren<Animator>();
        charControl = GetComponent<CharacterController>();
    }

    /// <summary>
    /// Sends movement data to the character controller
    /// </summary>
    /// <param name="dir"></param>
    public void MoveDelta(Vector3 dir)
    {
        charControl.SimpleMove(dir * speed);
        
        if (dir.magnitude > 0.1f)
        {
            transform.LookAt(transform.position + dir);
        }

    }

    //Push rigidbody objects around when the player runs into them
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
            return;

        if (hit.moveDirection.y < -0.3F)
            return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.velocity = pushDir * pushPower;
    }
}
