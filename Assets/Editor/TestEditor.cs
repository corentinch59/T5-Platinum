using System.Collections;
using System.Collections.Generic;
using Codice.CM.Common.Serialization;
using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(PlayerTest))]
public class TestEditor : Editor
{
    private Player myPlayer = null;

    public void OnEnable()
    {
        this.myPlayer = (Player)this.target;
    }

    public override void OnInspectorGUI()
    {
        GUIStyle myStyle = new GUIStyle();
        myStyle.fontSize = 15;
        myStyle.fontStyle = FontStyle.Bold;
        myStyle.normal.textColor = Color.white;
            
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Grave Settings", myStyle);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(5f);

        this.myPlayer.distGraveCreation =
            EditorGUILayout.FloatField("Grave's Creation Distance", this.myPlayer.distGraveCreation);


        EditorGUILayout.HelpBox("Set the grave's creation distance.", MessageType.Info);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Dig/Dig Up Settings", myStyle);
        EditorGUILayout.EndHorizontal();
    }
}
