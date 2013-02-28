using UnityEngine;
using System.Collections;
using System.Collections.Generic;

	/// <summary>
	/// Title: Grendel Engine
	/// Author: Elliot Hudson
	/// Date: Apr 2, 2012
	/// 
	/// Filename: AudioManager.cs
	/// 
	/// Summary: Handles playing sound effects / music in the scene
	///  
	/// </summary>

public class AudioManager : Singleton<AudioManager> 
{
	
	public float GlobalVolumeSFX = 1f;
	public float GlobalVolumeMusic = 1f;
	
	public int MaxPlayingMusicTracks = 3;
	
	//private List<AudioClip> _currentPlayingMusicTracks = new List<AudioClip>();
	
	private Dictionary<int, AudioSource> AudioDictionary = new Dictionary<int, AudioSource>();
	private int MusicAudioSourceID = 0;
	
	
	// Use this for initialization
	void Start () 
	{		
		Console.Instance.OutputToConsole(string.Format("AudioManager: Recognizing {0} Music Tracks and {1} Sound Effects.", AudioList.Instance.MusicTracks.Count, AudioList.Instance.SFX.Length), Console.Instance.Style_Admin);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//not sure if I need this yet
	}
	
	public void VolumeUp()
	{
		Mathf.Clamp(GlobalVolumeSFX += 0.1f, 0, 1);
		Mathf.Clamp(GlobalVolumeMusic += 0.1f, 0, 1);
		Console.Instance.OutputToConsole(System.String.Format("Volume Up - Music: {0} SFX: {1}", GlobalVolumeMusic, GlobalVolumeSFX), Console.Instance.Style_Admin);
		UpdateAudio();
	}
	
	public void VolumeDown()
	{
		Mathf.Clamp(GlobalVolumeSFX -= 0.1f, 0, 1);
		Mathf.Clamp(GlobalVolumeMusic -= 0.1f, 0, 1);
		Console.Instance.OutputToConsole(System.String.Format("Volume Down - Music: {0} SFX: {1}", GlobalVolumeMusic, GlobalVolumeSFX), Console.Instance.Style_Admin);
		UpdateAudio();
	}
	
	public void UpdateAudio()
	{		
		foreach(AudioSource source in AudioDictionary.Values)
		{
			source.volume = GlobalVolumeSFX = GlobalVolumeMusic; //TODO: Separate these	
		}
	}
	
	public void PlayMusicTrack(AudioClip musicTrack)
	{
		AudioSource source;
		
		if (!AudioDictionary.ContainsKey(MusicAudioSourceID))
		{		
			source = gameObject.AddComponent<AudioSource>();
			MusicAudioSourceID = source.GetInstanceID();
			AudioDictionary.Add(MusicAudioSourceID, source);
		}
		else
		{
			source = AudioDictionary[MusicAudioSourceID];
		}
		
		source.clip = musicTrack;
		source.volume = GlobalVolumeMusic;
		
		try
		{
			source.Play(1000);
			Console.Instance.OutputToConsole("AudioManager: Playing Music Track: " + musicTrack.name, Console.Instance.Style_Admin);
		}
		catch
		{
			Console.Instance.OutputToConsole("AudioManager: Error Playing Music Track: " + musicTrack.name, Console.Instance.Style_Error);
		}	
		
	}
	
	public void IncrementMusicTrack(int increment)
	{
		if (MusicAudioSourceID != 0)
		{
			int index = AudioList.Instance.MusicTracks.IndexOf( AudioDictionary[MusicAudioSourceID].clip );
			index += increment;
			
			if (index > (AudioList.Instance.MusicTracks.Count - 1)){ index = 0; } //limit going up in tracks
			if (index < 0) { index = (AudioList.Instance.MusicTracks.Count - 1); } //limit going down in tracks
			
			PlayMusicTrack(  AudioList.Instance.MusicTracks[ index ] );
		}
		else
		{
			PlayMusicTrack(  AudioList.Instance.MusicTracks[0] );
		}
	}
}
