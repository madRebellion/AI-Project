using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingParent : MonoBehaviour {

    public GameObject parent;

    public void setParent(GameObject target)
    {
        parent = target;
    }

}
