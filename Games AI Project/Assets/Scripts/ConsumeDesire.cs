using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeDesire : MonoBehaviour {

    public float sustinanceLevel;
    public float sustinanceDecayRate;
    public float rateOfConsumption;
    public float maxSustinance;
    public float consequent;

    public bool needSustinance;
    public bool isEating;

	// Use this for initialization
	void Start () {
        sustinanceLevel = Random.Range(60f, 100f);
        sustinanceDecayRate = Random.Range(0.025f, 0.032f);
        rateOfConsumption = Random.Range(0.32f, 0.4f);
        maxSustinance = 100.0f;
	}
	
	// Update is called once per frame
	void Update () {
		
        if (!isEating)
        {
            SustinanceDecay();
        }
        else
        {
            Consumption();
        }

        if (sustinanceLevel < 20f)
        {
            needSustinance = true;
        }
	}

    public void SustinanceDecay()
    {
        sustinanceLevel -= sustinanceDecayRate;
        sustinanceLevel = Mathf.Clamp(sustinanceLevel, 0.0f, maxSustinance);
    }

    public void Consumption()
    {
        sustinanceLevel += rateOfConsumption;
    }
}
