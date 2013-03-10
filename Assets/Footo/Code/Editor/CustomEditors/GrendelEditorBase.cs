using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

public class GrendelEditorBase<T> : Editor where T : class 
{	
	public T Target;
		
	private void OnEnable()
	{
		Target = target as T;
	}
	
	private void OnDisable()
	{
		
	}		
}
