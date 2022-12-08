using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player))]
public class PlayerSettingsEditor : Editor
{
    Player player;
    SerializedProperty holeVibration;
    SerializedProperty holeVibrationStr;
    SerializedProperty locations;
    SerializedProperty interactable;
    SerializedProperty nberOfTaps;
    SerializedProperty rayRadius;

    private void OnEnable()
    {
        holeVibration = serializedObject.FindProperty("holeVibrationDuration");
        holeVibrationStr = serializedObject.FindProperty("holeVibrationStrength");
        locations = serializedObject.FindProperty("locationLayers");
        interactable = serializedObject.FindProperty("interactableLayer");
        nberOfTaps = serializedObject.FindProperty("numberOfTaps");
        rayRadius = serializedObject.FindProperty("raycastRadius");
        
    }

    private void Awake()
    {
        player = (Player)this.target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(holeVibration);
        EditorGUILayout.PropertyField(holeVibrationStr);
        EditorGUILayout.PropertyField(locations);
        EditorGUILayout.PropertyField(interactable);
        EditorGUILayout.PropertyField(nberOfTaps);
        EditorGUILayout.PropertyField(rayRadius);
        serializedObject.ApplyModifiedProperties();
    }
}
