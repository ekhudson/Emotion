using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserInput : Singleton<UserInput> 
{
    [System.Serializable]
    public class KeyBinding
    {
        public string BindingName = "New Binding";
        public KeyCode Key = KeyCode.A;
        public KeyCode AltKey = KeyCode.B;
        public bool Enabled = true;
        public MouseButtons MouseButton = UserInput.MouseButtons.None;
        public MouseButtons AltMouseButton = UserInput.MouseButtons.None;

        public KeyBinding(string bindingName, KeyCode key, KeyCode altKey, MouseButtons mouseButton, MouseButtons altMouseButton)
        {
            BindingName = bindingName;
            Key = key;
            AltKey = altKey;
            MouseButton = mouseButton;
            AltMouseButton = altMouseButton;
        }
    }

    public enum MouseButtons
    {
        None = 0,
        One = 1,
        Two,
        Three,
        Four,
        Five,
        Six,
    }

    public float MouseSensitivityVertical = 1f;
    public float MouseSensitivityHorizontal = 1f;

    [HideInInspector]public KeyBinding MoveUp = new KeyBinding("Move Up", KeyCode.W, KeyCode.UpArrow, MouseButtons.None, MouseButtons.None);
    [HideInInspector]public KeyBinding MoveDown = new KeyBinding("Move Down", KeyCode.S, KeyCode.DownArrow, MouseButtons.None, MouseButtons.None);
    [HideInInspector]public KeyBinding MoveLeft = new KeyBinding("Move Left", KeyCode.A, KeyCode.LeftArrow, MouseButtons.None, MouseButtons.None);
    [HideInInspector]public KeyBinding MoveRight = new KeyBinding("Move Right", KeyCode.D, KeyCode.RightArrow, MouseButtons.None, MouseButtons.None);
    [HideInInspector]public KeyBinding Jump = new KeyBinding("Jump", KeyCode.Space, KeyCode.Return, MouseButtons.None, MouseButtons.None);
    [HideInInspector]public KeyBinding Run = new KeyBinding("Run", KeyCode.LeftShift, KeyCode.RightShift, MouseButtons.None, MouseButtons.None);
    [HideInInspector]public KeyBinding PrimaryFire = new KeyBinding("Primary Fire", KeyCode.None, KeyCode.None, MouseButtons.One, MouseButtons.None);
    [HideInInspector]public KeyBinding SecondaryFire = new KeyBinding("Secondary Fire", KeyCode.None, KeyCode.None, MouseButtons.Two, MouseButtons.None);

    [HideInInspector]public List<KeyBinding> KeyBindings = new List<KeyBinding>();
    
    // Use this for initialization
    private void Start ()
    {
    
    }

    private void StoreKeyBindings()
    {

    }
     
    // Update is called once per frame
    private void Update ()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
    
        }
    
        if(Input.GetKeyDown(KeyCode.M))
        {
    
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
    }
}
