using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpectator : MonoBehaviour {

    Vector3 moveForward;
    float cameraRotate, currentSpeed, speed, dashSpeed;
    bool dashing;

    Transform camera;

    // Use this for initialization
    void Start () {
        camera = Camera.main.transform;
        speed = 50f;
        dashSpeed = 100f;
	}
	
	// Update is called once per frame
	void Update () {

        moveForward = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (moveForward.normalized != Vector3.zero)
        {
            cameraRotate = Mathf.Atan2(moveForward.normalized.x, moveForward.normalized.y) * Mathf.Rad2Deg + camera.eulerAngles.y;
            transform.eulerAngles = Vector3.up * cameraRotate;
        }

        dashing = Input.GetKey(KeyCode.LeftShift);

        if (dashing)
        {
            currentSpeed = dashSpeed * moveForward.normalized.magnitude;
        }
        else
        {
            currentSpeed = speed * moveForward.normalized.magnitude;
        }

        if (moveForward.z >= 0)
        transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);
        else if (moveForward.z < 0)
        transform.Translate(-transform.forward * currentSpeed * Time.deltaTime, Space.World);
    }
}
