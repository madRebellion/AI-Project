using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Vehicle))]
public class SteeringBehaviours : MonoBehaviour {

    Vehicle vehicle;

    //SeekOn
    public bool IsSeekOn = false;
    Vector3 SeekOnTargetPos;
    float SeekOnStopDistance;

    //SeekOffsetOn
    public bool IsSeekOffsetOn = false;
    Vector3 SeekOffsetOnTargetPos;
    float SeekOffsetOnStopDistance;
    public float xOffset, zOffset; //To be changed on the GameObject as opposed to in script

    //FleeFrom
    public bool IsFleeFrom = false;
    Vector3 FleeFromTargetPos;
    float FleeFromStopDistance;

    //Arrive
    public bool IsArriveOn = false;
    Vector3 ArriveOnTargetPos;
    float ArriveOnStopDistance;
    float ArriveThreshold;
    float ArriveSlowingSpeed;

    //Pursuit
    public bool IsPursuitOn = false;
    Vector3 PursuitOnTargetPos;
    public GameObject PursuedEvader;

    //Evade
    public bool IsEvadeOn = false;
    Vector3 EvadeFromTargetPos;
    public GameObject EvadedPursuer;

    //WanderOn
    public bool IsWanderOn = false;
    public float WanderRadius = 10f;
    public float WanderDistance = 10f;
    public float WanderJitter = 1f;
    Vector3 WanderTarget = Vector3.zero;

    //Obstacle Avoidance
    public List<Collider> obstacles = new List<Collider>();
    public LayerMask ObstacleMask;
    public Collider closestObstacle;
    public bool usingOAvoidance;

    public bool usingWAvoidance;

    //Hiding
    float distanceFromBoundary = 1;
    float distanceAway;
    Vector3 toObstacle;
    public Vector3 BestHidingSpot;
    public GameObject hidingFrom;
    public bool isHiding;

    //Flocking
    public List<Collider> neighbours = new List<Collider>();
    public LayerMask FlockMask;
    public bool usingFlocking;

    public GameObject target;
    public GameObject checker;

	// Use this for initialization
	void Start ()
    {
        vehicle = GetComponent<Vehicle>();
        BestHidingSpot = new Vector3(10000, 10000, 10000);
        //Set an initial wander target
        WanderTarget = new Vector3(Random.Range(-WanderRadius, WanderRadius), 0, Random.Range(-WanderRadius, WanderRadius));
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (IsSeekOn)
        {
            SeekOn(target.transform.position);
        }

        if (IsArriveOn)
        {
            ArriveOn(new Vector3(10, 1, 10));
        }
        FindNeighbours();
        FindObstacles();
        if (isHiding)
        {
            FindHidingSpot();
            BestHidingSpot.y = 1f;
            SeekOn(BestHidingSpot, 1.5f);
            BestHidingSpot = new Vector3(10000, 10000, 10000);
        }
	}

