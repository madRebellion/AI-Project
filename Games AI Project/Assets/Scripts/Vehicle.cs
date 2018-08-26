using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteeringBehaviours))]
public class Vehicle : MonoBehaviour {

    /////////////////////
    //Updated Values
    /////////////////////
    /// <summary>
    /// This is applied to the current position every frame
    /// </summary>
    public Vector3 Velocity;

    //Position, Heading and Side can be accessed from the transform component with transform.position, transform.forward and transform.right respectively

    //"Constant" values, they are public so we can adjust them through the editor

    //Represents the weight of an object, will effect its acceleration
    public float Mass = 1;

    //The maximum speed this agent can move per second
    public float MaxSpeed = 7;

    //The thrust this agent can produce
    public float MaxForce = 1.5f;

    //We use this to determine how fast the agent can turn, but just ignore it for, we won't be using it
    public float MaxTurnRate = 1.0f;

    public float previousYaw = 0;

    public Vector3 newRotation;

    private SteeringBehaviours SB;

    // Use this for initialization
    void Start ()
    {
        SB = GetComponent<SteeringBehaviours>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 SteeringForce = Vector3.ClampMagnitude(SB.Calculate(), MaxForce);

        Vector3 Acceleration = SteeringForce / Mass;

        Velocity += Acceleration;

        Velocity = Vector3.ClampMagnitude(Velocity, MaxSpeed);

        newRotation = Quaternion.LookRotation(new Vector3(Velocity.x + .00001f, Velocity.y, Velocity.z) , Vector3.up).eulerAngles;

        bool tempNeg = false;
        bool tempPos = false; 

        if (previousYaw < newRotation.y - 180)
        {
            tempNeg = true;
        }
        if (previousYaw > newRotation.y + 180)
        {
            tempPos = true;
        }

        if (tempNeg == true)
        {
            if ((newRotation.y + 360) - (previousYaw + 360) < (-MaxTurnRate + 360))
            {
                newRotation.x = 0;
                newRotation.y = previousYaw - MaxTurnRate;
                newRotation.z = 0;
            }
        }
        else if (tempPos == true)
        {
            if ((newRotation.y + 360) - previousYaw > (MaxTurnRate + 360))
            {
                newRotation.x = 0;
                newRotation.y = previousYaw + MaxTurnRate;
                newRotation.z = 0;
            }
        }
        else if ((newRotation.y + 360) - previousYaw > (MaxTurnRate + 360))
        {
            newRotation.x = 0;
            newRotation.y = previousYaw + MaxTurnRate;
            newRotation.z = 0;
        } else if ((newRotation.y + 360) - previousYaw < (-MaxTurnRate + 360))
        {
            newRotation.x = 0;
            newRotation.y = previousYaw - MaxTurnRate;
            newRotation.z = 0;
        }


        previousYaw = newRotation.y;

        

        Velocity = (Quaternion.Euler(newRotation) * Vector3.forward) * Velocity.magnitude;

        if (Velocity != Vector3.zero)
        {
            transform.position += Velocity * Time.deltaTime;

            transform.forward = Velocity.normalized;
            transform.rotation = Quaternion.LookRotation(Velocity);
        }

        //This kind of works for keeping the capsule upright though not entirely however it is no longer an obvious x rotate
        if(transform.rotation.x != 0)
        {
            transform.Rotate(-transform.rotation.x * 100, 0, 0);
        }

        //transform.right should update on its own once we update the transform.forward
	}
}
