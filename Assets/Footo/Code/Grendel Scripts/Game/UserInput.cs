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
        public List<UserInput.KeyBinding> Conflicts = new List<UserInput.KeyBinding>(); //TODO: Figure out the most efficient way to update keybind conflicts

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

    private Dictionary<KeyCode, List<KeyBinding>> mKeyBindingsDictionary = new Dictionary<KeyCode, List<KeyBinding>>();
    private Dictionary<MouseButtons, List<KeyBinding>> mMouseBindingsDictionary = new Dictionary<MouseButtons, List<KeyBinding>>();
    
    // Use this for initialization
    private void Start ()
    {
        GatherKeyBindings();
        StoreKeyBindings();
    }

    //Find all the KeyBindings on UserInput
    public void GatherKeyBindings()
    {
        KeyBindings.Clear();

        System.Type myType = typeof(UserInput);

        System.Reflection.FieldInfo[] myField = myType.GetFields();

        for(int i = 0; i < myField.Length; i++)
        {
            if(myField[i].FieldType == typeof(UserInput.KeyBinding))
            {
                UserInput.KeyBinding binding = (UserInput.KeyBinding)myField[i].GetValue(this);
                if (!KeyBindings.Contains(binding))
                {
                    KeyBindings.Add(binding);
                }
            }
        }
    }

    //Store all the KeyBindings for easy referencing
    private void StoreKeyBindings()
    {
        foreach(KeyBinding binding in KeyBindings)
        {
            if (binding.Key != KeyCode.None)
            {
                if (!mKeyBindingsDictionary.ContainsKey(binding.Key))
                {
                    mKeyBindingsDictionary.Add(binding.Key, new List<KeyBinding>(){ binding } );
                }
                else
                {
                    mKeyBindingsDictionary[binding.Key].Add(binding);
                }
            }

            if (binding.AltKey != KeyCode.None)
            {
                if (!mKeyBindingsDictionary.ContainsKey(binding.AltKey))
                {
                    mKeyBindingsDictionary.Add(binding.AltKey, new List<KeyBinding>(){ binding });
                }
                else
                {
                    mKeyBindingsDictionary[binding.AltKey].Add(binding);
                }
            }

            if (binding.MouseButton != MouseButtons.None)
            {
                if (!mMouseBindingsDictionary.ContainsKey(binding.MouseButton))
                {
                    mMouseBindingsDictionary.Add(binding.MouseButton, new List<KeyBinding>(){ binding });
                }
                else
                {
                    mMouseBindingsDictionary[binding.MouseButton].Add(binding);
                }
            }

            if (binding.AltMouseButton != MouseButtons.None)
            {
                if (!mMouseBindingsDictionary.ContainsKey(binding.AltMouseButton))
                {
                    mMouseBindingsDictionary.Add(binding.AltMouseButton, new List<KeyBinding>(){ binding });
                }
                else
                {
                    mMouseBindingsDictionary[binding.AltMouseButton].Add(binding);
                }
            }
        }
    }
     
    // Update is called once per frame
    private void Update ()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
    
        }
    
        if(Input.GetAxis("Mouse ScrollWheel") < 0)
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
    }

    private void OnGUI()
    {
        Event e = Event.current;

        if (e.keyCode != KeyCode.None)
        {
            if (Input.GetKeyDown(e.keyCode))
            {
                ProcessKeycode(e.keyCode, UserInputKeyEvent.TYPE.KEYDOWN);
            }

            if (Input.GetKey(e.keyCode))
            {
                ProcessKeycode(e.keyCode, UserInputKeyEvent.TYPE.KEYHELD);
            }

            if (Input.GetKeyUp(e.keyCode))
            {
                ProcessKeycode(e.keyCode, UserInputKeyEvent.TYPE.KEYUP);
            }
        }
    }

    private void ProcessKeycode(KeyCode code, UserInputKeyEvent.TYPE inputType)
    {
        if (!mKeyBindingsDictionary.ContainsKey(code))
        {
            return;
        }

        foreach(KeyBinding binding in mKeyBindingsDictionary[code])
        {
            if (binding.Enabled)
            {
                EventManager.Instance.Post(new UserInputKeyEvent(inputType, binding, Vector3.zero, this));
            }
        }
    }
}
