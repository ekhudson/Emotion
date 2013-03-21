using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

using GrendelEditor.UI;

[CustomEditor(typeof(UserInput))]
public class UserInputEditor : GrendelEditor<UserInput>
{
    private const float kLabelWidth = 64f;
    private const float kButtonWidth = 128f;

    private void OnEnable()
    {
        //Clear the keybindings list and regather all the bindings
        Target.KeyBindings.Clear();

        System.Type myType = typeof(UserInput);

        System.Reflection.FieldInfo[] myField = myType.GetFields();

        for(int i = 0; i < myField.Length; i++)
        {
            if(myField[i].FieldType == typeof(UserInput.KeyBinding))
            {
                UserInput.KeyBinding binding = (UserInput.KeyBinding)myField[i].GetValue(Target);
                if (!Target.KeyBindings.Contains(binding))
                {
                    Target.KeyBindings.Add(binding);
                }
            }
        }
    }

    public override void OnInspectorGUI()
	{
        DrawDefaultInspector();

        foreach(UserInput.KeyBinding binding in Target.KeyBindings)
        {
            GUILayout.BeginVertical(GUI.skin.box);

                string name = binding.BindingName;
                name = name.Replace(" ", string.Empty);

                EditorGUI.indentLevel++;

                GUILayout.Label(name, EditorStyles.boldLabel); //TODO: Replace with something that reflects the variables actual name

                binding.BindingName = EditorGUILayout.TextField("Binding Name: ", binding.BindingName);

                GUILayout.FlexibleSpace();

                GUILayout.BeginHorizontal();

                    GUILayout.FlexibleSpace();

                    CustomEditorGUI.KeyBindButtonLayout(kButtonWidth, 32f, binding, false);

                    GUILayout.FlexibleSpace();

                GUILayout.EndHorizontal();

                GUILayout.FlexibleSpace();
    
                    GUILayout.BeginHorizontal();

                        GUILayout.FlexibleSpace();
    
                        GUILayout.Label("Mouse: ", GUILayout.Width(kLabelWidth));
                        GUILayout.Button(binding.MouseButton.ToString(), GUILayout.Width(kButtonWidth));

                        GUILayout.FlexibleSpace();

                        GUILayout.Label("Alt Mouse: ", GUILayout.Width(kLabelWidth));
                        GUILayout.Button(binding.AltMouseButton.ToString(), GUILayout.Width(kButtonWidth));
    
                        GUILayout.FlexibleSpace();
    
                    GUILayout.EndHorizontal();

            EditorGUI.indentLevel--;

            GUILayout.EndVertical();

            Repaint();
        }


    }
}

