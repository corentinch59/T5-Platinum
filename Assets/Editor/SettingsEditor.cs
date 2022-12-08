using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Settings))]
public class SettingsEditor : Editor
{
    Settings settings;

    private void Awake()
    {
        settings = (Settings)this.target;
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        //settings.PlayerSettings.holeVibrationStrength = EditorGUI.FloatField(EditorGUILayout.BeginVertical();, settings.PlayerSettings.holeVibrationStrength);
        settings.PlayerSettings.locationLayers = EditorGUI.LayerField(EditorGUILayout.BeginVertical(), settings.PlayerSettings.locationLayers);
        EditorGUILayout.EndVertical();

    }
}
