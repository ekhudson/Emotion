using UnityEngine;
using System.Collections;

public class ScreenFlash : MonoBehaviour {

public GUITexture flash;
Color flashColor;

void Start () {

    Texture tex = new Texture();
    //tex.SetPixel( 0 , 0 , Color.white );
    //tex.Apply();      
    flash.pixelInset = new Rect(0 , 0 , Screen.width , Screen.height );
    //flash.color = flashColor;
    //flash.texture = tex;
    //flash.enabled = false;	
	GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height), tex);
}

public void Flash (float duration) {
     flash.enabled = true;
     Invoke("Cancel", duration);
}

void Cancel () {
    
	flash.enabled = false;
}

} //end class