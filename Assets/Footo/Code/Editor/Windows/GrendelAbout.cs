using System.Collections;

using UnityEngine;
using UnityEditor;

public class GrendelAbout : EditorWindow
{
	private const string kAboutWindowTitle = "About Grendel";
	private static Rect kAboutPosition = new Rect(64f, 64f, 128f, 128f);
	private static Vector2 kAboutSize = new Vector2(160f, 256f);
	private static GrendelAbout AboutWindow = (GrendelAbout)ScriptableObject.CreateInstance(typeof(GrendelAbout));
	private const string kGrendelIconPath = "Assets/Textures/Grendel Textures/Grendel_Icon_Large_White.png";
	private static Rect kGrendelIconPosition = new Rect(16f, 16f, 128f, 128f);
	
	[MenuItem ("Help/About Grendel...")]
	public static void Init()
	{ 			
		AboutWindow.ShowUtility();		
		AboutWindow.title = kAboutWindowTitle;		
		AboutWindow.position = kAboutPosition;
		AboutWindow.minSize = kAboutSize;
		AboutWindow.maxSize = kAboutSize;			
	}
	
	private void OnGUI()
	{
		GUI.color = Color.white;
		GUILayout.BeginHorizontal();
		
			GUILayout.BeginVertical();
				GUILayoutUtility.GetRect(kGrendelIconPosition.width + 16f, kGrendelIconPosition.height + 16f);				
		
				GUI.DrawTexture(kGrendelIconPosition, (Texture)AssetDatabase.LoadAssetAtPath(kGrendelIconPath,typeof(Texture)));
				
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
					GUILayout.Label("Grendel Framework 1.0", EditorStyles.boldLabel);
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
		
				EditorGUILayout.Space();
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
					GUILayout.Label("by Elliot Hudson");
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();				
				
				EditorGUILayout.Space();
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
					GUILayout.Label("\u00A9 2012 Elliot Hudson");
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
		
				EditorGUILayout.Space();
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				if(GUILayout.Button("www.ekhudson.com", GUI.skin.label))				
				{
					Help.BrowseURL("http://www.ekhudson.com");
				}
		
				if(GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
				{
					EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
				}
				
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
		
			GUILayout.EndVertical();
		
		GUILayout.EndHorizontal();
	}
	
}
