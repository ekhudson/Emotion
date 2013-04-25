using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(GrendelAudioOptions))]
public class GrendelAudioOptionsEditor : GrendelEditor<GrendelAudioOptions>
{
    public override void OnInspectorGUI()
    {
        GUILayout.Label("Channels: ");

        foreach(GrendelAudioChannel channel in Target.AudioChannels)
        {
            GUILayout.BeginHorizontal();

            EditorGUILayout.TextField("Channel Name", channel.ChannelName);

            GUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add New Channel"))
        {
            Target.AudioChannels.Add(new GrendelAudioChannel());
        }
    }

}
