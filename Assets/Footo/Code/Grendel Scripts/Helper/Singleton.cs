using UnityEngine;
using System.Collections;
using System.Reflection;

	/// <summary>
	/// Title: Grendel Engine
	/// Author: Elliot Hudson
	/// Date: Mar 28, 2012
	/// 
	/// Filename: Singleton.cs
	/// 
	/// Summary: Subclass in order to create a singleton.
	/// Ensures that only one instance exists in the scene
	/// 
	/// </summary>

public class Singleton<T> : MonoBehaviour where T : class
{ 
	public bool DoNotDestroyOnLoad = false;
    public bool DestroyGameObject = false;
	
	protected static T _instance;
	
	static public T Instance
	{
		get 
		{
//			if (_instance == null)
//			{
//				Debug.LogWarning(string.Format("Component of type <{0}> does not exist in the scene", typeof(T).ToString() ));				
//			}
			
			return _instance;
		}
	}
		
	virtual protected void Awake()
	{
		//checks if there is already an instance in the game
		//and destroys this object if it is a duplicate
		if (_instance == null)
		{
			_instance = this as T;
			if (DoNotDestroyOnLoad){ DontDestroyOnLoad(gameObject); }
		}
		else
		{
            if (DestroyGameObject)
            {
                GameObject.Destroy(this.gameObject);
            }
            else
            {
			   Destroy( this );
            }
		}
	}
			
}
