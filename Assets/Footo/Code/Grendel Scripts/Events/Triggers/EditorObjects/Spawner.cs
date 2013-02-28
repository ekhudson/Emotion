using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Spawner : EditorObject, IEditorObject 
{	
	public List<GameObject> EntitiesToSpawn = new List<GameObject>();

	// Use this for initialization
	protected override void Start () 
	{
		base.Start();
		
	}
			
	public void OnHear(object sender, TriggerEventBase evt)
	{
		//Debug.Log("Hear Event " + evt.evt.ToString() + " with " + evt.GetCollider + evt.Place);
	}
	
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		
		if (Application.isPlaying) { OnPlayGizmos(); } else { OnEditGizmos(); }	
		_gizmoName = "Gizmo_Spawner";
			
	}
	
	public override void OnActivate(object caller, EventBase evt) //called when the editor object is activated
	{		
		base.OnActivate(caller, evt);
		
		foreach(GameObject entityToSpawn in EntitiesToSpawn)
		{
			GameObject.Instantiate(entityToSpawn, transform.position, Quaternion.identity); 
		}
	}
	
	public override void OnDeactivate(object caller, EventBase evt) //called when the editor object is deactivated
	{
		base.OnDeactivate(caller, evt);
	}
	
	public override void OnToggle(object caller, EventBase evt) //called when the editor object is toggled
	{
		base.OnToggle(caller, evt);
	}
	
	public override void OnEnabled(object caller, EventBase evt) //called when the editor object is deactivated
	{
		base.OnEnabled(caller, evt);
	}
	
	public override void OnDisabled(object caller, EventBase evt) //called when the editor object is toggled
	{
		base.OnDisabled(caller, evt);
	}
	
	
	
}
