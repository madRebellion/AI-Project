using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeHealth : MonoBehaviour {

    public int health;

	// Use this for initialization
	void Start () {
        health = 5;
	}

    public void eatTree(GameObject Tree)
    {
        health--;
        if (health <= 0)
        {
            Destroy(Tree);
        }
    }
}
