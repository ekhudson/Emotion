using UnityEngine;
using System.Collections;

[System.Serializable]
public class TriggerEventStay : TriggerEventBase 
{	
	public TriggerEventStay(Collider collider)
	{		
		_collider = collider;
	}
	
	public TriggerEventStay()
	{	
		_collider = null;
	}
}
