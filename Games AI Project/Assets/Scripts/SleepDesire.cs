using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepDesire : MonoBehaviour {

    public float energyLevel;
    public float energyDecayRate;
    public float rateOfRest;
    public float maxEnergy;
    public float consequent;

    public bool needSleep;
    public bool isSleeping;

    public GameObject home;

	// Use this for initialization
	void Start () {
        energyLevel = Random.Range(60f, 100f);
        energyDecayRate = Random.Range(0.0075f, 0.0115f);
        rateOfRest = Random.Range(0.085f, 0.14f);
        maxEnergy = 100.0f;
	}
	
	// Update is called once per frame
	void Update () {

        if (!isSleeping)
        {
            EnergyDecay();
        }
        else
        {
            Rest();
        }
       
        if (energyLevel < 10f)
        {
            needSleep = true;
        }

    }
    
    public void EnergyDecay()
    {
        energyLevel -= energyDecayRate;
        energyLevel = Mathf.Clamp(energyLevel, 0.0f, maxEnergy);
    }

    public void Rest()
    {
        energyLevel += rateOfRest;
    }
}
