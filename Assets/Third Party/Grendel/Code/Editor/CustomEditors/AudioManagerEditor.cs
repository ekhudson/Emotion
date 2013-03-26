using System.Collections;
using System.IO;

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : Editor {	
	
	public AudioManager _target;
	
	void OnEnable()
	{
		_target = (AudioManager)target;
		FindMusicFiles();
	}
	
	// Override the GUI
	public override void OnInspectorGUI()
	{
		GUI.changed = false;
		_target.GlobalVolumeMusic = EditorGUILayout.Slider("Global Music Volume: ", (target as AudioManager).GlobalVolumeMusic, 0f, 1f);
		_target.GlobalVolumeSFX = EditorGUILayout.Slider("Global SFX Volume: ", (target as AudioManager).GlobalVolumeSFX, 0f, 1f);
		if (GUI.changed){ EditorUtility.SetDirty( target ); }	
	}	
	
	public static void FindMusicFiles()
	{			
		//string[] filePaths = Directory.GetFiles(Application.dataPath + "/Audio/Music/","*.mp3");
		//Object[] musicFiles = AssetDatabase.LoadAllAssetsAtPath("Assets/Audio/Music/");	
		FileInfo[] ScenesFileInfo = (new DirectoryInfo(Application.dataPath + "/Audio/Music/")).GetFiles("*.mp3", SearchOption.AllDirectories);
		
		
		//if (musicFiles != null)
		//{
			
			foreach (FileInfo fileInfo in ScenesFileInfo)
			{//
				
				//WWW www = new WWW(path);
				//AudioClip tempClip = AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip) ) as AudioClip;
				AudioClip tempClip = AssetDatabase.LoadAssetAtPath("Assets/Audio/Music/" + fileInfo.Name, typeof(AudioClip) ) as AudioClip;
				AssetDatabase.Refresh();			
				if (tempClip != null)
				{
					
					if( (GameObject.FindObjectOfType(typeof(AudioList))as AudioList).MusicTracks.Contains(tempClip) )
					{
						continue;
					}
					else
					{
						(GameObject.FindObjectOfType(typeof(AudioList))as AudioList).MusicTracks.Add(tempClip);
					}
				}
				else
				{
					Debug.LogWarning("Music File " + fileInfo.ToString() + " not found.");
				}
			}
		//}
	}
}



