using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MovableObject))]
public class MoveableObjectEditor : GrendelEditorBase<MovableObject>
{	

	public override void OnInspectorGUI()
	{
        if (Target.Positions == null || Target.Positions.Count <= 0)
        {

        }
        else
        {
            int positionCount = 1;

            foreach(MovableObject.MoveableObjectPosition position in Target.Positions)
            {
                EditorGUI.BeginChangeCheck();

                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(string.Format("Position {0}", positionCount.ToString()), EditorStyles.boldLabel);
    
                        if (Application.isPlaying)
                        {
                            if (GUILayout.Button("Move to position"))
                            {
                                Target.MoveToPosition(Target.Positions.IndexOf(position));
                            }
                        }
    
                    EditorGUILayout.EndHorizontal();
                    position.Position = EditorGUILayout.Vector3Field("Position", position.Position);
                    position.Rotation = Quaternion.Euler(EditorGUILayout.Vector3Field("Rotation", position.Rotation.eulerAngles));
                    EditorGUILayout.Space();
                    EditorGUILayout.EndVertical();
                    positionCount++;

                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(Target);
                }
            }
        }

        if (GUILayout.Button("Add Position"))
        {
            Target.Positions.Add(new MovableObject.MoveableObjectPosition());
        }

        base.OnInspectorGUI();
	}

    private void OnSceneGUI()
    {
        Undo.SetSnapshotTarget(Target, "Moveable Object Change");

        int positionCount = 1;
        Vector3 originalPosition;

        if (Application.isEditor && !Application.isPlaying)
        {
            originalPosition = Target.transform.position;
        }
        else
        {
            originalPosition = Target.OriginalPosition;
        }

        foreach(MovableObject.MoveableObjectPosition position in Target.Positions)
        {

            Handles.Label(originalPosition + position.Position, string.Format("Position {0}", positionCount.ToString()), EditorStyles.whiteLabel);
            position.Position = Handles.FreeMoveHandle(originalPosition + position.Position, Quaternion.identity, 0.125f, new Vector3(0.25f, 0.25f, 0.25f), Handles.SphereCap) - originalPosition;
            positionCount++;
        }

        Undo.CreateSnapshot();
        Undo.RegisterSnapshot();
    }

}

