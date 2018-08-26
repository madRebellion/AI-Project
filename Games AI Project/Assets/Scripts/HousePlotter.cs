using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HousePlotter : MonoBehaviour {

    public GameObject[] plots;
    public int plotsAvailable;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        plots = GameObject.FindGameObjectsWithTag("Empty Plot");
        plotsAvailable = plots.Length;

        


	}


}
