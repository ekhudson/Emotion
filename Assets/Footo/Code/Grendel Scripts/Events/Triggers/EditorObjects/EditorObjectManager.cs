using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EditorObjectManager : Singleton<EditorObjectManager> 
{	
	private Dictionary<EditorObject, List<EditorObjectConnection>> _connectionRegistry = new Dictionary<EditorObject, List<EditorObjectConnection>>();	
	
	public Dictionary<EditorObject, List<EditorObjectConnection>> ConnectionRegistry
	{
		get { return _connectionRegistry; }
	}
	
	public static EditorObjectManager DesignInstance
	{
		get
		{
			return GameObject.Find("GameManager").GetComponent<EditorObjectManager>();
		}
	}
	
	public bool ContainsConnection(EditorObject subject, EditorObject caller)
	{
		bool connectionExists = false;
		
		if (_connectionRegistry.ContainsKey(caller))
		{
			foreach(EditorObjectConnection connection in _connectionRegistry[caller])
			{
				if (connection.Subject == subject)
				{
					connectionExists = true;
				}			
			}
		}
		
		return connectionExists;			
	}
	
//	public void AddConnection(EditorObject subject, EditorObject caller, EditorObject.EditorObjectMessage message)
//	{		
//		EditorObjectConnection newConnection = new EditorObjectConnection(message, caller, subject);
//				
//		if (_connectionRegistry.ContainsKey(caller))
//		{
//			
//			foreach(EditorObjectConnection connection in _connectionRegistry[caller])
//			{
//				if (connection.Subject == subject) //check if a connection already exists between these two objects
//				{
//					return;
//				}
//			}
//			
//			_connectionRegistry[caller].Add(newConnection);
//		}
//		else
//		{			
//			_connectionRegistry.Add(caller, new List<EditorObjectConnection>(){newConnection});		
//		}
//	}
//	
//	public void RemoveConnection(EditorObject subject, EditorObject caller)
//	{
//		
//	}	
	
	
}
