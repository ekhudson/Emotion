using UnityEngine;
using UnityEditor;

using System.Collections;

[CustomEditor(typeof(AdjustableAudioClip))]
public class AdjustableAudioClipEditor : GrendelEditor<AdjustableAudioClip>
{

    public const float kPlayButtonWidth = 192f;

    public static void StaticOnInspectorGUI(AdjustableAudioClip target)
    {
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();

            if (GrendelAudioOptions.PreviewAudioSource != null &&
                GrendelAudioOptions.PreviewAudioSource.isPlaying &&
                GrendelAudioOptions.PreviewAudioSource.clip == target.Clip)
            {
                GUI.color = Color.green;
            }

            if(GUILayout.Button(target.Clip.name, GUILayout.Width(kPlayButtonWidth)))
            {
                GrendelAudioOptions.PlayAudioClipPreview(target);
            }

            GUILayout.Box( AssetPreview.GetAssetPreview(target.Clip) );

            GUI.color = Color.white;

        GUILayout.EndHorizontal();

        target.AttributesExpanded = EditorGUILayout.Foldout(target.AttributesExpanded, "Clip Attributes");

        if (target.AttributesExpanded)
        {
            GUILayout.BeginVertical();

                target.RandomPitch = GUILayout.Toggle(target.RandomPitch, "Random Pitch");

                if (target.RandomPitch)
                {
                    target.PitchMin = Mathf.Clamp(EditorGUILayout.FloatField("Pitch Min", target.PitchMin), 0.0f, Mathf.Infinity);
                    target.PitchMax = Mathf.Clamp(EditorGUILayout.FloatField("Pitch Max", target.PitchMax), 0.0f, Mathf.Infinity);
                }
                else
                {
                    target.Pitch = Mathf.Clamp(EditorGUILayout.FloatField("Pitch", target.Pitch), 0.0f, Mathf.Infinity);
                }

            GUILayout.EndVertical();
        }




        GUILayout.EndVertical();
    }

}
