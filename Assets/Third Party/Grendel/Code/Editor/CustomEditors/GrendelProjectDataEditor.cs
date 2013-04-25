using UnityEngine;
using UnityEditor;

using System.Collections;
using System.IO;

[CustomEditor(typeof(GrendelProjectData))]
public class GrendelProjectDataEditor : GrendelEditor<GrendelProjectData>
{
    public static GrendelProjectData CreateProjectDataAsset()
    {
        GrendelProjectData asset = (GrendelProjectData)ScriptableObject.CreateInstance(typeof(GrendelProjectData));  //scriptable object
        if (!Directory.Exists(Path.Combine(Application.dataPath,"Resources\\Grendel\\")))
        {
            Directory.CreateDirectory(Path.Combine(Application.dataPath,"Resources\\Grendel\\"));
        }

        AssetDatabase.CreateAsset(asset, string.Format("Assets/Resources/Grendel/{0}.asset", "Grendel Project Data"));
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;

        return asset;
    }
}
