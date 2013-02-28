using UnityEngine;
using System;
   
[System.Serializable]
public class EventBase
{
    // Time at which the event occured.
    public readonly float Time;

    // Location at which the event occurred.
    public readonly Vector3 Place;	
	
    // Create a new event.
    protected EventBase() : this(Vector2.zero)
    {
    }

    // Create a new event at a location in the world.
    protected EventBase(Vector2 place)
    {
        Time = UnityEngine.Time.fixedTime; 
		//Time = time;
        Place = place;
    }
}
