using UnityEngine;
using System.Collections;

[System.Serializable]
public class TriggerEventExit : TriggerEventBase
{
	public TriggerEventExit(Collider collider)
	{	
		_collider = collider;
	}	
	
	public TriggerEventExit()
	{	
		_collider = null;
	}
}
