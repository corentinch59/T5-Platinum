using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MyCustomObject)), CanEditMultipleObjects]
public class MyCustomObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Background");

        SerializedProperty backgroundColor = serializedObject.FindProperty("backgroundColor");
        EditorGUILayout.PropertyField(backgroundColor);

        EditorGUILayout.Space(7f);

        EditorGUILayout.LabelField("Text");
        SerializedProperty textColor = serializedObject.FindProperty("textColor");
        EditorGUILayout.PropertyField(textColor);

        SerializedProperty fontSize = serializedObject.FindProperty("fontSize");
        EditorGUILayout.PropertyField(fontSize);

        SerializedProperty fontStyle = serializedObject.FindProperty("fontStyle");
        EditorGUILayout.PropertyField(fontStyle);

        serializedObject.ApplyModifiedProperties();
    }
}