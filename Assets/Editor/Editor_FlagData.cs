using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FlagData))]
public class Editor_FlagData : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space(20f);

        FlagData flagData = target as FlagData;

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            flagData.Save();
        }
        if (GUILayout.Button("Load"))
        {
            flagData.Load();
        }
        GUILayout.EndHorizontal();

        EditorGUILayout.Space(20f);
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Set All True"))
        {
            flagData.SetAllTrue();
        }
        if (GUILayout.Button("Set All False"))
        {
            flagData.SetAllFalse();
        }
        GUILayout.EndHorizontal();
    }
}
