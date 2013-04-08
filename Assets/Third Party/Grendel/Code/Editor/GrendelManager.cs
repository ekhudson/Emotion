using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

public class GrendelManager : Editor
{
    private static GameManager mGameManager = null;

    public static void Update(SceneView sv)
    {
//        if (mGameManager == null)
//        {
//            GUI.Box(new Rect(8,8, 256, 64), "Hey");
//
//            // .HelpBox(new Rect(8,8, 256, 64), "Scene does not contain a Game Manager", MessageType.Warning);
//            mGameManager = (GameManager)GameObject.FindObjectOfType(typeof(GameManager));
//        }
    }

    public static void OnSceneGUI(SceneView sv)
    {
        Vector3 pos = new Vector3(8,8,0);

        pos = SceneView.currentDrawingSceneView.camera.WorldToScreenPoint(pos);


        if (mGameManager == null)
        {
            GUI.Box(new Rect(pos.x,pos.y, 256, 64), "Hey");

            // .HelpBox(new Rect(8,8, 256, 64), "Scene does not contain a Game Manager", MessageType.Warning);
            mGameManager = (GameManager)GameObject.FindObjectOfType(typeof(GameManager));
        }
    }
}
