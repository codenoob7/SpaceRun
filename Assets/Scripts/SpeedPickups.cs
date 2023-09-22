using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum type
{
    SpeedUp,
    SpeedBreaker
}

public class SpeedPickups : MonoBehaviour
{
    public type speedType;
    public float increaseAnimOffset;
    public float increaseSpeedOffset;


    void Start()
    {
       if(speedType == type.SpeedBreaker)
        {
            increaseAnimOffset = -increaseAnimOffset;
            increaseSpeedOffset = -increaseSpeedOffset;
        }
    }
}
