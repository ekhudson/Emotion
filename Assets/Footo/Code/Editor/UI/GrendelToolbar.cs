using System.Collections;

using UnityEditor;
using UnityEngine;

public class GrendelToolbar : Editor
{
	private static bool _toolbarEnabled = true;
	private static bool _objectPlacer = false;
	private static int _buttonSize = 64;
	private static GUILayoutOption[] buttonSizes = new GUILayoutOption[]{ GUILayout.Width(_buttonSize), GUILayout.Height(_buttonSize) };
	//private static GameObject _objectToPlace = null;
	
	[MenuItem ("Grendel/Show Toolbar")]	
	static void Init() 
	{		
		if(SceneView.onSceneGUIDelegate != OnSceneGUI)
	    {
	       SceneView.onSceneGUIDelegate += OnSceneGUI;
			_toolbarEnabled = true;
	    }
		else
		{
			SceneView.onSceneGUIDelegate -= OnSceneGUI;
			_toolbarEnabled = false;
		}
    }	
	
	static void OnSceneGUI (SceneView scnView) 
	{	
		if (_toolbarEnabled)
		{		
			GUI.Window(110, new Rect(0,16, 256, _buttonSize), ToolbarWindow,"", GUI.skin.label);
		}
    }
	
	static void ToolbarWindow(int windowID)
	{
		//GUILayoutOption[] buttonSizes = new GUILayoutOption[]{ GUILayout.Width(32), GUILayout.Height(32) };
		
		EditorGUILayout.BeginHorizontal();
				
		if (GUILayout.Button(new GUIContent("", Resources.LoadAssetAtPath("Assets/Textures/Grendel Textures/Grendel_Icon_Large_White.png", typeof(Texture2D)) as Texture2D), 
											GUI.skin.label, GUILayout.Width(_buttonSize))
			)
		{
			Init();	
		}
		_objectPlacer = GUILayout.Toggle(_objectPlacer, new GUIContent("O"), GUI.skin.button, buttonSizes);
		GUILayout.Button("2", buttonSizes);
		GUILayout.Button("3", buttonSizes);
		
		if (_objectPlacer)
		{
			GUI.Window(120, new Rect(_buttonSize,16,_buttonSize + 10, 512), ObjectPlacer,"", GUI.skin.label);
		}
		
		EditorGUILayout.EndHorizontal();
	}
	
	static void ObjectPlacer(int windowID)
	{		
		EditorGUILayout.BeginVertical(GUI.skin.box);
			
			GUILayout.Button("", GUI.skin.label, buttonSizes);
		
			GUILayout.Button(new GUIContent("", Resources.LoadAssetAtPath("Assets/Gizmos/Gizmo_Trigger.png", typeof(Texture2D)) as Texture2D), buttonSizes);			
		
			GUILayout.Button(new GUIContent("", Resources.LoadAssetAtPath("Assets/Gizmos/Gizmo_Spawner.png", typeof(Texture2D)) as Texture2D), buttonSizes);
		
		EditorGUILayout.EndVertical();
	}		
}

