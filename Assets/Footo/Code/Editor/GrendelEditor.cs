using System.Collections;

using UnityEngine;
using UnityEditor;

public class GrendelEditor<T> : Editor where T : class
{	
	public T Target
	{
		get{return target as T;}		
	}
	
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();	
	}
}
