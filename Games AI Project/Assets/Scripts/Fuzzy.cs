using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuzzy : MonoBehaviour
{

    public float RVLeftShoulder = 5f, RVTrapezoid = 50f, RVRightShoulder = 95f;
    //public float health;

    private void Start()
    {
        //health = 100f;
    }

    private void Update()
    {
        //health -= Time.deltaTime;

        //DOMLeftShoulder = LeftShoulderMembership(health);
        //DOMTrapezoid = TrapezoidMembership(health);
        //DOMRightShoulder = RightShoulderMembership(health);
    }

    // Membership functions
    #region

    /// <summary>
    /// Danger Zone - 
    /// lowestValue = 100% true, maxValue = 100% false membership
    /// </summary>
    public float LeftShoulderMembership(float crispValue, float lowestValue = 10f, float maxValue = 45f)
    {
        float membership;

        if (crispValue <= lowestValue)
        {
            return membership = 1f;
        }
        else if (crispValue >= maxValue)
        {
            return membership = 0f;
        }
        else
        {
            return membership = (-crispValue / (maxValue - lowestValue)) + (maxValue / (maxValue - lowestValue));
        }
    }

    /// <summary>
    /// OK State - 
    /// lowestValue = 100% false, first and second peak = 100% true, maxValue = 100% false
    /// </summary>
    public float TrapezoidMembership(float crispValue, float lowestValue = 10f, float firstPeak = 45f, float secondPeak = 55f, float maxValue = 90f)
    {
        float membership;

        if (crispValue <= lowestValue || crispValue >= maxValue)
        {
            return membership = 0f;
        }
        else if (crispValue >= firstPeak && crispValue <= secondPeak)
        {
            return membership = 1f;
        }
        else if (crispValue >= lowestValue && crispValue <= firstPeak)
        {
            return membership = (crispValue / (firstPeak - lowestValue)) - (lowestValue / (firstPeak - lowestValue));
        }
        else
        {
            return membership = (-crispValue / (maxValue - secondPeak)) + (maxValue / (maxValue - secondPeak));
        }
    }

    /// <summary>
    /// Perfectly Fine State -
    /// lowestValue = 100% false, maxValue = 100% true membership
    /// </summary>
    public float RightShoulderMembership(float crispValue, float lowestValue = 55f, float maxValue = 90f)
    {
        float membership;

        if (crispValue >= maxValue)
        {
            return membership = 1f;
        }
        else if (crispValue <= lowestValue)
        {
            return membership = 0f;
        }
        else
        {
            return membership = (crispValue / (maxValue - lowestValue)) - (lowestValue / (maxValue - lowestValue));
        }
    }

    #endregion

    //Fuzzy Logical Operators
    #region
        /// <summary>
        /// Picks the lowest value
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
    public float FLO_AND(float a, float b)
    {
        if (a >= b)
        {
            return b;
        }
        else
        {
            return a;
        }
    }

    /// <summary>
    /// Picks the highest value
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public float FLO_OR(float a, float b)
    {
        if (a >= b)
        {
            return a;
        }
        else
        {
            return b;
        }
    }

    public float FLO_NOT(float a)
    {
        return 1f - a;
    }

    #endregion
}
