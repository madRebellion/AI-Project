using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillUnit : MonoBehaviour {

    public Transform target;
    float speed = 10;
    Vector3[] path;         // Reference to a path array
    int targetIndex;        // keeps track of the current waypoint in the path array

    public bool showPath;

    SteeringBehaviours SB;
    
	// Use this for initialization
	void Start ()
    {
        SB = GetComponent<SteeringBehaviours>();
        target = transform;
        StartCoroutine(UpdatePath());
    }
	

    public void OnPathFound(Vector3[] newPath, bool pathSuccess)
    {
        if (this != null)
        {
            if (pathSuccess)
            {
                path = newPath;
                targetIndex = 0;
                StopCoroutine(FollowPath());
                StartCoroutine(FollowPath());
            }
        }
    }

    // Updates the path for the agents every one second using the IEnumerators WaitForSeconds()
    IEnumerator UpdatePath()
    {   
        PathThreader.RequestPath(new PathRequest(transform.position, target.transform.position, OnPathFound));

        Vector3 oldTargetPos = target.position;

        while (true)
        {            
            PathThreader.RequestPath(new PathRequest(transform.position, target.transform.position, OnPathFound));
            oldTargetPos = target.position;
            yield return new WaitForSeconds(1f);
        }
    }


    //The agent will follow this path using waypoints
    //These waypoints appear every time the agent changes looking direction
    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            //if the agent is within 1 unit from the current waypoint 
            if (Vector3.Distance(transform.position, currentWaypoint) < 1f)
            {

                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    targetIndex = 0;
                    path = new Vector3[0];
                    yield break;
                }

               
            }
            //move along the path
            currentWaypoint = path[targetIndex];
            if (target != null)
            {
                // Move to a specific area of the tree when looking for food only
                if (target.name == "Food Tree Red(Clone)" || target.name == "Food Tree Blue(Clone)" || target.name == "Food Tree Yellow(Clone)")
                {
                    SB.ArriveOn(currentWaypoint + new Vector3(2.5f, 0, 0), 0.5f, 3.5f);
                }
                else
                {
                    SB.ArriveOn(currentWaypoint, 0.5f, 3.5f);
                }
            }

            yield return null;
        }

    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