    public Vector3 Calculate()
    {
        Vector3 VelocitySum = Vector3.zero;

        if (IsSeekOn)
        {
            if (Vector3.Distance(transform.position, SeekOnTargetPos) <= SeekOnStopDistance)
            {
                //We're close enough to "stop"
                IsSeekOn = false;

                //Set the vehicle's velocity back to zero
                vehicle.Velocity = Vector3.zero;
            }
            else
            {
                VelocitySum += Seek(SeekOnTargetPos);
            }
        }

        if (IsSeekOffsetOn)
        {
            
            if (Vector3.Distance(transform.position, SeekOffsetOnTargetPos) <= SeekOffsetOnStopDistance)
            {
                //We're close enough to "stop"
                IsSeekOffsetOn = false;

                //Set the vehicle's velocity back to zero
                vehicle.Velocity = Vector3.zero;
            }
            else
            {
                VelocitySum += Seek(SeekOffsetOnTargetPos);
            }
        }

        if (IsFleeFrom)
        {
            if (Vector3.Distance(transform.position, FleeFromTargetPos) >= FleeFromStopDistance)
            {
                IsFleeFrom = false;
                vehicle.Velocity = Vector3.zero;
            }
            else
            {
                VelocitySum += Flee(FleeFromTargetPos);
            }
        }

        if (IsArriveOn)
        {
            if (Vector3.Distance(transform.position, ArriveOnTargetPos) < ArriveThreshold)
            {
                vehicle.MaxSpeed = (Vector3.Distance(transform.position, ArriveOnTargetPos) / (ArriveThreshold / 7)) / ArriveSlowingSpeed;
            }
            else
            {
                vehicle.MaxSpeed = 7;
            }
            if (Vector3.Distance(transform.position, ArriveOnTargetPos) <= ArriveOnStopDistance)
            {
                //We're close enough to "stop"
                IsArriveOn = false;

                //Set the vehicle's velocity back to zero
                vehicle.Velocity = Vector3.zero;
            }
            else
            {
                VelocitySum += Seek(ArriveOnTargetPos);
            }
        }

        if (IsPursuitOn)
        {
            if (Vector3.Dot(transform.forward, PursuedEvader.transform.forward) == 0)
                {
                PursuitOnTargetPos = PursuedEvader.transform.position;
            } else
            {
                float LookAheadTime = (Vector3.Distance(PursuedEvader.transform.position, transform.position) / (vehicle.MaxSpeed + PursuedEvader.GetComponent<Vehicle>().MaxSpeed));
                Vector3 EvaderFuturePosition = PursuedEvader.transform.position + PursuedEvader.GetComponent<Vehicle>().Velocity * LookAheadTime;
                PursuitOnTargetPos = EvaderFuturePosition;
            }
            VelocitySum += Seek(PursuitOnTargetPos);
        }

        if (IsEvadeOn)
        {
            float LookAheadTime = (Vector3.Distance(EvadedPursuer.transform.position, transform.position) / (vehicle.MaxSpeed + EvadedPursuer.GetComponent<Vehicle>().MaxSpeed));
            Vector3 PursuerFuturePosition = EvadedPursuer.transform.position + EvadedPursuer.GetComponent<Vehicle>().Velocity * LookAheadTime;
            EvadeFromTargetPos = PursuerFuturePosition;
            VelocitySum += Flee(EvadeFromTargetPos);
        }

        if (IsWanderOn)
        {
            vehicle.MaxForce = 7;
            VelocitySum += Wander();
        }

        if (usingOAvoidance)
        {
            VelocitySum += ObstacleAvoidance();
        }
        if (usingFlocking)
        {
            VelocitySum += (SeperationFlocking() * 4);
            VelocitySum += AlignmentFlocking();
            VelocitySum += (CohesionFlocking() / 2);
        }
        if (usingWAvoidance)
        {
            VelocitySum += WallAvoidance() * 20;
        }
        Debug.DrawRay(transform.position, transform.forward * 1);
        return VelocitySum;
    }

    //public void OnDrawGizmos()
    //{
        
    //    Gizmos.DrawCube(transform.position + transform.forward * 2.5f, new Vector3(1, 1, 5));
        
    //}

    Vector3 Seek(Vector3 TargetPos)
    {
        Vector3 DesiredVelocity = (TargetPos - transform.position).normalized * vehicle.MaxSpeed;

        return (DesiredVelocity - vehicle.Velocity);
    }


    Vector3 Flee(Vector3 TargetPos)
    {
        Vector3 DesiredVelocity = (transform.position - TargetPos).normalized * vehicle.MaxSpeed;

        return (DesiredVelocity - vehicle.Velocity);
    }

    Vector3 Wander()
    {
        WanderTarget += new Vector3(
            Random.Range(-2f, 2f) * WanderJitter,
            0,
            Random.Range(-2f, 2f) * WanderJitter);

        WanderTarget.Normalize();

        WanderTarget *= WanderRadius;

        //Vector3 targetLocal = WanderTarget;

        Vector3 targetWorld = transform.position + WanderTarget;

        targetWorld += transform.forward * WanderDistance;

        return targetWorld - transform.position;
    }

