using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeRepopulation : MonoBehaviour {

    public GameObject[] trees;
    public GameObject foodTree;
    public GameObject park;
    float ticker = 10;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if (trees.Length < 10)
        {
            ticker += Time.deltaTime;
            if (ticker > 10)
            {
                GameObject[] temp = new GameObject[trees.Length + 1];
                trees.CopyTo(temp, 0);
                trees = temp;
                float xOff, zOff;
                xOff = Random.Range(-30f, 30f);
                zOff = Random.Range(-30f, 30f);
                GameObject i = Instantiate(foodTree, new Vector3(park.transform.transform.position.x + xOff, park.transform.transform.position.y, park.transform.transform.position.z + zOff), Quaternion.Euler(foodTree.transform.rotation.x, Random.Range(0f, 360f), foodTree.transform.rotation.z));
                trees[trees.Length - 1] = i;
                ticker = 0;
            }
        }
        foreach (GameObject i in trees)
        {
            if (i == null)
            {
               CheckForDeadTrees();
            }
        }
	}
    public void CheckForDeadTrees()

    {        
        if (trees.Length > 1)
        {
            int dead = 0;
            for (int i = 1; i < trees.Length; i ++)
            {
                if (trees[trees.Length - i - 1] == null)
                {
                    
                        trees[trees.Length - i - 1] = trees[trees.Length - i];
                        dead++;
                    
                }
            }
            if (dead >= 1)
            {
                GameObject[] temp = new GameObject[trees.Length - dead];
                for (int i = 0; i < temp.Length; i++)
                {
                    temp[i] = trees[i];
                }
                trees = temp;
            }
        }
    }
}
