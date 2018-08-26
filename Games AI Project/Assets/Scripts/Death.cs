using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.Rotate(Random.Range(10f, 50f), Random.Range(0f, 360f), Random.Range(10f, 50f));
        transform.GetComponent<Rigidbody>().AddForce(Vector3.up * Random.Range(300f, 600f));
        transform.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
    }
}
