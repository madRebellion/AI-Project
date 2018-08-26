using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletDesire : MonoBehaviour {

    public float bowelLevel;
    public float bowelDecayRate;
    public float rateOfExcrement;
    public float maxBowel;
    public float consequent;

    public bool needExcretion;
    public bool isExcreting;

	// Use this for initialization
	void Start () {
        bowelLevel = Random.Range(60f, 100f);
        bowelDecayRate = Random.Range(0.02f, 0.05f);
        rateOfExcrement = Random.Range(0.3f, 0.7f);
        maxBowel = 100.0f;
	}
	
	// Update is called once per frame
	void Update () {

        if (!isExcreting)
        {
            BowelDecay();
        }
        else
        {
            Excretion();
        }

        if (bowelLevel < 15f)
        {
            needExcretion = true;
        }
	}

    public void BowelDecay()
    {
        bowelLevel -= bowelDecayRate;
        bowelLevel = Mathf.Clamp(bowelLevel, 0.0f, maxBowel);
    }

    public void Excretion()
    {
        bowelLevel += rateOfExcrement;
    }
}
