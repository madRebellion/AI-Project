using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillBoi : MonoBehaviour {

    //Ray ray = new Ray();
    //RaycastHit hitInfo;
    //LayerMask mask;

    //public float rotateSpeed = 8.0f;

    //float rotationSmoothingModifier;
    //float rotationVelocityModifier = 1;
    //float smoothTime = 0.1f;
    //float angleToLerp;
    
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(transform.forward / 8.7f, Space.World);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(-transform.forward / 17.4f, Space.World);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, -2, 0, Space.World);
        } else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, 2, 0, Space.World);
        }
    }
}
