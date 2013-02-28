using UnityEngine;
using System.Collections;

[System.Serializable]
public class TriggerEventEnter : TriggerEventBase
{
   	
	public TriggerEventEnter(Collider collider)
	{		
		_collider = collider;
	}
	
	public TriggerEventEnter()
	{		
		_collider = null;
	}
}
