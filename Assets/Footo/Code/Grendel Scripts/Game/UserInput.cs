using UnityEngine;
using System.Collections;

public class UserInput : Singleton<UserInput> 
{

     public float MouseSensitivityVertical = 1f;
     public float MouseSensitivityHorizontal = 1f;     
     

     // Use this for initialization
     void Start ()      
     {
          
     }
     
     // Update is called once per frame
     void Update ()
     {
     
          if(Input.GetKeyDown(KeyCode.C))
          {               
               //MainCamera.Instance.CycleCameras();          
          }
          
          if(Input.GetKeyDown(KeyCode.M))
          {               
               MapCamera.Instance.ToggleScript();               
          }

          if(Input.GetKeyDown(KeyCode.Equals))
          {               
               AudioManager.Instance.VolumeUp();
          }
          
          if(Input.GetKeyDown(KeyCode.Minus))
          {               
               AudioManager.Instance.VolumeDown();     
          }
          
          if(Input.GetKeyDown(KeyCode.BackQuote))
          {
               if(GameOptions.Instance.DebugMode){ Console.Instance.ToggleConsole(); }
          }
          
          if(Input.GetAxis("Mouse ScrollWheel") > 0)
          {

          }
          
          if(Input.GetAxis("Mouse ScrollWheel") < 0)
          {

          }


//TODO: Replace with TNManager.player
//          if(Input.GetKey(KeyCode.W))
//          {
//               GrendelPlayer.Instance.MovePlayer(new Vector3(0,1,0));
//          }
//
//          if(Input.GetKey(KeyCode.A))
//          {
//               GrendelPlayer.Instance.MovePlayer(new Vector3(-1,0,0));
//          }
//
//          if(Input.GetKey(KeyCode.D))
//          {
//               GrendelPlayer.Instance.MovePlayer(new Vector3(1,0,0));
//          }
//
//          if(Input.GetKey(KeyCode.S))
//          {
//               GrendelPlayer.Instance.MovePlayer(new Vector3(0,-1,0));
//          }

     }
          
}