    /// <summary>
    /// Will Seek to TargetPos until within StopDistance range from it
    /// </summary>
    /// <param name="TargetPos"></param>
    /// <param name="StopDistance"></param>
    public void SeekOn(Vector3 TargetPos, float StopDistance = 0.01f)
    {
        IsSeekOn = true;
        SeekOnTargetPos = TargetPos;
        SeekOnStopDistance = StopDistance;
    }

    public void SeekOffsetOn(Vector3 TargetPos, float StopDistance = 0.1f)
    {
        TargetPos.x = TargetPos.x + xOffset;
        TargetPos.z = TargetPos.z + zOffset;
        IsSeekOffsetOn = true;
        SeekOffsetOnTargetPos = TargetPos;
        SeekOffsetOnStopDistance = StopDistance;
    }

    public void FleeFrom(Vector3 TargetPos, float StopDistance = 30.0f)
    {
        IsFleeFrom = true;
        FleeFromTargetPos = TargetPos;
        FleeFromStopDistance = StopDistance;
    }

    public void ArriveOn(Vector3 TargetPos, float StopDistance = 0.1f, float Threshold = 70, float SlowingSpeed = 1.2f)
    {
        IsArriveOn = true;
        ArriveOnTargetPos = TargetPos;
        ArriveOnStopDistance = StopDistance;
        ArriveThreshold = Threshold;
        ArriveSlowingSpeed = SlowingSpeed;
    }

    public void PursuitOn()
    {
        IsPursuitOn = true;
    }

    public void EvadeOn()
    {
        IsEvadeOn = true;
    }

    public void WanderOn()
    {
        vehicle.MaxTurnRate = 1;
        IsWanderOn = true;
    }

    public void WanderOff()
    {
        vehicle.MaxTurnRate = 3;
        IsWanderOn = false;
        vehicle.Velocity = Vector3.zero;
    }

