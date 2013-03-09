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
 [SerializeField]
 public UnityEngine.Object SceneToLoadOnGameLaunch;
 public UnityEngine.Object MainMenuScene;
 
 public static class GameStates
    {
        public enum STATES
        {
            LOADING,
             INTRO,
             MAINMENU,
             OPTIONS,
             RUNNING,
             PAUSED,
             CREDITS,
        }
    }
 #endregion
 
 #region PRIVATE VARIABLES
 
 private GameStates.STATES _gameState = GameStates.STATES.LOADING;
 
 #endregion
 
 #region PROPERTIES
 public GameStates.STATES GameState
 {
     get{return _gameState;}
     
 }
 #endregion
 
 protected override void Awake()
 {       
     base.Awake();       
 }

 // Use this for initialization
 protected virtual void Start ()
 {               
     Console.Instance.OutputToConsole(string.Format("{0}: Starting up {1} {2}",  this.ToString(), ApplicationTitle, ApplicationVersion), Console.Instance.Style_Admin);
     if(SceneToLoadOnGameLaunch != null && Application.loadedLevelName != SceneToLoadOnGameLaunch.name)
     {
         Console.Instance.OutputToConsole(string.Format("{0}: Supposed to start up in Scene {1}, but this is Scene {2}",  this.ToString(), SceneToLoadOnGameLaunch.name, Application.loadedLevel), Console.Instance.Style_Admin);
         Application.LoadLevel(SceneToLoadOnGameLaunch.name);
     }
 }
 
 protected virtual void Update()
 {
     switch (_gameState)
     {
         case GameStates.STATES.LOADING:
         
             if (ComponentsLoaded())
             {
                if (Application.loadedLevelName == MainMenuScene.name)
                {
                    SetGameState(GameStates.STATES.MAINMENU);
                }
                else
                {
                    SetGameState(GameStates.STATES.RUNNING);
                }
             }
                                 
         break;
         
         case GameStates.STATES.INTRO:
         
         break;
         
         case GameStates.STATES.MAINMENU:
         
         break;
         
         case GameStates.STATES.OPTIONS:
         
         break;
         
         case GameStates.STATES.RUNNING:
         
         break;
         
         case GameStates.STATES.PAUSED:
         
         break;
         
         case GameStates.STATES.CREDITS:
         
         break;
     
         default:
     
         break;
     }
 }
 
 public void SetGameState(GameStates.STATES state)
 {
     if (state == _gameState)
     {
         Console.Instance.OutputToConsole(string.Format("{0}: Attempted to set GameState to {1}, but GameState is already set to {2}", this.ToString(), state.ToString(), _gameState.ToString()), Console.Instance.Style_Admin);
         return;
     }
     
     switch(state)
     {
         case GameStates.STATES.LOADING:
         
             switch (_gameState)
             {
                 case GameStates.STATES.LOADING:
                                 
                 break;
                 
                 case GameStates.STATES.INTRO:
                 
                 break;
                 
                 case GameStates.STATES.MAINMENU:
                 
                 break;
                 
                 case GameStates.STATES.OPTIONS:
                 
                 break;
                 
                 case GameStates.STATES.RUNNING:
                 
                 break;
                 
                 case GameStates.STATES.PAUSED:
                 
                 break;
                 
                 case GameStates.STATES.CREDITS:
                 
                 break;
             
                 default:
             
                 break;
             }
         
         break;
         
         case GameStates.STATES.INTRO:
         
             switch (_gameState)
             {
                 case GameStates.STATES.LOADING:
                             
                 break;
                 
                 case GameStates.STATES.INTRO:
                         
                 break;
                 
                 case GameStates.STATES.MAINMENU:
                 
                 break;
                 
                 case GameStates.STATES.OPTIONS:
                 
                 break;
                 
                 case GameStates.STATES.RUNNING:
                 
                 break;
                 
                 case GameStates.STATES.PAUSED:
                 
                 break;
                 
                 case GameStates.STATES.CREDITS:
                 
                 break;
             
                 default:
             
                 break;
             }
         
         break;
         
         case GameStates.STATES.MAINMENU:
         
             switch (_gameState)
             {
                 case GameStates.STATES.LOADING:
                             
                 break;
                 
                 case GameStates.STATES.INTRO:

                    LevelManager.Instance.LoadLevel(MainMenuScene.ToString());
                 
                 break;
                 
                 case GameStates.STATES.MAINMENU:
                     
                 break;
                 
                 case GameStates.STATES.OPTIONS:
                 
                 break;
                 
                 case GameStates.STATES.RUNNING:

                    LevelManager.Instance.LoadLevel(MainMenuScene.ToString());
                 
                 break;
                 
                 case GameStates.STATES.PAUSED:
                 
                 break;
                 
                 case GameStates.STATES.CREDITS:

                    LevelManager.Instance.LoadLevel(MainMenuScene.ToString());
                 
                 break;
             
                 default:
             
                 break;
             }
                
         break;
         
         case GameStates.STATES.OPTIONS:
         
             switch (_gameState)
             {
                 case GameStates.STATES.LOADING:
                                 
                 break;
                 
                 case GameStates.STATES.INTRO:
                 
                 break;
                 
                 case GameStates.STATES.MAINMENU:
                 
                 break;
                 
                 case GameStates.STATES.OPTIONS:
                                         
                 break;
                 
                 case GameStates.STATES.RUNNING:
                 
                 break;
                 
                 case GameStates.STATES.PAUSED:
                 
                 break;
                 
                 case GameStates.STATES.CREDITS:
                 
                 break;
             
                 default:
             
                 break;
             }
         
         break;
         
         case GameStates.STATES.RUNNING:
         
             switch (_gameState)
             {
                 case GameStates.STATES.LOADING:
                                 
                 break;
                 
                 case GameStates.STATES.INTRO:
                 
                 break;
                 
                 case GameStates.STATES.MAINMENU:
                 
                 break;
                 
                 case GameStates.STATES.OPTIONS:
                 
                 break;
                 
                 case GameStates.STATES.RUNNING:
                     
                 break;
                 
                 case GameStates.STATES.PAUSED:
                 
                 break;
                 
                 case GameStates.STATES.CREDITS:
                 
                 break;
             
                 default:
             
                 break;
             }
         
         break;
         
         case GameStates.STATES.PAUSED:
         
             switch (_gameState)
             {
                 case GameStates.STATES.LOADING:
                                 
                 break;
                 
                 case GameStates.STATES.INTRO:
                 
                 break;
                 
                 case GameStates.STATES.MAINMENU:
                 
                 break;
                 
                 case GameStates.STATES.OPTIONS:
                 
                 break;
                 
                 case GameStates.STATES.RUNNING:
                 
                 break;
                 
                 case GameStates.STATES.PAUSED:
                     
                 break;
                 
                 case GameStates.STATES.CREDITS:
                 
                 break;
             
                 default:
             
                 break;
             }
         
         break;
         
         case GameStates.STATES.CREDITS:
         
             switch (_gameState)
             {
                 case GameStates.STATES.LOADING:
                             
                 break;
                 
                 case GameStates.STATES.INTRO:
                 
                 break;
                 
                 case GameStates.STATES.MAINMENU:
                 
                 break;
                 
                 case GameStates.STATES.OPTIONS:
                 
                 break;
                 
                 case GameStates.STATES.RUNNING:
                 
                 break;
                 
                 case GameStates.STATES.PAUSED:
                 
                 break;
                 
                 case GameStates.STATES.CREDITS:
                         
                 break;
             
                 default:
             
                 break;
             }
         
         break;          
         
         default:                

         break;
     }
     
     
     Console.Instance.OutputToConsole(string.Format("{0}: Setting GameState to {1}. Previous state: {2}", this.ToString(), state.ToString(), _gameState.ToString()), Console.Instance.Style_Admin);
     EventManager.Instance.Post(new GameStateEvent(state, _gameState, this));
     _gameState = state;
     
     
 }
 
 private bool ComponentsLoaded()
 {       
     object[] singletons = FindObjectsOfType(typeof(Singleton<>));
     
     foreach(object st in singletons)
     {
         if (st == null)
         {
             return false;
         }
     }
     
     return true;
 }   
}
