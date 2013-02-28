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



          if(Input.GetKey(KeyCode.W))
          {
               GrendelPlayer.Instance.MovePlayer(new Vector3(0,1,0));
          }
          
          if(Input.GetKey(KeyCode.A))
          {
               GrendelPlayer.Instance.MovePlayer(new Vector3(-1,0,0));
          }
          
          if(Input.GetKey(KeyCode.D))
          {
               GrendelPlayer.Instance.MovePlayer(new Vector3(1,0,0));
          }
          
          if(Input.GetKey(KeyCode.S))
          {
               GrendelPlayer.Instance.MovePlayer(new Vector3(0,-1,0));
          }

          Vector3 mousePos = MainCamera.Instance.camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
          mousePos = TileManager.Instance.transform.InverseTransformPoint(mousePos);




        Color colorToUse = Color.cyan;
        
        if (!TileManager.Instance.CheckPassability(mousePos))
        {
            colorToUse = Color.red;
        }
        
        Color highlightColor = GrendelColor.FlashingColor(colorToUse, 2f);
        highlightColor.a += 0.12f;
        TileManager.Instance.HighlightTile.color = highlightColor;
        
        TileManager.Instance.HighlightTile.transform.localPosition = TileManager.Instance.GetTileCoordinates(mousePos);
        
        if(Input.GetMouseButtonDown(0) && colorToUse != Color.red)
        {
//            TileManager.TileCoordinate start = new TileManager.TileCoordinate
//                                           ( (int)GrendelPlayer.Instance.transform.localPosition.x / TileManager.Instance.TileSize,
//                                             (int)GrendelPlayer.Instance.transform.localPosition.y / TileManager.Instance.TileSize);
//            TileManager.TileCoordinate end =  new TileManager.TileCoordinate((int)Mathf.Floor(mousePos.x / TileManager.Instance.TileSize),
//                             (int)Mathf.Floor(mousePos.y / TileManager.Instance.TileSize));            
        }

     }
          
}
