using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DesirabilityBoi : MonoBehaviour
{
    
    public float QoLStatus
               , distanceToItem
               , visibilityOnTarget
               , needToSleep
               , needToConsume
               , needToExcrete
               , needToSocialize
               , needForFun
               , needToWash
               , lifeSpan;

    public GameObject desireBoi;
    public GameObject deadBoi;
    public Text qoltext;
    public Text Qol, food, sleep, toilet, wash, fun, QolValue, foodValue, sleepValue, toiletValue, washValue, funValue, currentD;

    SleepDesire SD;
    ConsumeDesire CD;
    ToiletDesire TD;
    Birdwatcher BW;
    WasherDesire WD;
    PackMember PM;

    Fuzzy WashDesirability, EatDesirability, SleepDesirability, ExcreteDesirability, BirdDesirability;
    
    // Use this for initialization
    void Start()
    {
        SD = GetComponent<SleepDesire>();
        CD = GetComponent<ConsumeDesire>();
        TD = GetComponent<ToiletDesire>();
        BW = GetComponent<Birdwatcher>();
        WD = GetComponent<WasherDesire>();
        PM = GetComponent<PackMember>();
        WashDesirability = GetComponent<Fuzzy>();
        EatDesirability = GetComponent<Fuzzy>();
        SleepDesirability = GetComponent<Fuzzy>();
        ExcreteDesirability = GetComponent<Fuzzy>();
        BirdDesirability = GetComponent<Fuzzy>();
        lifeSpan = 0;

        // Assigning UI texts
        #region
        foreach (Text t in GameObject.FindObjectsOfType<Text>())
        {
            if (t.name == "Quality of Life")
            {
                Qol = t;
            } else if (t.name == "Food")
            {
                food = t;
            }
            else if (t.name == "Sleep")
            {
                sleep = t;
            }
            else if (t.name == "Toilet")
            {
                toilet = t;
            }
            else if (t.name == "Wash")
            {
                wash = t;
            }
            else if (t.name == "Fun")
            {
                fun = t;
            }
            else if (t.name == "QoLValue")
            {
                QolValue = t;
            }
            else if (t.name == "FoodValue")
            {
                foodValue = t;
            }
            else if (t.name == "SleepValue")
            {
                sleepValue = t;
            }
            else if (t.name == "ToiletValue")
            {
                toiletValue = t;
            }
            else if (t.name == "WashValue")
            {
                washValue = t;
            }
            else if (t.name == "FunValue")
            {
                funValue = t;
            }
            else if (t.name == "Desire")
            {
                currentD = t;
            }
        }
        #endregion


    }

    // Update is called once per frame
    void Update()
    {
        needToSleep = SD.energyLevel;
        needToConsume = CD.sustinanceLevel;
        needToExcrete = TD.bowelLevel;
        needToWash =  100 - WD.dirtLevel;
        needForFun = BW.birdExcitment;

        NeedToSleep();
        NeedToEat();
        NeedToWash();
        BirdWatch();
        NeedToExcrete();

        QoLStatus = (((needToSleep / 5) + (needToConsume / 5) + (needToExcrete / 5) + (needToWash / 5) /* + (needToSocialize / 6) */ + (needForFun / 5)) / 100);

        desireBoi.GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.green, QoLStatus);

        lifeSpan += Time.deltaTime;

        //if (desireBoi != null)
        //{
        //    qoltext.text = "" + (Mathf.RoundToInt(QoLStatus * 10000)) / 100f;
        //    qoltext.color = Color.Lerp(Color.red, Color.green, QoLStatus);
        //} else
        //{
        //    //of course this never triggers
        //    qoltext.text = "Dead, aged: " + lifeSpan;
        //    qoltext.color = Color.black; 
        //}

        if (desireBoi.transform.localScale.x > 1f)
        {
            QolValue.text = (QoLStatus * 100).ToString();
            foodValue.text = /*needToConsume.ToString()*/(100f - CD.consequent).ToString();
            sleepValue.text = /*needToSleep.ToString()*/(100f - CD.consequent).ToString();
            toiletValue.text = /*needToExcrete.ToString()*/(100f - TD.consequent).ToString();
            washValue.text = /*needToWash.ToString()*/(100f - WD.consequent).ToString();
            funValue.text = /*needForFun.ToString()*/(100f - BW.consequent).ToString();
            currentD.text = PM.currentDesire.ToString();
            Qol.color = Color.Lerp(Color.red, Color.green, QoLStatus);
            food.color = Color.Lerp(Color.red, Color.green, needToConsume / 100);
            sleep.color = Color.Lerp(Color.red, Color.green, needToSleep / 100);
            toilet.color = Color.Lerp(Color.red, Color.green, needToExcrete / 100);
            wash.color = Color.Lerp(Color.red, Color.green, needToWash / 100);
            fun.color = Color.Lerp(Color.red, Color.green, needForFun / 100);
        }

        if (QoLStatus < .15f)
        {
            Debug.Log("A boi died aged: " + Mathf.RoundToInt(lifeSpan / 20));
            Destroy(desireBoi.GetComponent<SleepDesire>().home);
            desireBoi.GetComponent<SleepDesire>().home.GetComponent<BuildingParent>().parent.tag = "Empty Plot";
            Instantiate(deadBoi, desireBoi.GetComponent<Transform>().localPosition, desireBoi.GetComponent<Transform>().localRotation);
            Destroy(desireBoi);
            
        }
    }

    void NeedToSleep()
    {

        float sleepDesireLS = SleepDesirability.LeftShoulderMembership(needToSleep);
        float sleepDesireT = SleepDesirability.TrapezoidMembership(needToSleep);
        float sleepDesireRS = SleepDesirability.RightShoulderMembership(needToSleep);
        
        SD.consequent = ((5f * sleepDesireLS) + (50f * sleepDesireT) + (95f * sleepDesireRS)) / ((sleepDesireLS + sleepDesireT + sleepDesireRS));
    }

    void NeedToWash()
    {

        float washDesireLS = WashDesirability.LeftShoulderMembership(needToWash);
        float washDesireT = WashDesirability.TrapezoidMembership(needToWash);
        float washDesireRS = WashDesirability.RightShoulderMembership(needToWash);

        WD.consequent = ((5f * washDesireLS) + (50f * washDesireT) + (95f * washDesireRS)) / ((washDesireLS + washDesireT + washDesireRS));
    }

    void NeedToEat()
    {

        float eatDesireLS = EatDesirability.LeftShoulderMembership(needToConsume);
        float eatDesireT = EatDesirability.TrapezoidMembership(needToConsume);
        float eatDesireRS = EatDesirability.RightShoulderMembership(needToConsume);

        CD.consequent = ((5f * eatDesireLS) + (50f * eatDesireT) + (95f * eatDesireRS)) / ((eatDesireLS + eatDesireT + eatDesireRS));
    }

    void BirdWatch()
    {
        float birdDesireLS = BirdDesirability.LeftShoulderMembership(needForFun);
        float birdDesireT = BirdDesirability.TrapezoidMembership(needForFun);
        float birdDesireRS = BirdDesirability.RightShoulderMembership(needForFun);
        
        BW.consequent = ((5f * birdDesireLS) + (50f * birdDesireT) + (95f * birdDesireRS)) / ((birdDesireLS + birdDesireT + birdDesireRS));
    }

    void NeedToExcrete()
    {
        float excreteDesireLS = ExcreteDesirability.LeftShoulderMembership(needToExcrete);
        float excreteDesireT = ExcreteDesirability.TrapezoidMembership(needToExcrete);
        float excreteDesireRS = ExcreteDesirability.RightShoulderMembership(needToExcrete);

        //ExcreteDesirability.DOMLeftShoulder = excreteDesireLS;
        //ExcreteDesirability.DOMTrapezoid = excreteDesireT;
        //ExcreteDesirability.DOMRightShoulder = excreteDesireRS;

        TD.consequent = ((5f * excreteDesireLS) + (50f * excreteDesireT) + (95f * excreteDesireRS)) / ((excreteDesireLS + excreteDesireT + excreteDesireRS));
    }

}
