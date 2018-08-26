using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Birdwatcher : MonoBehaviour
{

    public GameObject boi;
    public LayerMask mask;
    public float distanceToWatchRedBird, distanceToWatchBlueBird, distanceToWatchYellowBird;
    public GameObject redParkTarget, blueParkTarget, yellowParkTarget;
    public float desireToWatchRedBird, desireToWatchBlueBird, desireToWatchYellowBird;
    public bool watchingRedBird, watchingBlueBird, watchingYellowBird;
    float currentDesire;
    public string desiredPark;
    public Text boiDes, boiRed, boiBlue, boiYellow, boiwatchRed, boiwatchBlue, boiwatchYellow;
    float ticker = 0;

    public float birdExcitment;
    public bool watchingBirbz;
    public float consequent;

    // Use this for initialization
    void Start () {
        desireToWatchBlueBird = Random.Range(0f, 1f);
        desireToWatchRedBird = Random.Range(0f, 1f);
        desireToWatchYellowBird = Random.Range(0f, 1f);
        int temp = Random.Range(0, 3);
        if (temp == 0)
        {
            desiredPark = "Blue";
        }
        else if (temp == 1)
        {
            desiredPark = "Red";
        }
        else if (temp == 2)
        {
            desiredPark = "Yellow";
        }
    }
	
	// Update is called once per frame
	void Update () {

        //Desire Increase/Decrease

        if (watchingRedBird == true)
        {
            desireToWatchRedBird -= 0.001f;
        } else
        {
            desireToWatchRedBird += 0.001f;
        }

        if (watchingBlueBird == true)
        {
            desireToWatchBlueBird -= 0.001f;
        }
        else
        {
            desireToWatchBlueBird += 0.001f;
        }

        if (watchingYellowBird == true)
        {
            desireToWatchYellowBird -= 0.001f;
        }
        else
        {
            desireToWatchYellowBird += 0.001f;
        }

        if (desiredPark == "Red")
        {
            currentDesire = desireToWatchRedBird;
            
        }
        if (desiredPark == "Blue")
        {
            currentDesire = desireToWatchBlueBird;
        }
        if (desiredPark == "Yellow")
        {
            currentDesire = desireToWatchYellowBird;
        }

        //Park Detection

        Ray ray = new Ray(transform.position, -transform.up * 10);
        RaycastHit hitinfo;
        if (Physics.Raycast(ray, out hitinfo, 10, mask))
        {
            if (hitinfo.collider.tag == "Red Park")
            {
                watchingRedBird = true;
                watchingBlueBird = false;
                watchingYellowBird = false;
                watchingBirbz = true;
            }
            else if (hitinfo.collider.tag == "Blue Park")
            {
                watchingRedBird = false;
                watchingBlueBird = true;
                watchingYellowBird = false;
                watchingBirbz = true;
                
            }
            else if (hitinfo.collider.tag == "Yellow Park")
            {
                watchingRedBird = false;
                watchingBlueBird = false;
                watchingYellowBird = true;
                watchingBirbz = true;
            }
        }
        else
        {
            watchingRedBird = false;
            watchingBlueBird = false;
            watchingYellowBird = false;
            watchingBirbz = false;
        }

        GameObject[] redParks = GameObject.FindGameObjectsWithTag("Red Park");
        GameObject[] blueParks = GameObject.FindGameObjectsWithTag("Blue Park");
        GameObject[] yellowParks = GameObject.FindGameObjectsWithTag("Yellow Park");

        float temp = 10000;
        GameObject tempGO = null;
        foreach (GameObject i in redParks)
        {
            if (Vector3.Distance(transform.position, i.GetComponent<Transform>().localPosition) < temp)
            {
                temp = Vector3.Distance(transform.position, i.GetComponent<Transform>().localPosition);
                tempGO = i;
            }
        }

        if (tempGO != null)
        {
            redParkTarget = tempGO;
            distanceToWatchRedBird = Vector3.Distance(transform.position, tempGO.GetComponent<Transform>().localPosition);
        }

        temp = 10000;
        tempGO = null;
        foreach (GameObject i in blueParks)
        {
            if (Vector3.Distance(transform.position, i.GetComponent<Transform>().localPosition) < temp)
            {
                temp = Vector3.Distance(transform.position, i.GetComponent<Transform>().localPosition);
                tempGO = i;
            }
        }

        if (tempGO != null)
        {
            blueParkTarget = tempGO;
            distanceToWatchBlueBird = Vector3.Distance(transform.position, tempGO.GetComponent<Transform>().localPosition);
        }

        temp = 10000;
        tempGO = null;
        foreach (GameObject i in yellowParks)
        {
            if (Vector3.Distance(transform.position, i.GetComponent<Transform>().localPosition) < temp)
            {
                temp = Vector3.Distance(transform.position, i.GetComponent<Transform>().localPosition);
                tempGO = i;
            }
        }

        if (tempGO != null)
        {
            yellowParkTarget = tempGO;
            distanceToWatchYellowBird = Vector3.Distance(transform.position, tempGO.GetComponent<Transform>().localPosition);
        }

        if (currentDesire < desireToWatchRedBird - (distanceToWatchRedBird / 100))
        {
            currentDesire = desireToWatchRedBird;
            desiredPark = "Red";
        }

        if (currentDesire < desireToWatchBlueBird - (distanceToWatchBlueBird / 100))
        {
            currentDesire = desireToWatchBlueBird;
            desiredPark = "Blue";
        }

        if (currentDesire < desireToWatchYellowBird - (distanceToWatchYellowBird / 100))
        {
            currentDesire = desireToWatchYellowBird;
            desiredPark = "Yellow";
        }   

        if (watchingBirbz)
        {
            birdExcitment += 0.001f;
        }
    }
}