    public void FindObstacles()
    {
        obstacles.Clear();
        Collider[] hitColliders = Physics.OverlapBox(vehicle.transform.position + (vehicle.transform.forward * 2.5f), new Vector3(.5f, .5f, 2.5f), new Quaternion(0,0,0,0), ObstacleMask);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            obstacles.Add(hitColliders[i]);
        }
        if (obstacles.Count == 1)
        {
            closestObstacle = obstacles[0];
        }
        else if (obstacles.Count > 1)
        {
            closestObstacle = obstacles[0];
            for (int i = 0; i < obstacles.Count; i++)
            {
                if (Vector3.Distance(transform.position, closestObstacle.transform.position) > Vector3.Distance(transform.position, obstacles[i].transform.position))
                {
                    closestObstacle = obstacles[i];
                }
            }
        }
        else
        {
            closestObstacle = null;
        }
    }

    public Vector3 ObstacleAvoidance()
    {
        double forceMultiplier = 0;
        Vector3 steeringForce = new Vector3(0f, 0f, 0f);
        if (closestObstacle != null)
        {
            forceMultiplier = 1.0 + (5 - closestObstacle.transform.localPosition.x) / 5;
            steeringForce.z = ((closestObstacle.transform.localScale.x + closestObstacle.transform.localScale.z / 4) - closestObstacle.transform.localPosition.z) * (float)forceMultiplier;
            float breakingWeight = .2f;
            steeringForce.x = ((closestObstacle.transform.localScale.x + closestObstacle.transform.localScale.z / 4) - closestObstacle.transform.localPosition.x) * breakingWeight;
        }
        transform.TransformPoint(steeringForce);
        return steeringForce;
    }

    public Vector3 WallAvoidance()
    {
        Vector3 steeringForce = new Vector3(0, 0, 0);
        float penetrationDistance = 0;
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, transform.forward, out hit, 4))
        {
            if (hit.transform.gameObject.tag == "Wall")
            {
                if (hit.transform != transform)
                {
                    Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);
                    penetrationDistance = hit.distance;
                }
            }
        }

        else  if (Physics.Raycast(transform.position, transform.forward + transform.right, out hit, 4))
        {
            if (hit.transform.gameObject.tag == "Wall")
            {
                if (hit.transform != transform)
                {
                    Debug.DrawRay(transform.position, transform.forward * (hit.distance / 2) + transform.right * (hit.distance / 2), Color.blue);
                    penetrationDistance = hit.distance;
                }
            }
        }

        else if (Physics.Raycast(transform.position, transform.forward - transform.right, out hit, 4))
        {
            if (hit.transform.gameObject.tag == "Wall")
            {
                if (hit.transform != transform)
                {
                    Debug.DrawRay(transform.position, transform.forward * (hit.distance / 2) - transform.right * (hit.distance / 2), Color.yellow);
                    penetrationDistance = hit.distance;
                }
            }
        }
        
        steeringForce = hit.point.normalized * penetrationDistance;
        return steeringForce;
    }

    public void FindHidingSpot()
    {
        Vector3 hidingSpot;
        GameObject[] spots = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach(GameObject i in spots)
        {
            distanceAway = (i.transform.lossyScale.x / 2) + distanceFromBoundary; //Find the Radius of the obstacle and add the distance from boundary
            toObstacle = (i.transform.position - hidingFrom.transform.position).normalized;
            hidingSpot = (toObstacle * distanceAway) + i.transform.position;
            if (Vector3.Distance(transform.position, BestHidingSpot) > Vector3.Distance(transform.position, hidingSpot))
            {
                BestHidingSpot = hidingSpot;
            }
        }
    }

    public void FindNeighbours()
    {
        neighbours.Clear();
        Collider[] hitColliders = Physics.OverlapSphere(vehicle.transform.position, 25, FlockMask);
        for(int i = 0; i < hitColliders.Length; i++)
        {
            neighbours.Add(hitColliders[i]);
        }
    }
    
    public Vector3 SeperationFlocking()
    {
        Vector3 steeringForce = new Vector3(0f, 0f, 0f);
        for(int i = 0; i < neighbours.Count; i++)
        {
            if (neighbours[i].transform.position != transform.position)
            {
                Vector3 toOurAgent = transform.position - neighbours[i].transform.position;
                float distance = toOurAgent.magnitude;
                if (distance <= 2.5f)
                {
                    distance = distance / 10;
                }
                if (distance <= 1.25f)
                {
                    distance = distance / 100;
                }
                toOurAgent = toOurAgent.normalized;
                toOurAgent = toOurAgent / distance;
                steeringForce += toOurAgent;
            }
        }
        return steeringForce;
    }

    public Vector3 AlignmentFlocking()
    {
        Vector3 averageHeading = new Vector3 (0f, 0f, 0f);
        for (int i = 0; i < neighbours.Count; i++)
        {
            averageHeading += neighbours[i].transform.forward;
        }
        if (neighbours.Count > 1)
        {
            averageHeading /= neighbours.Count;
            averageHeading -= transform.forward;
        }

        return averageHeading;
    }

    public Vector3 CohesionFlocking()
    {
        Vector3 averagePosition = new Vector3(0f, 0f, 0f);
        Vector3 steeringForce = new Vector3(0f, 0f, 0f);

        for (int i = 0; i < neighbours.Count; i++)
        {
            if (neighbours[i].transform.position != transform.position)
            {
                averagePosition += neighbours[i].GetComponent<Transform>().position;
            }           
        }
        if (neighbours.Count > 1)
        {
            averagePosition /= (neighbours.Count - 1);
        }
        steeringForce = averagePosition - transform.position;
        return steeringForce;
    }
}
