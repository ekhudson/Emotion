using UnityEngine;
using UnityEditor;

using System.Collections;

[CustomEditor(typeof(ProgressionManager))]
public class ProgressionManagerEditor : GrendelEditor<ProgressionManager>
{
    private int mNumLevels;

    [MenuItem("Colony/Create ProgressionManager Asset")]
    public static void CreateProgressionManager()
    {
        ProgressionManager asset = new ProgressionManager();  //scriptable object
        AssetDatabase.CreateAsset(asset, "Assets/Content/Design/ProgressionManager.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }

    public void OnEnable()
    {
        mNumLevels = Target.Levels.Count;
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

            base.OnInspectorGUI();

            mNumLevels = EditorGUILayout.IntField("Number of Levels", mNumLevels);

        if(EditorGUI.EndChangeCheck())
        {
            if (mNumLevels != Target.Levels.Count)
            {
                Target.Levels.Clear();
                Target.Levels.AddRange(new ProgressionManager.LevelData[mNumLevels]);
            }

            if (Target.LevellingCurve.keys.Length > 2)
            {
                Target.LevellingCurve.keys = new Keyframe[]{Target.LevellingCurve.keys[0], Target.LevellingCurve.keys[1]};
            }

            if (Target.LevellingCurve.keys.Length < 2)
            {
                Target.LevellingCurve.keys = new Keyframe[]{new Keyframe(0, 0), new Keyframe(1, 10000)};
            }

            if (Target.LevellingCurve.keys[ 1 ].time != 1.0f)
            {
                float val = Target.LevellingCurve.keys[ 1 ].value;
                Target.LevellingCurve.MoveKey( 1, new Keyframe(1, val));
            }

            if (Target.LevellingCurve.keys[ 0 ].time != 0.0f)
            {
                Target.LevellingCurve.keys[ 0 ].time = 0.0f;
            }

            foreach (ProgressionManager.LevelData level in Target.Levels)
            {
                if (level == null)
                {
                    continue;
                }

                level.Level = Target.Levels.IndexOf(level);
                level.XPAmount = (int)Target.LevellingCurve.Evaluate((float)level.Level / (float)Target.Levels.Count);
            }

            EditorUtility.SetDirty(Target);
        }

        foreach(ProgressionManager.LevelData level in Target.Levels)
        {
            if (level == null)
            {
                continue;
            }

            GUILayout.BeginHorizontal();

            GUILayout.Label(string.Format("Level {0}", level.Level.ToString()), GUI.skin.box);
            EditorGUILayout.LabelField("XP: ", GUI.skin.label, GUILayout.Width(64));
            EditorGUILayout.LabelField(level.XPAmount.ToString(), GUI.skin.textField);

            GUILayout.FlexibleSpace();

            GUILayout.EndHorizontal();
        }
    }

}
