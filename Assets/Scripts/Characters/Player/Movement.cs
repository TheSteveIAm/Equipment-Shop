using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //private Animator anim;
    public float speed = 1f;
    //Rigidbody body;

    //void Start()
    //{
    //    //anim = GetComponentInChildren<Animator>();
    //    //body = GetComponent<Rigidbody>();

    //}

    public void MoveDelta(Vector3 dir)
    {
        //anim.SetFloat("Speed", (restrictMovement) ? 0f : dir.magnitude);

        transform.position += dir * speed * Time.deltaTime;
        //body.MovePosition(body.position + dir * speed * Time.deltaTime);
        if (dir.magnitude > 0.1f)
        {
            transform.LookAt(transform.position + dir);
        }

    }
}
