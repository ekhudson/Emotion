using System.Collections;

using UnityEngine;
using UnityEditor;

public class GrendelPrefencesWindow 
{
	[PreferenceItem ("Grendel")]
	public static void PreferencesGUI()
	{
		EditorGUI.BeginChangeCheck();
		
		GrendelEditorPreferences.DrawEditorObjectLabels = GUILayout.Toggle(GrendelEditorPreferences.DrawEditorObjectLabels, "Draw Editor Object Labels");
		
		if (EditorGUI.EndChangeCheck())
		{
			SceneView.currentDrawingSceneView.Repaint(); 
		}
	}
}
