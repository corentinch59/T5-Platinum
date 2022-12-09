using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player))]
public class PlayerSettingsEditor : Editor
{
    Player player;
    [Header("Vibration Settings")]
    SerializedProperty holeVibration;
    SerializedProperty holeVibrationStr;

    [Header("Locations to check")]
    SerializedProperty locations;
    SerializedProperty nberOfTaps;

    [Header("")]
    SerializedProperty interactable;
    SerializedProperty rayRadius;

    private void OnEnable()
    {
        holeVibration = serializedObject.FindProperty("holeVibrationDuration");
        holeVibrationStr = serializedObject.FindProperty("holeVibrationStrength");
        locations = serializedObject.FindProperty("locationLayers");
        nberOfTaps = serializedObject.FindProperty("numberOfTaps");
        interactable = serializedObject.FindProperty("interactableLayer");
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
