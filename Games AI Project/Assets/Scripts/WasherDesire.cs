using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasherDesire : MonoBehaviour {

    public float dirtLevel;
    public float dirtDecayRate;
    public float rateOfWashing;
    public float maxDirt;
    public float consequent;
    public float closestWasher;
    public float dirtLow, dirtOK, dirtHigh;
    public float sum;
    public float WasherDesirability;

    public float closeMembership;
    public float mediumMembership;
    public float farMembership;
    public float rvLeft, rvRight, rvTrap;
    public float fnl, fnm, fnh, mnl, mnm, mnh, cnl, cnm, cnh, rvfl, rvfm, rvfh, rvml, rvmm, rvmh, rvcl, rvcm, rvch;
    public float confidenceSum;

    PackMember desirable;
    Fuzzy fuzzyLogic, fuzzyLogicDis;

    public bool needWash;
    public bool isWashing;

    public List<float> desirabilityValues = new List<float>();

	// Use this for initialization
	void Start () {
        desirable = GetComponent<PackMember>();
        fuzzyLogic = GetComponent<Fuzzy>();
        fuzzyLogicDis = GetComponent<Fuzzy>();
        dirtLevel = Random.Range(0f, 40f);
        dirtDecayRate = Random.Range(0.0006f, 0.01f);
        rateOfWashing = Random.Range(0.3f, 0.45f);
        maxDirt = 100.0f;
        rvLeft = fuzzyLogic.RVLeftShoulder;
        rvRight = fuzzyLogic.RVRightShoulder;
        rvTrap = fuzzyLogic.RVTrapezoid;
	}
	
	// Update is called once per frame
	void Update () {
        
        closestWasher = desirable.closestWasher;
        WasherDistance();
        FarAndHigh();
        FarAndMedium();
        FarAndLow();
        MediumAndHigh();
        MediumAndMedium();
        MediumAndLow();
        CloseAndHigh();
        CloseAndMedium();
        CloseAndLow();
        ConfidenceValue();

        if (!isWashing)
        {
            DirtDecay();
        }
        else
        {
            Washing();
        }

        if (dirtLevel > 80f)
        {
            needWash = true;
        }

	}

    public void DirtDecay()
    {
        dirtLevel += dirtDecayRate;
        dirtLevel = Mathf.Clamp(dirtLevel, 0f, maxDirt);
    }

    public void Washing()
    {
        dirtLevel -= rateOfWashing;
    }
     
    //Memberships
    void WasherDistance()
    {
        closeMembership = fuzzyLogic.LeftShoulderMembership(closestWasher, 10f, 70f);
        mediumMembership = fuzzyLogic.TrapezoidMembership(closestWasher, 10f, 70f, 100f, 170f);
        farMembership = fuzzyLogic.RightShoulderMembership(closestWasher, 100f, 170f);

        dirtLow = fuzzyLogic.LeftShoulderMembership(dirtLevel);
        dirtOK = fuzzyLogic.TrapezoidMembership(dirtLevel);
        dirtHigh = fuzzyLogic.RightShoulderMembership(dirtLevel);
    }

    //Fuzzy Rules
    //iGn0rE!!!!!
    #region
    //IF targetFar and dirtLevel_Low then undesirable
    void FarAndLow()
    {   
        //Picks the lowest value as its degree
        fnl = fuzzyLogic.FLO_AND(farMembership, dirtLow);
        rvfl = rvRight * fnl;
       
    }

    //IF targetFar and dirtLevel_OK then undesirable
    void FarAndMedium()
    {
        fnm = fuzzyLogic.FLO_AND(farMembership, dirtOK);
        rvfm = rvRight * fnm;
        
    }

    //IF targetFar and dirtLevel_High then desirable
    void FarAndHigh()
    {
        fnh = fuzzyLogic.FLO_AND(farMembership, dirtHigh);
        rvfh = rvRight * fnh;
        
    }

    //IF targetMedium and dirtLevel_Low then undesirable
    void MediumAndLow()
    {
        mnl = fuzzyLogic.FLO_AND(mediumMembership, dirtLow);
        rvml = rvRight * mnl;
       
    }

    //IF targetMedium and dirtLevel_OK then desirable
    void MediumAndMedium()
    {
        mnm = fuzzyLogic.FLO_AND(mediumMembership, dirtOK);
        rvmm = rvRight * mnm;
       
    }

    //IF targetMedium and dirtLevel_High then desirable
    void MediumAndHigh()
    {
        mnh = fuzzyLogic.FLO_AND(mediumMembership, dirtHigh);
        rvmh = rvRight * mnh;
       
    }

    //IF targetClose and dirtLevel_Low then undesirable
    void CloseAndLow()
    {
        cnl = fuzzyLogic.FLO_AND(closeMembership, dirtLow);
        rvcl = rvRight * cnl;
       
    }

    //IF targetClose and dirtLevel_OK then desirable
    void CloseAndMedium()
    {
        cnm = fuzzyLogic.FLO_AND(closeMembership, dirtOK);
        rvcm = rvRight * cnm;
        
    }

    //IF targetClose and dirtLevel_High then very desirable
    void CloseAndHigh()
    {
        cnh = fuzzyLogic.FLO_AND(closeMembership, dirtHigh);
        rvch = rvRight * cnh;
        //if (!desirabilityValues.Contains(rvch))
        //{
        //    //desirabilityValues.Remove(rvch);
        //    desirabilityValues.Add(rvch);
        //}
    }
 

    //Confidence
    void ConfidenceValue()
    {
        //for (int i = 0; i < desirabilityValues.Count; i++)
        //{
        //    sum += desirabilityValues[i];
        //}

        sum = rvch + rvcl + rvcm + rvfh + rvfl + rvfm + rvmh + rvml + rvmm;
        confidenceSum = fnh + fnm + fnl + mnm + mnh + mnl + cnm + cnh + cnl;

        WasherDesirability = sum / confidenceSum;
    }
    #endregion
}
