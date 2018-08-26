using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoiSelector : MonoBehaviour {

    public GameObject activePill;
    Quaternion standardRotation;
    public Text Qol, food, sleep, toilet, wash, fun, QolValue, foodValue, sleepValue, toiletValue, washValue, funValue;
    public Image background;
    float camY;
    public GameObject[] pills;
    public DesirabilityBoi DB;

    // Use this for initialization
    void Start () {
        standardRotation.eulerAngles = new Vector3(90f, 0f, 0f);
        //transform.position = (new Vector3(0f, 45f, 50f));
        transform.rotation = standardRotation;
        camY = Camera.main.transform.position.y;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
            {
                pills = GameObject.FindGameObjectsWithTag("Boi");
                foreach (GameObject i in pills)
                {
                    if (i.name == hitInfo.transform.name)
                    {
                        activePill = i;
                        activePill.transform.localScale = new Vector3(1.01f, 1.01f, 1.01f);
                    }
                }
                             
            }
        }

        if (Input.GetButtonDown("Backspace") || Input.GetMouseButtonDown(1) && activePill != null)
        {
            if (activePill != null)
            {
                transform.position = (new Vector3(activePill.transform.position.x, 30f, activePill.transform.position.z));
                activePill.transform.localScale = new Vector3(1, 1, 1);
            } else
            {
                transform.position = (new Vector3(transform.position.x, 30f, transform.position.z));
            }
            transform.rotation = standardRotation;            
            activePill = null;
        }

        if (activePill != null)
        {
            transform.position = (new Vector3(activePill.transform.position.x, activePill.transform.position.y, activePill.transform.position.z));
            transform.rotation = activePill.transform.rotation;
            transform.Translate(0f, 1.5f, -2f);
        }
        else
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                transform.Translate(0f, 0f, 1f);
                transform.position = new Vector3(transform.position.x , Mathf.Clamp(transform.position.y, 35.0f, 100.0f), transform.position.z);
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                transform.Translate(0f, 0f, -1f);
                transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, 35.0f, 100.0f), transform.position.z);
            }

        }


        if (activePill == null && Qol.enabled == true)
        {
            Qol.enabled = false;
            food.enabled = false;
            sleep.enabled = false;
            toilet.enabled = false;
            wash.enabled = false;
            fun.enabled = false;
            QolValue.enabled = false;
            foodValue.enabled = false;
            sleepValue.enabled = false;
            toiletValue.enabled = false;
            washValue.enabled = false;
            funValue.enabled = false;
            background.enabled = false;
        }
        else if (activePill != null && Qol.enabled == false)
        {
            Qol.enabled = true;
            food.enabled = true;
            sleep.enabled = true;
            toilet.enabled = true;
            wash.enabled = true;
            fun.enabled = true;
            QolValue.enabled = true;
            foodValue.enabled = true;
            sleepValue.enabled = true;
            toiletValue.enabled = true;
            washValue.enabled = true;
            funValue.enabled = true;
            background.enabled = true;
        }


    }

}
