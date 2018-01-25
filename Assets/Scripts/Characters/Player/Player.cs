using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour {

    Movement move;

    Vector3 targetDir;

	// Use this for initialization
	void Start () {
        move = GetComponent<Movement>();
	}

    //void Update()
    //{
    //}

    // Update is called once per frame
    void FixedUpdate()
    {
        targetDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move.MoveDelta(targetDir * 7.5f);
    }
}
