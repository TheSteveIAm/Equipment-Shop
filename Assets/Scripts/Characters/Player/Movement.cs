using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //private Animator anim;
    public float speed = 1f;
    public float pushPower = 1f;
    private CharacterController charControl;
    //Rigidbody body;

    void Start()
    {
        //    //anim = GetComponentInChildren<Animator>();
        //body = GetComponent<Rigidbody>();
        charControl = GetComponent<CharacterController>();
    }

    public void MoveDelta(Vector3 dir)
    {
        //anim.SetFloat("Speed", (restrictMovement) ? 0f : dir.magnitude);

        //transform.position += dir * speed * Time.deltaTime;

        charControl.SimpleMove(dir * speed);

        //body.MovePosition(body.position + dir * speed * Time.deltaTime);
        if (dir.magnitude > 0.1f)
        {
            transform.LookAt(transform.position + dir);
        }

    }

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
