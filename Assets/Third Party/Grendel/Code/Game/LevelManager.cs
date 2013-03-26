using System.Collections;

using UnityEngine;

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

    protected override void Awake()
    {
        if (_instance)
        {
            AudioManager.Instance.PlayMusicTrack(BackgroundMusicTrack);
        }

        base.Awake();
    }

	
	// Use this for initialization
	void Start () 
	{
		Console.Instance.OutputToConsole(string.Format("{0}: {1} loaded, calling music track", this.ToString(), Application.loadedLevelName), Console.Instance.Style_Admin);
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
	
	public void LoadLevel(string sceneName)
	{
		if (Application.loadedLevelName == sceneName)
		{
			Console.Instance.OutputToConsole(string.Format("{0}: Asked to load scene {1}, but that scene is already loaded.", this.ToString(), sceneName), Console.Instance.Style_Error);		
		}
		
		Console.Instance.OutputToConsole(string.Format("{0}: Loading scene {1}.", this.ToString(), sceneName), Console.Instance.Style_Admin);		
		
		try
		{			
			StartCoroutine("LevelLoading");
			//GameManager.Instance.SetGameState(GameManager.GAMESTATE.LOADING);
			Application.LoadLevel(sceneName);			
			
			
		}
		catch
		{
			Console.Instance.OutputToConsole(string.Format("{0}: Attempted to load scene {1}, but a scene with that name was not found.", this.ToString(), sceneName), Console.Instance.Style_Error);					
		}
	}
	
	IEnumerable LevelLoading()
	{
		double time = Time.realtimeSinceStartup;
		GameManager.Instance.SetGameState(GameManager.GameStates.STATES.LOADING);
		while(Application.isLoadingLevel)
		{
			
		}
		time = Time.realtimeSinceStartup - time;
		Console.Instance.OutputToConsole(string.Format("{0}: Scene {1} loaded in {2} seconds.", this.ToString(), Application.loadedLevelName, time.ToString()), Console.Instance.Style_Admin);	
		return null;
	}
}
