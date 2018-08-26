using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorCam : MonoBehaviour
{
    Vector3 camMove;
    public Transform player;
    float sensitivity, speed; 
    float lookVert, lookHori;

    // Use this for initialization
    void Start ()
    {
        sensitivity = 5f;
        transform.position = player.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        // getting the x and y value of the camera when the mouse is moved
        lookHori += Input.GetAxis("Mouse X") * sensitivity;
        lookVert -= Input.GetAxis("Mouse Y") * sensitivity;
               
        // prevents the camera from rotating fully on the x axis
        lookVert = Mathf.Clamp(lookVert, -90, 90);
        
        transform.eulerAngles = new Vector3(lookVert, lookHori);

        transform.position = player.position;

    }
}
