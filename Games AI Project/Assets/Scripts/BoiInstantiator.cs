using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoiInstantiator : MonoBehaviour {

    public int maxBois; //How many bois can appear at one time
    public GameObject[] bois;
    public GameObject boi;
    public GameObject house;
    public HousePlotter HP;
    public BuildingParent BP;
    public int boisSpawned;

    float spawnTime;

	// Use this for initialization
	void Start () {
        spawnTime = 0;
        boisSpawned = 0;
        HP = GetComponent<HousePlotter>();
        BP = GetComponent<BuildingParent>();
	}
	
	// Update is called once per frame
	void Update () {
        bois = GameObject.FindGameObjectsWithTag("Boi");
        if (bois.Length < maxBois && HP.plotsAvailable > 0)
        {
            spawnTime += Time.deltaTime;
            if (spawnTime >= 1)
            {
                int rnd;
                do
                {
                    rnd = Random.Range(0, HP.plotsAvailable - 1); //Randomizes which plot will be taken
                } while (HP.plots[rnd].activeInHierarchy == false);


                GameObject i = Instantiate(boi, new Vector3(HP.plots[rnd].transform.position.x, 1f, HP.plots[rnd].transform.position.z), boi.transform.rotation);
                GameObject j = Instantiate(house, new Vector3(HP.plots[rnd].transform.position.x, HP.plots[rnd].transform.position.y, HP.plots[rnd].transform.position.z), HP.plots[rnd].transform.rotation);
                boisSpawned++;
                i.name = "Capsule Boy #" + boisSpawned;
                HP.plots[rnd].tag = "Building";
                i.GetComponent<SleepDesire>().home = j;
                j.GetComponent<BuildingParent>().setParent(HP.plots[rnd]);
                spawnTime = 0;
            }

        }
	}
}
