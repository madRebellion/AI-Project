using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackMember : MonoBehaviour
{
    SteeringBehaviours SB;
    Birdwatcher DB;
    SleepDesire SD;
    ConsumeDesire CD;
    ToiletDesire TD;
    WasherDesire WD;
    PillUnit PU;
    public GameObject packBoi;
    Vector3 LeaderPosition = Vector3.zero;
    public LayerMask mask, sleepMask, consumptionMask, excretionMask, washingMask;
    bool wandering = false;
    GameObject food = null, toilet = null, washer = null;
    public float closestSustinance = 10000000, closestToilet = 10000000, closestWasher = 10000000;
    float treeTracker = 0;
    public string targetLocation;

    public GameObject[] foodTrees, toilets, washers;

    public bool performingAction;

    //Desirability   
    public float sleepDesire;
    public float eatDesire;
    public float toiletDesire;
    public float washDesire;
    public float redParkDesire;
    public float blueParkDesire;
    public float yellowParkDesire;
    public float toSustinance;
    public float toToilet;
    public float toWasher;
    public string currentDesire;
    public float currentDesireValue;

    // Use this for initialization
    void Awake()
    {
        performingAction = false;
        SB = GetComponent<SteeringBehaviours>();
        DB = GetComponent<Birdwatcher>();
        SD = GetComponent<SleepDesire>();
        CD = GetComponent<ConsumeDesire>();
        TD = GetComponent<ToiletDesire>();
        WD = GetComponent<WasherDesire>();
        PU = GetComponent<PillUnit>();
        foodTrees = GameObject.FindGameObjectsWithTag("Food");
        toilets = GameObject.FindGameObjectsWithTag("Toilet");
        washers = GameObject.FindGameObjectsWithTag("Washer");
        FindClosestSustinance();
        FindClosestToilet();
        FindClosestWasher();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -5) // If the Pill falls beneath the bounds of the map...
        {
            transform.GetComponent<Transform>().localPosition = SD.home.GetComponent<Transform>().localPosition; // ...teleport them back to their home.
        }

        foodTrees = GameObject.FindGameObjectsWithTag("Food");
        toilets = GameObject.FindGameObjectsWithTag("Toilet");
        washers = GameObject.FindGameObjectsWithTag("Washer");
        Debug.DrawRay(transform.position, -transform.up * 10, Color.green);
        Ray ray = new Ray(transform.position, -transform.up * 10);

        if (!performingAction)
        {
            CalculateDesires();
        }

        if (performingAction)
        {
            if (!Physics.Raycast(ray, 10, sleepMask) && SD.isSleeping)
            {
                SD.isSleeping = false;
                performingAction = false;
            }
            if (!Physics.Raycast(ray, 10, consumptionMask) && CD.isEating)
            {
                CD.isEating = false;
                performingAction = false;
            }
            if (!Physics.Raycast(ray, 10, excretionMask) && TD.isExcreting)
            {
                TD.isExcreting = false;
                performingAction = false;
            }
            if (!Physics.Raycast(ray, 10, washingMask) && WD.isWashing)
            {
                WD.isWashing = false;
                performingAction = false;
            }

            if (SD.isSleeping == true && SD.energyLevel > 99.9f)
            {
                SD.isSleeping = false;
                SD.needSleep = false;
                performingAction = false;
            }
            if (CD.isEating && CD.sustinanceLevel > 99.9f)
            {
                CD.isEating = false;
                performingAction = false;
                CD.needSustinance = false;
                food.GetComponent<TreeHealth>().eatTree(food);
                closestSustinance = 10000000;
                food = null;

            }
            if (TD.isExcreting && TD.bowelLevel > 99.9f)
            {
                TD.isExcreting = false;
                performingAction = false;
                TD.needExcretion = false;
                closestToilet = 10000000;
                toilet = null;
            }
            if (WD.isWashing && WD.dirtLevel < 0.1f)
            {
                WD.isWashing = false;
                performingAction = false;
                WD.needWash = false;
                closestWasher = 10000000;
                washer = null;
            }            
        }

        

        //The below system needs changing so that taking care of the pills needs are based on the desirability of satisfying each need

        if (currentDesire == "Sleep" && !performingAction)
        {
            SB.WanderOff();
          
            if (Physics.Raycast(ray, 10, sleepMask))
            {
                SD.isSleeping = true;
                performingAction = true;
            }           
        }
        else if (currentDesire == "Eat" && !performingAction)
        {
            SB.WanderOff();
            if (food == null)
            {
                FindClosestSustinance();
            }
            else
            {               
                if (Physics.Raycast(ray, 10, consumptionMask))
                {
                    CD.isEating = true;
                    performingAction = true;
                }                
            }

        }
        else if (currentDesire == "Toilet" && !performingAction)
        {
            SB.WanderOff();           
            if (toilet == null)
            {
                FindClosestToilet();
            }
            else
            {                
                if (Physics.Raycast(ray, 10, excretionMask))
                {
                    TD.isExcreting = true;
                    performingAction = true;
                }               
            }
           
        }
        else if (currentDesire == "Wash" && !performingAction)
        {
            SB.WanderOff();
            if (washer == null)
            {
                FindClosestWasher();
            }
            else
            {
                if (Physics.Raycast(ray, 10, washingMask))
                {
                    WD.isWashing = true;
                    performingAction = true;
                }               
            }
        }

        if (DB.redParkTarget != null && currentDesire == "Red Park" && DB.watchingRedBird == false && !performingAction)
        {
            SB.WanderOff();
            targetLocation = "Red Park";
            //SB.SeekOffsetOn(new Vector3(DB.redParkTarget.GetComponent<Transform>().localPosition.x, 1.0f, DB.redParkTarget.GetComponent<Transform>().localPosition.z), 1.0f);
            PU.target = DB.redParkTarget.transform;
            //SB.SeekOffsetOn(PU.target.transform.position);
            //PathThreader.RequestPath(new PathRequest(transform.position, PU.target.transform.position, PU.OnPathFound));
            //SB.SeekOffsetOn(PU.path[PU.targetIndex]);

        }
        else if (DB.blueParkTarget != null && currentDesire == "Blue Park" && DB.watchingBlueBird == false && !performingAction)
        {
            SB.WanderOff();
            targetLocation = "Blue Park";
            //SB.SeekOffsetOn(new Vector3(DB.blueParkTarget.GetComponent<Transform>().localPosition.x, 1.0f, DB.blueParkTarget.GetComponent<Transform>().localPosition.z), 1.0f);
            PU.target = DB.blueParkTarget.transform;
            //SB.SeekOffsetOn(PU.target.transform.position);
            //PathThreader.RequestPath(new PathRequest(transform.position, PU.target.transform.position, PU.OnPathFound));
            //SB.SeekOffsetOn(PU.path[PU.targetIndex]);

        }
        else if (DB.yellowParkTarget != null && currentDesire == "Yellow Park" && DB.watchingYellowBird == false && !performingAction)
        {
            SB.WanderOff();
            targetLocation = "Yellow Park";
            //SB.SeekOffsetOn(new Vector3(DB.yellowParkTarget.GetComponent<Transform>().localPosition.x, 1.0f, DB.yellowParkTarget.GetComponent<Transform>().localPosition.z), 1.0f);
            PU.target = DB.yellowParkTarget.transform;
            //SB.SeekOffsetOn(PU.target.transform.position);
            //PathThreader.RequestPath(new PathRequest(transform.position, PU.target.transform.position, PU.OnPathFound));
        }

        if (!performingAction)
        {
            if (currentDesire == "Sleep" && !Physics.Raycast(ray, 10, sleepMask))
            {
                targetLocation = "Sleep";
                //SB.SeekOffsetOn(new Vector3(SD.home.GetComponent<Transform>().localPosition.x, 1.0f, SD.home.GetComponent<Transform>().localPosition.z));
                PU.target = SD.home.transform;
                //SB.SeekOffsetOn(PU.target.transform.position);
                //PathThreader.RequestPath(new PathRequest(transform.position, PU.target.transform.position, PU.OnPathFound));
                //SB.SeekOffsetOn(PU.path[PU.targetIndex]);

            }
            if (currentDesire == "Eat" && !Physics.Raycast(ray, 10, consumptionMask))
            {
                targetLocation = "Food";
                //SB.SeekOffsetOn(new Vector3(food.GetComponent<Transform>().localPosition.x, 1.0f, food.GetComponent<Transform>().localPosition.z));
                PU.target = food.transform;
                //SB.SeekOffsetOn(PU.target.transform.position);
                //PathThreader.RequestPath(new PathRequest(transform.position, PU.target.transform.position, PU.OnPathFound));
                //SB.SeekOffsetOn(PU.path[PU.targetIndex]);

                treeTracker += Time.deltaTime;     //
                if (treeTracker >= .5)             //
                {                                  // This section of the code updates the closest sustinance to the pill.
                    food = null;                   // It updates every half a second instead of each tick as there often won't be new trees which are suddenly closer.
                    treeTracker = 0;               // Another reason is that there can be a lot of trees and therefore if they check distances to each every tick it will affect performance.
                }                                  //
            }
            if (currentDesire == "Toilet" && !Physics.Raycast(ray, 10, excretionMask))
            {
                targetLocation = "Toilet";
                //SB.SeekOffsetOn(new Vector3(toilet.GetComponent<Transform>().position.x, 1.0f, toilet.GetComponent<Transform>().position.z));
                PU.target = toilet.transform;
                //SB.SeekOffsetOn(PU.target.transform.position);
                //PathThreader.RequestPath(new PathRequest(transform.position, PU.target.transform.position, PU.OnPathFound));
                //SB.SeekOffsetOn(PU.path[PU.targetIndex]);

            }
            if (currentDesire == "Wash" && !Physics.Raycast(ray, 10, washingMask))
            {
                targetLocation = "Washer";
                //SB.SeekOffsetOn(new Vector3(washer.GetComponent<Transform>().position.x, 1.0f, washer.GetComponent<Transform>().position.z));
                PU.target = washer.transform;
                //SB.SeekOffsetOn(PU.target.transform.position);
                //PathThreader.RequestPath(new PathRequest(transform.position, PU.target.transform.position, PU.OnPathFound));
                //SB.SeekOffsetOn(PU.path[PU.targetIndex]);

            }
            if (currentDesire == "Red Park" && Physics.Raycast(ray, 10, mask) && Vector3.Distance(transform.position, DB.redParkTarget.transform.position) < 60 || currentDesire == "Blue Park" && Physics.Raycast(ray, 10, mask) && Vector3.Distance(transform.position, DB.blueParkTarget.transform.position) < 60 || currentDesire == "Yellow Park" && Physics.Raycast(ray, 10, mask) && Vector3.Distance(transform.position, DB.yellowParkTarget.transform.position) < 60)
            {
                targetLocation = "Wandering";
                wandering = true;
            }
            else
            {
                wandering = false;
            }

            if (wandering == true)
            {
                SB.WanderOn();
            }
            if (wandering == false)
            {
                SB.WanderOff();
            }

        }      
       
    }

    public void FindClosestSustinance()
    {
        closestSustinance = 10000000;
        foreach (GameObject I in foodTrees)
        {
            if (Vector3.Distance(packBoi.transform.position, I.transform.position) < closestSustinance)
            {
                closestSustinance = Vector3.Distance(packBoi.transform.position, I.transform.position);
                toSustinance = closestSustinance;
                food = I;
            }
        }
    }

    public void FindClosestToilet()
    {
        closestToilet = 10000000;
        foreach (GameObject I in toilets)
        {
            if (Vector3.Distance(packBoi.transform.position, I.transform.position) < closestToilet)
            {
                closestToilet = Vector3.Distance(packBoi.transform.position, I.transform.position);
                toToilet = closestToilet;
                toilet = I;
            }
        }
    }

    public void FindClosestWasher()
    {
        closestWasher = 10000000;
        foreach (GameObject I in washers)
        {
            if (Vector3.Distance(packBoi.transform.position, I.transform.position) < closestWasher)
            {
                closestWasher = Vector3.Distance(packBoi.transform.position, I.transform.position);
                toWasher = closestWasher;
                washer = I;
            }
        }
    }

    public void CalculateDesires()
    {
        redParkDesire = (100 - DB.consequent) + DB.desireToWatchRedBird;
        currentDesireValue = redParkDesire; currentDesire = "Red Park";
        blueParkDesire = (100 - DB.consequent) + DB.desireToWatchBlueBird;
        if (blueParkDesire > currentDesireValue)
        {
            currentDesireValue = blueParkDesire; currentDesire = "Blue Park";
        }
        yellowParkDesire = (100 - DB.consequent) + DB.desireToWatchYellowBird;
        if (yellowParkDesire > currentDesireValue)
        {
            currentDesireValue = yellowParkDesire; currentDesire = "Yellow Park";
        }
        FindClosestSustinance();
        eatDesire = 100 - CD.consequent;
        if (eatDesire > currentDesireValue)
        {
            currentDesireValue = eatDesire; currentDesire = "Eat";
        }
        FindClosestToilet();
        toiletDesire = 100 - TD.consequent;
        if (toiletDesire > currentDesireValue)
        {
            currentDesireValue = toiletDesire; currentDesire = "Toilet";
        }
        FindClosestWasher();
        washDesire = 100 - WD.consequent;
        if (washDesire > currentDesireValue)
        {
            currentDesireValue = washDesire; currentDesire = "Wash";
        }
        sleepDesire = 100 - SD.consequent;
        if (sleepDesire > currentDesireValue)
        {
            currentDesireValue = sleepDesire; currentDesire = "Sleep";
        }
    }
}
