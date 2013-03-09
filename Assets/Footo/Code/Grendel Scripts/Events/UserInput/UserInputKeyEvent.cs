using UnityEngine;
using System.Collections;

public class UserInputKeyEvent : EventBase
{
	public enum TYPE
	{
		KEYDOWN,
		KEYHELD,
		KEYUP,		
	}
	
	public readonly TYPE Type;
	public readonly KeyCode Key;
	
	public UserInputKeyEvent(TYPE inputType, KeyCode key, Vector3 location, object sender) : base(location, sender)
	{
		Type = inputType;
		Key = key;
	}
	
	public UserInputKeyEvent() : base (Vector3.zero, null)
	{		
		
	}
}