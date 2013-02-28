using UnityEngine;
using System.Collections;

	/// <summary>
	/// Title: Grendel Engine
	/// Author: Elliot Hudson
	/// Date: Feb 15, 2012
	/// 
	/// Filename: GameManager.cs
	/// 
	/// Summary: Essentially holds information that might need to be
	/// accessed by a variety of objects in the scene, and contains
	/// global references to useful scripts (ie. UserInput, EntityManager)
	///  
	/// </summary>

public class GameManager : Singleton<GameManager>
{
	#region PUBLIC VARIABLES
	public string ApplicationTitle = "Grendel";
	public string ApplicationVersion = "1.0";
	public bool DebugBuild = true;	
	#endregion		
	
	protected override void Awake()
	{		
		base.Awake();		
	}

	// Use this for initialization
	void Start ()
	{				
		Console.Instance.OutputToConsole(string.Format("Starting up {0} {1}", ApplicationTitle, ApplicationVersion), Console.Instance.Style_Admin);
		ConnectionRegistry.Instance.BuildConnections();

	}
	
	void Update ()
	{
		
		
		if (Input.GetKeyDown(KeyCode.R))
		{
			GrendelPlayer.Instance.ResetPlayer();
		}
		
		
	}
	
	void OnGUI()
	{
	
	}
	

}
