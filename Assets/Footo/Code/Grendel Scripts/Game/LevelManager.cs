using UnityEngine;
using System.Collections;

public class LevelManager : Singleton<LevelManager> 
{
	
	public bool RandomMusicTrack = false;
	public AudioClip BackgroundMusicTrack;	
	
	private int _musicTrackIndex; //the index of the currently set music track
	
	[SerializeField]
	public int MusicTrackIndex
	{
		get{ return _musicTrackIndex; }
		set{ _musicTrackIndex = value; }
	}
	
	// Use this for initialization
	void Start () 
	{
		Console.Instance.OutputToConsole(string.Format("LevelManager: {0} loaded, calling music track.", Application.loadedLevelName), Console.Instance.Style_Admin);
		PlayBackgroundMusicTrack();		
	}
	
	// Update is called once per frame
//	void Update () 
//	{
//		Debug.Log(_musicTrackIndex);
//	}
	
	void PlayBackgroundMusicTrack()
	{
		if (RandomMusicTrack) 
		{ 
			AudioManager.Instance.PlayMusicTrack( AudioList.Instance.MusicTracks[Random.Range(0, AudioList.Instance.MusicTracks.Count)] ); 
		}
		else
		{			
			AudioManager.Instance.PlayMusicTrack(BackgroundMusicTrack);
		}
	}
}
