using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EditorObject : MonoBehaviour, IEditorObject
{
	public bool DebugMode = true;
	[HideInInspector]public string Comment = "";
	public int NumberOfUses = -1; //-1 == infinite
	
	protected Transform _transform;
	protected GameObject _gameObject;
	//protected GUIStyle SelectedTextStyle = new GUIStyle();
	protected int _labelWidth = 128;
	//protected int _labelHeight = 32;
	protected GameObject _currentActiveObject;
	
	[HideInInspector]
	public bool LookingForSubject = false;
	
	public static EditorObject CurrentHoveredEditorObject;
	
	[HideInInspector]
	public bool ActivateHighlight = false; //the highlight to show when in editor when this object is going to be activated by another	
	[HideInInspector]
	public bool DeactivateHighlight = false; //the highlight to show when in editor when this object is going to be deactivated by another
	[HideInInspector]
	public bool ToggleHighlight = false; //the highlight to show when in editor when this object is going to be toggled by another
	[HideInInspector]
	public bool HighlightHighlight = false; //the highlight to show when in editor when this object highlight via the info panel
		
	protected Camera _cameraToUse;
	protected string _gizmoName = "";
	protected bool _nameConflict = false; //used by the editor to flag this object if it has a name conflict with another object
	protected EventTransceiver.Events[] _associatedEvents;
	
	private bool _clearConnections = false;
	
	[HideInInspector]
	public Vector2 InfoScrollPos;
	
	[System.Serializable]
	public enum EditorObjectMessage
	{
		None,
		Activate, //turn on the editor object
		Deactivate, //turn off the editor object
		Toggle, //toggle the state of the editor object
		Enable, //enable a disabled editor object
		Disable //disable an enabled editor object		
	}
	
	public enum EDITOROBJECTSTATES 
	{
		ACTIVE, //has been activated and is running
		INACTIVE, //not running and awaiting activation
		DISABLED //will not receive any messages from other editor objects
	};
	
	protected EDITOROBJECTSTATES _state = EDITOROBJECTSTATES.INACTIVE;
	
	public EDITOROBJECTSTATES State
	{
		get {return _state;}
		set {_state = value;}
	}
	
	public bool NameConflict
	{
		get {return _nameConflict;}
		set {_nameConflict = value;}
	}
	
	public string GizmoName
	{
		get {return _gizmoName;}		
	}
	
	//returns events associated with this object
	virtual public EventTransceiver.Events[] AssociatedEvents
	{
		get {return null;}
	}
	
	public EditorObject() : base()
	{
		_clearConnections = true;				
	}
	
	virtual protected void Awake ()
	{
		
	}
	
	// Use this for initialization
	virtual protected void Start () 
	{
	
	}
	
	// Update is called once per frame
	virtual protected void Update () 
	{
	
	}	
	
	virtual public void OnSceneGUI()
	{		
		
	}
	
//	public void DrawSimpleLabel(Camera cameraToUse)
//	{		
//		Vector3 distance = cameraToUse.transform.position - transform.position;	
//		_cameraToUse = cameraToUse;
//		
//		SelectedTextStyle = GUI.skin.button;
//		SelectedTextStyle.normal.textColor = Color.yellow;
//		Vector2 screenCoords = cameraToUse.WorldToScreenPoint(transform.position);
//		screenCoords.y = cameraToUse.pixelHeight - screenCoords.y;			
//		
//		Rect labelRect = new Rect(screenCoords.x - (_labelWidth * 0.5f), screenCoords.y - (_labelHeight * 0.5f), _labelWidth, _labelHeight);
//	}
		
	public void LabelWindow(int windowID)
	{		
		//maybe put some kind of pop-up info here when the object is hovered over
	}
	
	virtual protected void OnDrawGizmos()
	{		
		if (!DebugMode) { return; }
		
		switch(_state)
		{
			case EDITOROBJECTSTATES.ACTIVE:
			
				Gizmos.DrawIcon(transform.position, "Gizmo_Active");
			
			break;
			
			case EDITOROBJECTSTATES.INACTIVE:
			
				Gizmos.DrawIcon(transform.position, "Gizmo_Inactive");
			
			break;
			
			case EDITOROBJECTSTATES.DISABLED:
			
				Gizmos.DrawIcon(transform.position, "Gizmo_Disabled");
			
			break;
		}
		
		if (_clearConnections)
		{
			//Connections.Clear();
			//_clearConnections = false;
		}
		
		//OnEditGizmos();
	}
	
	virtual protected void OnPlayGizmos()
	{
		
	}
	
	virtual protected void OnEditGizmos()
	{
		
	}
	
//	public void AcceptMessage(EditorObjectMessage message, EditorObject caller)
//	{
//		switch (message)
//		{
//			case EditorObjectMessage.Activate:
//				OnActivate(caller);
//			break;
//			
//			case EditorObjectMessage.Deactivate:
//				OnDeactivate(caller);
//			break;
//			
//			case EditorObjectMessage.Toggle:
//				OnToggle(caller);
//			break;
//			
//			case EditorObjectMessage.Disable:
//				OnDisabled(caller);
//			break;
//			
//			case EditorObjectMessage.Enable:
//				OnEnabled(caller);
//			break;
//			
//			default:
//				Debug.LogWarning(string.Format("Message {0} from caller {1} to subject {2} unrecognized.",
//								 message.ToString(), caller.name, this.name));
//			break;			
//		}
//	}
	
	public void SetState(EDITOROBJECTSTATES state)
	{
		switch (state)
		{
			
			case EDITOROBJECTSTATES.ACTIVE:
				//stuff to do when going active
			break;
			
			case EDITOROBJECTSTATES.INACTIVE:
				//stuff to do when going inactive
			break;
			
			case EDITOROBJECTSTATES.DISABLED:
				//stuff to do when going disabled
			break;	
		}
	}
	
	//This EditorObject has been Activated
	virtual public void OnActivate(object caller, EventBase evt)
	{		
		switch(_state)
		{
			case EDITOROBJECTSTATES.ACTIVE:
			
			break;
			
			case EDITOROBJECTSTATES.INACTIVE:
			
			break;
			
			case EDITOROBJECTSTATES.DISABLED:
			
			break;
		}
	}
	
	//This EditorObject has been Deactivated
	virtual public void OnDeactivate(object caller, EventBase evt)
	{		
		switch(_state)
		{
			case EDITOROBJECTSTATES.ACTIVE:
			
			break;
			
			case EDITOROBJECTSTATES.INACTIVE:
			
			break;
			
			case EDITOROBJECTSTATES.DISABLED:
			
			break;
		}		
	}
	
	//This EditorObject has been Toggled
	virtual public void OnToggle(object caller, EventBase evt)
	{
		switch(_state)
		{
			case EDITOROBJECTSTATES.ACTIVE:
			
			break;
			
			case EDITOROBJECTSTATES.INACTIVE:
			
			break;
			
			case EDITOROBJECTSTATES.DISABLED:
			
			break;
		}		
	}
	
	virtual public void OnEnabled(object caller, EventBase evt)
	{
		switch(_state)
		{
			case EDITOROBJECTSTATES.ACTIVE:
			
			break;
			
			case EDITOROBJECTSTATES.INACTIVE:
			
			break;
			
			case EDITOROBJECTSTATES.DISABLED:
			
			break;
		}		
	}
	
	virtual public void OnDisabled(object caller, EventBase evt)
	{
		switch(_state)
		{
			case EDITOROBJECTSTATES.ACTIVE:
			
			break;
			
			case EDITOROBJECTSTATES.INACTIVE:
			
			break;
			
			case EDITOROBJECTSTATES.DISABLED:
			
			break;
		}		
	}	

	//returns false if there are no more uses left
	private bool DecrementUses()
	{
		if (NumberOfUses > 0)
		{
			if (NumberOfUses == -1)
			{
				//do not decrement
				return true;
			}
			else
			{
				NumberOfUses--; //do decrement
				return true;
			}
		}
		else
		{
			return false; //no uses left
		}
	}
}//end class
