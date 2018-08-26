using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingTracker : MonoBehaviour {

    public bool xPositive;
    public bool zPositive;
    public float xlow, xhi, zlow, zhi;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (xPositive)
        {
            transform.Translate(Random.Range(xlow, xhi), 0, 0);
        }
        else
        {
            transform.Translate(Random.Range(-xlow, -xhi), 0, 0);
        }
        if (zPositive)
        {
            transform.Translate(0, 0, Random.Range(zlow, zhi));
        }
        else
        {
            transform.Translate(0, 0, Random.Range(-zlow, -zhi));
        }
        if (transform.position.x > 250)
        {
            xPositive = false;  
        } else if (transform.position.x < -250)
        {
            xPositive = true;
        }
        if (transform.position.z > 250)
        {
            zPositive = false;
        }
        else if (transform.position.z < -250)
        {
            zPositive = true;
        }
    }
}
